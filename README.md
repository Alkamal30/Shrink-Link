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

3. Configure if necessary (`./scripts/.env`)

4. Run
```shell
docker compose up -d
```

### Host (Linux or WSL)

1. Clone repository
```shell
git clone https://github.com/Alkamal30/Shrink-Link.git
```

2. Create Docker network
```shell
docker network create ch-kafka-net
```

3. Run Postgres Docker container
```shell
docker run --name some-postgres -e POSTGRES_PASSWORD=postgres -d -p "5432:5432" postgres
```

4. Run Kafka Docker container
```shell
docker run -d \
    --name some-kafka \
    --network ch-kafka-net \
    -p 9092:9092 -p 29092:29092 \
    -e KAFKA_NODE_ID=1 \
    -e KAFKA_PROCESS_ROLES=broker,controller \
    -e KAFKA_LISTENERS=PLAINTEXT://:9092,PLAINTEXT_HOST://:29092,CONTROLLER://:9093 \
    -e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://some-kafka:9092,PLAINTEXT_HOST://localhost:29092 \
    -e KAFKA_CONTROLLER_LISTENER_NAMES=CONTROLLER \
    -e KAFKA_LISTENER_SECURITY_PROTOCOL_MAP=CONTROLLER:PLAINTEXT,PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT \
    -e KAFKA_CONTROLLER_QUORUM_VOTERS=1@localhost:9093 \
    -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1 \
    -e KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR=1 \
    -e KAFKA_TRANSACTION_STATE_LOG_MIN_ISR=1 \
    -e KAFKA_GROUP_INITIAL_REBALANCE_DELAY_MS=0 \
    -e KAFKA_NUM_PARTITIONS=3 \
    apache/kafka:latest
```

5. Run ClickHouse Docker container
Change `CLICKHOUSE_USER` and `CLICKHOUSE_PASSWORD` if you need.
```shell
docker run -d \
    -p 8123:8123 \
    --network ch-kafka-net \
    --name some-clickhouse \
    --ulimit nofile=262144:262144 \
    -e CLICKHOUSE_USER=clickhouse \
    -e CLICKHOUSE_PASSWORD=clickhouse \
    clickhouse/clickhouse-server
```

6. Run `init-databases-host.sh` script
```shell
./scripts/init-databases-host.sh
```

7. Make development certificates trusted
```shell
dotnet dev-certs https --trust
```

8. Generate gRPC files in Golang:
```shell
protoc --go_out=./src/redirect-service/internal/contracts --go-grpc_out=./src/redirect-service/internal/contracts ./src/redirect-service/internal/contracts/*.proto
```

9. Run projects