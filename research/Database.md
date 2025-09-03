# Database

Will be using a time series database due to the nature of data that is being collected.

## Time series database
Databases designed for handling datasets that represent the measurements or events that are tracked, monitored, downsampled, and aggregated over time.

### Foundation
Optimized to handle data that arrives in time order. Particularly designed to store, query, and analyze data points with a timestamp associated with them.

Time is treated as a first class citizen allowing for:

- High write throughput: Optimized for large volumes of sequential writes, making it easy to ingest high throughput streams without bottlenecks

- Efficient queries on time range: Optimized for time-range queries (How?_

- Data retention and compression: Typically contains retention policies, automatically expiring or downsampling older data. Also apply compression algorithms, significantly reducing storage requirements.

### Implementations

#### TimescaleDB

#### InfluxDB

#### ClickHouse


