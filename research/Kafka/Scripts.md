# Scripts

Brief documentation of Kafka command line scripts.

# Managing Kafka configuration and metadata

## kafka-server-start

## kafka-server-stop
- Accepts `procss-role` which is the value of a `broker`, `controller`, or `node-id`

## kafka-storage
- Generates cluster UUID 
- Formats storage

## kafka-cluster
- Cluster management

## kafka-features
- Manage feature flags

## kafka-broker-api-versions
- Checks broker information

## kafka-metadata-quorum

## kafka-metadata-shell

## kafka-configs
- Change and describe topic, client, user, broker, IP configuration settings or KRaft controller

# Managing topics, partitions, and replication

## kafka-topics
- Create and delete topics

## kafka-configs

## kafka-get-offsets
- Topic partition offsets

## kafka-leader-election
- Elects new leader for a set of topic partitions

## kafka-transactions
- List and describe transactions

## kafka-reassing-partitions
- Removing partitions between replicas

## kafka-delete-records
- Recommended to use if a topic receives bad data

## kafka-log-dirs
- List of replicas per log

## kafka-replica-verification
- Validate that replicas of a topic contain the same data

## connect-mirror-maker
- Repliace topics from one cluster to another

# Client, producer, and consumer tools

## kafka-client-metrics
- Manipulate and describe client metrics configurations if enabled
- Simpler alternative to `kafka-configure` for client metrics

## kafka-verifiable-consumer
- Consumes messages from a topic and emits consumer events to STDOUT

## kafka-verifiable-producer
- Creates incrementing integers to a specific topic and prints its metadata to STDOUT on each `send` request

## kafka-console-consumer
- Consume records from a topic 

## kafka-console-producer
- Creates record for a topic

## kafka-console-share-consumer
- Consume messags using a "share group"
- Allows multiple consumer instances to jointly consume messages in a queue like fashion

## kafka-producer-perf-test
- Produces large quantity of data for testing purposes

## kafka-groups

## kafka-consumer-groups

## kafka-consumer-perf-test

## kafka-share-groups

# Managing Kafka Connect

TODO: Finish reading docs

# General

## kafka-configs
