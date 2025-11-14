package app

import (
	"log"
	"redirect-service/internal/config"
	"redirect-service/internal/contracts"
	"redirect-service/internal/grpcclient"
	"redirect-service/internal/handlers"

	"github.com/gin-gonic/gin"
)

func Run() {
	cfg, err := config.LoadConfig()
	if err != nil {
		log.Panicf("Cannot load configuration: %v", err)
	}

	conn, err := grpcclient.NewClientConnection(cfg.Grpc)
	if err != nil {
		log.Panicf("Cannot create grpc client connection: %v", err)
	}
	defer conn.Close()

	engine := gin.Default()

	linkClient := contracts.NewLinkServiceClient(conn)
	redirectHandler := handlers.NewRedirectHandler(linkClient)

	api := engine.Group("/api")
	{
		api.GET("/redirect/:code", redirectHandler.Redirect)
	}

	engine.Run(cfg.Url)
}
