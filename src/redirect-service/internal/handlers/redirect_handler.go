package handlers

import (
	"context"
	"net/http"
	"redirect-service/internal/contracts"
	"time"

	"github.com/gin-gonic/gin"
)

type RedirectHandler struct {
	linkClient contracts.LinkServiceClient
}

func NewRedirectHandler(linkClient contracts.LinkServiceClient) *RedirectHandler {
	return &RedirectHandler{
		linkClient: linkClient,
	}
}

func (h *RedirectHandler) Redirect(c *gin.Context) {
	code := c.Param("code")

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	request := &contracts.GetOriginalLinkRequest{
		Code: code,
	}

	response, err := h.linkClient.GetOriginalLink(ctx, request)
	if err != nil {
		c.JSON(http.StatusNotFound, "Link is not found!")
		return
	}

	c.JSON(http.StatusFound, response.OriginalLink)
}
