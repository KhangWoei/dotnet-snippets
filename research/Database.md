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

## TimescaleDB

### [Hypertables](https://docs.tigerdata.com/use-timescale/latest/hypertables/)
Automatically partitions data across multiple chunks based on time enabling efficient storage and querying of time-series data at scale.

#### Chunks
The partitions of a hypertable, each chunk is assigned a range of time and only contains data from that range. Defaults to a range of 7 days but can be overriden with the `chunk_interval` parameter.

Alternatively, hypertables can also be partitioned by dimension (space), recommended to partition on a non-time column and can be altered/increased as the data grows (only affecting chunks created after the change).


```
CREATE TABLE conditions(
    "time" timestamptz not null,
    device_id integer,
    temperature float
)
WITH (
    timescaledb.hypertable,
    timescaledb.partition_column='time',
    timescaledb.chunk_interval='1 day'
);

# Adding a hash partition on a non time column
SELECT * FROM add_dimension('conditions', by_hash('device_id', 3));

# Altering partition
SELECT SET_NUMBER_PARTITIONS('conditions', 5, 'device_id');

```

#### Indexes
Created by default on time, descending. Can be prevented by setting the `create_default_indexes` to `false`.

If a unique index is required, it must include all the partitioning columns for the table.

### [Hyperfunctions](https://docs.tigerdata.com/use-timescale/latest/hyperfunctions/about-hyperfunctions/)
Specialized set of functions that power real-time analytics on time series and events. 

### Continuous aggregates
A kind of hypertable that is refreshed automatically in the background as new data is added, or when old data is modified. 

#### Components

1. Materialization hypertables

Takes raw data from the original hypertable, aggregate it, and stores the results in a materialization hypertable. 

2. Materialization engine
Engine that aggregates data from the original hypertable to the materialization table. It runs two transactions:

- Transaction 1 blocks all INSERTs, UPDATEs, and DELETEs, to determine the time range to materialize, and update the invalidation threshold
- Transaction 2 unblocks other transactions, and materializes the aggregates.

3. Invalidation engine
Determines when data needs to be re-materialized. Checks to ensure that the system does not become swamped with invalidations. Has the concept of a `materialization threshold` which is used to determine how much new data can be materialized without invalidating the continous aggregate. Threshold is always recalculated on materialization and ensures that it lags behind a point-in-time where data changes are common and that most INSERTs do not require extra writes.

When data older than the trheshold is changed, the max and min timestamps of the changed rows are logged and the values are used to determine which rows in the aggregation table needs to be recalculated.

#### [Creating a continuous aggregate](https://docs.tigerdata.com/use-timescale/latest/continuous-aggregates/create-a-continuous-aggregate/)
```
CREATE MATERIALIZED VIEW conditions_summary_daily
WITH (timescaledb.continuous) AS 
SELECT device
    ...
FROM conditions
GROUP BY ...;

# Setting refresh policy
SELECT add_continuous_aggregate_policy('conditions_summary_daily',
    start_offset => INTERVAL '1 month',
    end_offset => INTERVAL '1 day',
    schedule_interval => INTERVAL '1 hour'
);
```

### Data retention
TODO - This exists, I don't need to know about it now.
