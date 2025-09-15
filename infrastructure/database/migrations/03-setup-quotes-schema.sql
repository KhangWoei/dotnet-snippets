CREATE SCHEMA quote;

CREATE TABLE quote.quotes(
timestamp TIMESTAMPTZ NOT NULL,
symbol_id INTEGER NOT NULL REFERENCES symbols(id),
price DECIMAL(12,4));

SET search_path = core, quote, public;

