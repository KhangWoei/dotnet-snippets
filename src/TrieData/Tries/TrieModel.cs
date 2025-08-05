namespace TrieData.Tries;

internal sealed record TrieModel(int Id, string Name) : ITrieModel;

public interface ITrieModel
{
    int Id { get; }
    string Name { get; }
}