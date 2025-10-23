#!/bin/bash

SCRIPT_DIR=$(dirname "$0")

echo "Creating Kafka environment"
docker-compose -f "${SCRIPT_DIR}/kafka-setup.yaml" up -d 

echo "Creating test-topic"
kafka-topics.sh --create --topic test-topic --bootstrap-server localhost:9092
