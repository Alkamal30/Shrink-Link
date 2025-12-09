#!/bin/bash

# Run ONLY in WSL (if you have Windows)

set -e

source .env

echo "===== Creating Kafka topic ====="
docker exec -it $KAFKA_CONTAINER_NAME \
    ./opt/kafka/bin/kafka-topics.sh \
        --bootstrap-server $KAFKA_BROKER_ADDRESS \
        --create \
        --if-not-exists \
        --topic $KAFKA_REDIRECT_ANALYTICS_TOPIC \
        --partitions $KAFKA_REDIRECT_ANALYTICS_PARTITIONS \
        --replication-factor $KAFKA_REDIRECT_ANALYTICS_REPLICAS


echo "===== Creating ClickHouse DB ====="
curl -s "http://${CLICKHOUSE_ADDRESS}" -u "clickhouse:clickhouse" --data "
    CREATE DATABASE IF NOT EXISTS ${CLICKHOUSE_DB}
"

echo "===== Creating MergeTree ====="
curl -s "http://${CLICKHOUSE_ADDRESS}?database=${CLICKHOUSE_DB}" -u "clickhouse:clickhouse" --data "
    CREATE TABLE IF NOT EXISTS raw_clicks
    (
        short_code String,
        timestamp DateTime64(7),
        ip String,
        user_agent String,
        referer String,
        language String
    )
    ENGINE = MergeTree
    PARTITION BY toYYYYMM(timestamp)
    ORDER BY (short_code, timestamp);
"

echo "===== Creating Kafka Engine ====="
curl -s "http://${CLICKHOUSE_ADDRESS}?database=${CLICKHOUSE_DB}" -u "clickhouse:clickhouse" --data "
    CREATE TABLE IF NOT EXISTS raw_clicks_queue
    (
        short_code String,
        timestamp String,
        ip String,
        user_agent String,
        referer String,
        language String
    )
    ENGINE = Kafka(
        '${KAFKA_BROKER_ADDRESS}',
        '${KAFKA_REDIRECT_ANALYTICS_TOPIC}',
        'clickhouse-redirect-analytics-group',
        'JSONEachRow'
    );
"

echo "===== Creating Materialized View ====="
curl -s "http://${CLICKHOUSE_ADDRESS}?database=${CLICKHOUSE_DB}" -u "clickhouse:clickhouse" --data "
    CREATE MATERIALIZED VIEW IF NOT EXISTS raw_clicks_mv
    TO raw_clicks
    (
        short_code String,
        timestamp DateTime64(7),
        ip String,
        user_agent String,
        referer String,
        language String
    )
    AS SELECT
        short_code,
        parseDateTime64BestEffort(timestamp, 7, 'UTC') AS timestamp,
        ip,
        user_agent,
        referer,
        language
    FROM raw_clicks_queue;
"

echo "===== Done ====="