-- SELECT create_hypertable(
--     'quote.quotes',
--     by_range('timestamp', INTERVAL '7 day'),
--     by_hash('symbol_id', 100));

SELECT create_hypertable('quote.quotes', 'timestamp', chunk_time_interval => INTERVAL '7 day', partitioning_column => 'symbol_id', number_partitions => 100);
