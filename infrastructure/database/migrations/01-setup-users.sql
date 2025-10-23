CREATE ROLE market_data_reader;
CREATE ROLE grafana;
GRANT market_data_reader TO grafana;

CREATE ROLE market_data_writer;
CREATE ROLE csharp;
GRANT market_data_writer TO csharp;
