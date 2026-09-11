using System.Collections;

namespace DsaPractice.LinkedLists;

/// <summary>
/// Exercise 08 — A doubly linked list with O(1) operations at both ends. See README.md for details.
/// </summary>
public class MyDoublyLinkedList<T> : IEnumerable<T>
{
    // Suggested design:
    // private sealed class Node(T value) { public T Value = value; public Node? Previous; public Node? Next; }
    // private Node? _head;
    // private Node? _tail;
    // private int _count;

    public int Count => throw new NotImplementedException();

    public void AddFirst(T value)
    {
        throw new NotImplementedException();
    }

    public void AddLast(T value)
    {
        throw new NotImplementedException();
    }

    public T RemoveFirst()
    {
        throw new NotImplementedException();
    }

    public T RemoveLast()
    {
        throw new NotImplementedException();
    }

    public T PeekFirst()
    {
        throw new NotImplementedException();
    }

    public T PeekLast()
    {
        throw new NotImplementedException();
    }

    /// <summary>Enumerates from the first element to the last.</summary>
    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    /// <summary>Enumerates from the last element to the first, following Previous references.</summary>
    public IEnumerable<T> Backwards()
    {
        throw new NotImplementedException();
    }

    // Provided: the non-generic version simply forwards to the generic one.
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
