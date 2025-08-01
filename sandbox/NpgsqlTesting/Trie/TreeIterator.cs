using System.Collections;

namespace NpgsqlTesting.Trie;

public class TreeIterator : IEnumerator<(Node Node, string Path)>
{
    private readonly Node _head;
    private readonly Stack<(Node Node, string Path)> _stack = [];
    private (Node Node, string Path)? _current;

    public TreeIterator(Node head)
    {
        _head = head;
        Reset();
    }

    public bool MoveNext()
    {
        _current = _stack.Count > 0 ? _stack.Pop() : null;

        if (_current is not null)
        {
            foreach (var child in _current.Value.Node.Children)
            {
                var currentPath = string.IsNullOrEmpty(_current.Value.Node.Path)
                    ? string.Empty
                    : string.Join("/", _current.Value.Node.Path, child.Value.Path);
                _stack.Push((child.Value, currentPath));
            }
        }

        return _current is not null;
    }

    public void Reset()
    {
        _stack.Clear();
        _stack.Push((_head, _head.Path));
    }

    (Node Node, string Path) IEnumerator<(Node Node, string Path)>.Current =>
        _current ?? throw new InvalidOperationException();

    object? IEnumerator.Current => _current;

    public void Dispose()
    {
    }
}