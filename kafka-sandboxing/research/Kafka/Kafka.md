# Kafka
- Event streaming platform with 3 key capabilities:
    - Publish and Subscribe to steams of events (Pub/Sub or Producer/Consumer model)
    - Store streams of events
    - Process stream of events retrospectively
- Distrubuted system consisting of *servers* and *clients* communicating via TCP.

## Servers
- Range from storage units, called brokers to `Kafka Connect` instances that continuously import and export data as event streams.
- Often ran as a cluster that spans multiple datacenters.

## Clients
- Distributed applications and microservices that read, write, and process streams of events.

## Events
- Main unit of communication in Kafka, also known as a record or message.
- A declaration that "something happened", conceptually built with a *key*, *value*, *timestamp*, and optional metadata. For example:
    - Event ```
            Key: Alice
            Value: Made a payment of $200 to Bob
            Timestamp: 202004251406
            ```

## Producers
- Client applications that publish events

## Consumers
- Client applications that are subscribed to events, they read and process events.

## Topics
- Categorization or storing of events. 
- Multi-producer and multi-subscriber, a topic can have zero, one, or many producers as well as zero, one, or many consumers.

### Partitions
- Topics are partitioned, meaning they are spread over a number of buckets located on different Kafka brokers. (Much like partition tables, maybe sharding if a topic can be partitioned accross instances too)
- Kafka guarantees that any consumer of a given topic partition will always read deterministically based on the write order.

### Replication
- Topics can be replicated across different servers. (Much like database replication)


# Core APIs
- [Adimin API](https://kafka.apache.org/documentation.html#adminapi)
- [Producer API](https://kafka.apache.org/documentation.html#producerapi)
- [Consumer API](https://kafka.apache.org/documentation.html#consumerapi)
- [Streams API](https://kafka.apache.org/documentation/streams)
- [Connect API](https://kafka.apache.org/documentation.html#connect)
