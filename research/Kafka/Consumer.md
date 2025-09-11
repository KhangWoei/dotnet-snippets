# Consumer
Works by issuing "fetch" requests to brokers leading the partitions it wants to consume. The fetch requests specifies the offset which will cause the broker to return a chunk of log beginning from that position. 

Consumers in Kafka implements a pull-based system giving them significant control when to consume. This allows consumers to do things like aggressive batching, and self-managing consumption rate.

However this can lead to issues where consumers end up polling in a tight loop if there's no new data to consume, busy-waiting. (There are ways around this using a "long poll" which basically blocks until data or sufficient amount of data arrives).

## Consumer Position
Most messaging systems keep track of what messages have been consumed on the broker. When a message is sent, it is often marked as *sent* until it has received an acknowledgement from the client then it will be marked as *consumed*. This two-step process is to prevent messages from being lost if a consumer fails to process a message due to some fault. However this can cause issues on performance as brokers must eep multiple states on every message. 

In Kafka, topics are divided into a set of totally ordered partitions where each partition servers only one consumer within each consumer group at any given time. This means that it only needs to keep track of the last consumed message making acknowledgements cheap. 

Articles:
- [Push vs Pull](https://kafka.apache.org/documentation/#design_pull)
[Push vs Pull - karlchris](https://karlchris.github.io/data-engineering/data-ingestion/push-pull/#example-use-case_1)

