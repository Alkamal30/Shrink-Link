package app

import (
	"log"
	"redirect-service/internal/configs"
	"redirect-service/internal/contracts"
	"redirect-service/internal/grpcclient"
	"redirect-service/internal/handlers"

	"github.com/gin-gonic/gin"
	"github.com/redis/go-redis/v9"
)

func Run() {
	cfg, err := configs.LoadConfig()
	if err != nil {
		log.Panicf("Cannot load configuration: %v", err)
	}

	conn, err := grpcclient.NewClientConnection(cfg.Grpc)
	if err != nil {
		log.Panicf("Cannot create grpc client connection: %v", err)
	}
	defer conn.Close()

	redisClient := createRedisClient(cfg.Redis)

	engine := gin.Default()

	linkClient := contracts.NewLinkServiceClient(conn)
	redirectHandler := handlers.NewRedirectHandler(linkClient, redisClient)

	api := engine.Group("/api")
	{
		api.GET("/redirect/:code", redirectHandler.Redirect)
	}

	engine.Run(cfg.Url)
}

func createRedisClient(cfg configs.RedisConfig) *redis.Client {
	return redis.NewClient(&redis.Options{
		Addr:     cfg.Address,
		Password: cfg.Password,
		DB:       0,
		Protocol: 2,
	})
}
