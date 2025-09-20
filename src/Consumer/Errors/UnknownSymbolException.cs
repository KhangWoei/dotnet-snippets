namespace Consumer.Errors;

internal class UnknownSymbolException(string symbol) : Exception($"Unknown symbol: {symbol}")
{
    public string Symbol { get; } = symbol;
}