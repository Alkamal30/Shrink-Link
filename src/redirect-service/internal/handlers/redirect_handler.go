package handlers

import (
	"context"
	"encoding/json"
	"log"
	"net/http"
	"redirect-service/internal/analytics"
	"redirect-service/internal/configs"
	"redirect-service/internal/contracts"
	"time"

	"github.com/gin-gonic/gin"
	"github.com/redis/go-redis/v9"
	"github.com/segmentio/kafka-go"
)

type RedirectHandler struct {
	appConfig   *configs.AppConfig
	linkClient  contracts.LinkServiceClient
	redisClient *redis.Client
	kafkaWriter *kafka.Writer
}

func NewRedirectHandler(
	appConfig *configs.AppConfig,
	linkClient contracts.LinkServiceClient,
	redisClient *redis.Client,
	kafkaWriter *kafka.Writer) *RedirectHandler {
	return &RedirectHandler{
		appConfig:   appConfig,
		linkClient:  linkClient,
		redisClient: redisClient,
		kafkaWriter: kafkaWriter,
	}
}

func (h *RedirectHandler) Redirect(c *gin.Context) {
	var (
		statusCode int
		response   string
	)
	code := c.Param("code")

	ctx, cancel := context.WithTimeout(c.Request.Context(), 5*time.Second)
	defer cancel()

	result, err := h.redisClient.Get(ctx, code).Result()
	if err == nil {
		statusCode = http.StatusFound
		response = result
	} else {
		linkRequest := &contracts.GetOriginalLinkRequest{
			Code: code,
		}

		linkResponse, err := h.linkClient.GetOriginalLink(ctx, linkRequest)
		if err != nil {
			statusCode = http.StatusNotFound
			response = "Link is not found!"
		} else {
			err := h.redisClient.Set(ctx, code, linkResponse.OriginalLink, 24*time.Hour).Err()
			if err != nil {
				log.Printf("Could not add value to Redis. Error: %v", err)
			}

			statusCode = http.StatusFound
			response = linkResponse.OriginalLink
		}
	}

	if statusCode == http.StatusFound {
		h.sendAnalyticsData(c)
	}

	c.JSON(statusCode, response)
}

func (h *RedirectHandler) sendAnalyticsData(ginContext *gin.Context) {
	message, err := json.Marshal(h.buildAnalyticsData(ginContext))
	if err != nil {
		log.Printf("Could not to marshal RedirectAnalyticsData to JSON. Error: %v", err)
		return
	}

	err = h.kafkaWriter.WriteMessages(context.Background(), kafka.Message{
		Value: message,
	})
	if err != nil {
		log.Printf("Could not to send message to Kafka broker. Error: %v", err)
	}
}

func (h *RedirectHandler) buildAnalyticsData(ginContext *gin.Context) *analytics.RedirectAnalyticsData {
	return &analytics.RedirectAnalyticsData{
		ShortCode: ginContext.Param("code"),
		Timestamp: time.Now().UTC(),
		Ip:        ginContext.ClientIP(),
		UserAgent: ginContext.Request.UserAgent(),
		Referer:   ginContext.Request.Referer(),
		Language:  ginContext.Request.Header.Get("Accept-Language"),
	}
}
