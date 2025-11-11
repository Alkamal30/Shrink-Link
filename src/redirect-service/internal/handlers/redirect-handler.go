package handlers

import (
	"net/http"

	"github.com/gin-gonic/gin"
)

// TODO: Replace this logic with a new
func Redirect(context *gin.Context) {
	code := context.Param("code")

	context.JSON(http.StatusFound, "localhost/test/"+code)
}
