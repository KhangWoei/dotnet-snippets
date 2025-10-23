SELECT
  time_bucket('15 minute', q.timestamp) AS time,
  q.price
FROM quote.quotes q
JOIN core.symbols s 
  ON q.symbol_id = s.id
WHERE s.symbol = <symbol>;
