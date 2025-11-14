package main

import (
	"redirect-service/internal/utils"

	"github.com/gin-gonic/gin"
)

func main() {
	engine := gin.Default()

	utils.SetupRoutes(engine)

	engine.Run("localhost:8080")
}
