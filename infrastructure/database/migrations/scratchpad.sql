 -- consider using different schemas for better data seperation
CREATE TABLE companies(
id SERIAL PRIMARY KEY,
name TEXT NOT NULL,
created_at TIMESTAMPTZ DEFAULT NOW(),
modiefied_at TIMESTAMPTZ DEFAULT NOW());

CREATE TABLE exchanges(
id SERIAL PRIMARY KEY,
code TEXT UNIQUE NOT NULL,
name TEXT NOT NULL
created_at TIMESTAMPTZ DEFAULT NOW(),
modified_at TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE symbols(
id SERIAL PRIMARY KEY,
company_id INTEGER NOT NULL REFERENCES companies(id),
exchange_id INTEGER NOT NULL REFERENCES exchanges(id),
symbol TEXT NOT NULL,
created_at TIMESTAMPTZ DEFAULT NOW(),
modified_at TIMESTAMPTZ DEFAULT NOW(),
UNIQUE(company_id, exchange_id),
UNIQUE(symbol, exchange_id));

CREATE TABLE quotes(
timestamp TIMESTAMPTZ NOT NULL,
symbol_id INTEGER NOT NULL REFERENCES symbols(id),
price DECIMAL(12,4));
