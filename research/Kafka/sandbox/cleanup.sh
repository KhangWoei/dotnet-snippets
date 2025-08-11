#!/bin/bash

SCRIPT_DIR=$(dirname "$0")

docker-compose -f "${SCRIPT_DIR}/kafka-setup.yaml" down
