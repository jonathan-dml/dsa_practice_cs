using System.Collections;

namespace DsaPractice.LinkedLists;

/// <summary>
/// Exercise 01 — A singly linked list with head and tail references. See README.md for details.
/// </summary>
public class MySinglyLinkedList<T> : IEnumerable<T>
{
    // Suggested design:
    // private sealed class Node(T value) { public T Value = value; public Node? Next; }
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

    public T PeekFirst()
    {
        throw new NotImplementedException();
    }

    public T PeekLast()
    {
        throw new NotImplementedException();
    }

    public bool Contains(T value)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    // Provided: the non-generic version simply forwards to the generic one.
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
