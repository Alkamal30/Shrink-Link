# Shrink-Link

## Getting Started

### docker-compose

1. Clone repository
```shell
git clone https://github.com/Alkamal30/Shrink-Link.git
```

2. Generate gRPC files in Golang:
```shell
protoc --go_out=./src/redirect-service/internal/contracts --go-grpc_out=./src/redirect-service/internal/contracts ./src/redirect-service/internal/contracts/*.proto
```

3. Run
```shell
docker compose up -d
```

### Host

1. Clone repository
```shell
git clone https://github.com/Alkamal30/Shrink-Link.git
```

2. Run Postgres Docker container
```shell
docker run --name some-postgres -e POSTGRES_PASSWORD=postgres -d -p "5432:5432" postgres
```

3. Make development certificates trusted
```shell
dotnet dev-certs https --trust
```

4. Generate gRPC files in Golang:
```shell
protoc --go_out=./src/redirect-service/internal/contracts --go-grpc_out=./src/redirect-service/internal/contracts ./src/redirect-service/internal/contracts/*.proto
```

5. Run projects