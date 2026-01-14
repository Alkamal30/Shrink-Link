package app

import (
	"context"
	"log/slog"
	"redirect-service/internal/configs"
	"redirect-service/internal/contracts"
	"redirect-service/internal/grpcclient"
	"redirect-service/internal/handlers"
	"redirect-service/internal/utils"

	"github.com/gin-gonic/gin"
	"github.com/redis/go-redis/v9"
	"github.com/segmentio/kafka-go"
	"go.opentelemetry.io/contrib/bridges/otelslog"
)

func Run() {
	ctx := context.Background()

	shutdown, err := utils.InitOTelLogging(ctx)
	if err != nil {
		slog.Error("Cannot initialize OpenTelemetry logging", "err", err)
	}
	defer shutdown(ctx)

	otelHandler := otelslog.NewHandler("redirect-service")
	logger := slog.New(otelHandler)
	slog.SetDefault(logger)

	slog.Info("Loading configuration")
	cfg, err := configs.LoadConfig()
	if err != nil {
		slog.Error("Cannot load configuration", "err", err)
		panic(err)
	}

	slog.Info("Connecting to gRPC link-service", "url", cfg.Grpc.Url)
	conn, err := grpcclient.NewClientConnection(cfg.Grpc)
	if err != nil {
		slog.Error("Cannot create gRPC client connection", "err", err)
		panic(err)
	}
	defer conn.Close()

	slog.Info("Connecting to Redis", "address", cfg.Redis.Address)
	redisClient := createRedisClient(cfg.Redis)

	slog.Info("Connecting to Kafka", "brokers", cfg.Kafka.Brokers, "topic", cfg.Kafka.RedirectAnalyticsTopic)
	kafkaWriter := kafka.NewWriter(kafka.WriterConfig{
		Brokers: cfg.Kafka.Brokers,
		Topic:   cfg.Kafka.RedirectAnalyticsTopic,
	})
	defer kafkaWriter.Close()

	slog.Info("Initializing Gin engine")
	engine := gin.Default()

	linkClient := contracts.NewLinkServiceClient(conn)
	redirectHandler := handlers.NewRedirectHandler(cfg, linkClient, redisClient, kafkaWriter)

	api := engine.Group("/api")
	{
		api.GET("/redirect/:code", redirectHandler.Redirect)
	}

	slog.Info("Starting server", "url", cfg.Url)
	if err := engine.Run(cfg.Url); err != nil {
		slog.Error("Server failed", "err", err)
		panic(err)
	}
}

func createRedisClient(cfg configs.RedisConfig) *redis.Client {
	return redis.NewClient(&redis.Options{
		Addr:     cfg.Address,
		Password: cfg.Password,
		DB:       0,
		Protocol: 2,
	})
}
