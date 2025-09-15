CREATE SCHEMA core;

CREATE TABLE core.companies(
id SERIAL PRIMARY KEY,
name TEXT NOT NULL,
created_at TIMESTAMPTZ DEFAULT NOW(),
modiefied_at TIMESTAMPTZ DEFAULT NOW());

CREATE TABLE core.exchanges(
id SERIAL PRIMARY KEY,
code TEXT UNIQUE NOT NULL,
name TEXT NOT NULL
created_at TIMESTAMPTZ DEFAULT NOW(),
modified_at TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE core.symbols(
id SERIAL PRIMARY KEY,
company_id INTEGER NOT NULL REFERENCES companies(id),
exchange_id INTEGER NOT NULL REFERENCES exchanges(id),
symbol TEXT NOT NULL,
created_at TIMESTAMPTZ DEFAULT NOW(),
modified_at TIMESTAMPTZ DEFAULT NOW(),
UNIQUE(company_id, exchange_id),
UNIQUE(symbol, exchange_id));

DO $$
    BEGIN
        EXECUTE 'SET search_path = ' || current_setting('search_path') || ', core';
END $$
