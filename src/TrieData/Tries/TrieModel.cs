namespace TrieData.Tries;

internal sealed record TrieModel(int Id, string Name, string BaseUrl) : ITrieModel;

public interface ITrieModel
{
    int Id { get; }
    string Name { get; }
    string BaseUrl { get; }
}