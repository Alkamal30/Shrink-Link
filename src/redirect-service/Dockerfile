FROM golang:latest AS build
WORKDIR /src
COPY go.mod go.sum ./
RUN go mod download
COPY . .
# Build a fully static Linux binary (without dependencies)
# It is neccessary to run in Alpine
RUN CGO_ENABLED=0 \
    GOOS=linux \
    GOARCH=amd64 \
    go build -o ./server ./cmd/redirect-service/main.go

FROM alpine:latest
WORKDIR /app
COPY --from=build /src/server .
COPY --from=build /src/internal/configs/*.yaml ./configs/
EXPOSE 8080
ENTRYPOINT ["./server"]