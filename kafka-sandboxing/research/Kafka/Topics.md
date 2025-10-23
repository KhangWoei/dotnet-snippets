# Topics
In Kafka. topics are distributed commit logs. 

## Commit logs
A sequentially ordered, append-only log. Often used in databases, distributed systems, and version control system to track changes. They tend to provide a historical record of actions or events, guaranteeing their order, immutability, and persistence within a system.


## Distributed Commit Log
An extension to commit logs by spreading the commit logs' storage and operation across multiple servers or nodes, in Kafka's case "brokers". Kafka's achieves this using partitions and replication.

### Partitions
Each topic is divided into partitions, essentially breaking down a single commit log into multiple, ordered, immutable logs. For every message in kafka, there's an optional "partition key" that is then hashed in order to figure out which partition a message belongs to. If a partition key is absent, Kafka does a round-robin approach (essentially a sequential manner).

Kafka guarantees the order of records within a single partition but not across different partitions.


### [Replication](https://docs.confluent.io/kafka/design/replication.html)
Kafka will replicate each partition across a configurable number of brokers defined by the replication factor. 

Within each partition's set of replicas, one broker is designated as the leader, handling all read and write requests for that partition. The remaining brokers in the replica set are followers which will passively replicate the leader's data.

Kafka ensures a message is considered committed only after it has been successfully written to the leader and replicated to all in sync replicasa.

If a leader broker fails, Kafka automatically elects a new leader from the remaining in sync replicas. (strict quorum)
