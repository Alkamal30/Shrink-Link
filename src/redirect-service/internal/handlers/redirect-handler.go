package handlers

import (
	"context"
	"log"
	"net/http"
	"redirect-service/internal/contracts"
	"time"

	"github.com/gin-gonic/gin"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

func Redirect(с *gin.Context) {
	code := с.Param("code")

	var opts []grpc.DialOption
	opts = append(opts, grpc.WithTransportCredentials(insecure.NewCredentials()))
	conn, err := grpc.NewClient("localhost:50051", opts...)
	if err != nil {
		log.Fatalf("Failed to connect: %v!\n", err)
	}

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	client := contracts.NewLinkServiceClient(conn)

	request := &contracts.GetOriginalLinkRequest{
		Code: code,
	}

	response, err := client.GetOriginalLink(ctx, request)
	if err != nil {
		log.Fatalf("ERROR: %v!", err)
	}

	с.JSON(http.StatusFound, response.OriginalLink)
}
