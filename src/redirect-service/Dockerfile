FROM golang:latest AS build
WORKDIR /src
RUN apt-get update && apt-get install -y protobuf-compiler
RUN go install google.golang.org/protobuf/cmd/protoc-gen-go@latest && \
    go install google.golang.org/grpc/cmd/protoc-gen-go-grpc@latest
COPY src/redirect-service/go.mod src/redirect-service/go.sum ./
RUN go mod download
COPY src/redirect-service/ .
RUN protoc --go_out=./internal/contracts --go-grpc_out=./internal/contracts/ ./internal/contracts/*.proto
# Build a fully static Linux binary (without dependencies)
# It is neccessary to run in Alpine
RUN CGO_ENABLED=0 \
    GOOS=linux \
    GOARCH=amd64 \
    go build -ldflags="-s -w" -o ./server ./cmd/redirect-service/main.go

FROM alpine:latest
WORKDIR /app
COPY --from=build /src/server .
COPY --from=build /src/internal/configs/*.yaml ./configs/
EXPOSE 8080
ENTRYPOINT ["./server"]