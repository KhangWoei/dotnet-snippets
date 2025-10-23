using System.Collections;

namespace TrieData;

public class TreeIterator : IEnumerator<Node>
{
    private readonly Node _head;
    private readonly Stack<Node> _stack = [];
    private Node? _current;

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
            foreach (var child in _current.Children.Values)
            {
                _stack.Push(child);
            }
        }

        return _current is not null;
    }

    public void Reset()
    {
        _stack.Clear();
        _stack.Push(_head);
    }

    Node IEnumerator<Node>.Current =>
        _current ?? throw new InvalidOperationException();

    object? IEnumerator.Current => _current;

    public void Dispose()
    {
    }
}
