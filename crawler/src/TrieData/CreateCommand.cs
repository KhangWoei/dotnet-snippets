using MediatR;

namespace TrieData;

public sealed record CreateTrieCommandRequest(Trie Trie) : IRequest;