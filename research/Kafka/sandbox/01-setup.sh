#!/bin/bash

SCRIPT_DIR=$(dirname "$0")

docker-compose -f "${SCRIPT_DIR}/kafka-setup.yaml" up -d 

kafka-topics.sh --create --topic test-topic --bootstrap-server localhost:9092
