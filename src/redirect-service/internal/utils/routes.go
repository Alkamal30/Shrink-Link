package utils

import (
	"redirect-service/internal/handlers"

	"github.com/gin-gonic/gin"
)

func SetupRoutes(engine *gin.Engine) {
	api := engine.Group("/api")
	{
		api.GET("/redirect/:code", handlers.Redirect)
	}
}
