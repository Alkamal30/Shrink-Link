package handlers

import (
	"context"
	"log"
	"net/http"
	"redirect-service/internal/contracts"
	"time"

	"github.com/gin-gonic/gin"
	"github.com/redis/go-redis/v9"
)

type RedirectHandler struct {
	linkClient  contracts.LinkServiceClient
	redisClient *redis.Client
}

func NewRedirectHandler(linkClient contracts.LinkServiceClient, redisClient *redis.Client) *RedirectHandler {
	return &RedirectHandler{
		linkClient:  linkClient,
		redisClient: redisClient,
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

	c.JSON(statusCode, response)
}
