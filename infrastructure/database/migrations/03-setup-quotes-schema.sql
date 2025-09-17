CREATE SCHEMA quote;

CREATE TABLE quote.quotes(
timestamp TIMESTAMPTZ NOT NULL,
symbol_id INTEGER NOT NULL REFERENCES symbols(id),
price DECIMAL(12,4));

DO $$
    BEGIN
        EXECUTE 'SET search_path = ' || current_setting('search_path') || ', quote';
END $$

