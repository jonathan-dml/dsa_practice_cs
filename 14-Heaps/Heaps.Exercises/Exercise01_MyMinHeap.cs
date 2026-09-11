namespace DsaPractice.Heaps;

/// <summary>
/// Exercise 01 — An array-backed binary min-heap (a max-heap with a reversed comparer). See README.md for details.
/// </summary>
public class MyMinHeap<T>
{
    // Suggested fields:
    // private readonly List<T> _items = new();
    // private readonly IComparer<T> _comparer;

    public MyMinHeap(IComparer<T>? comparer = null)
    {
        // TODO: store comparer ?? Comparer<T>.Default.
    }

    public int Count => throw new NotImplementedException();

    public void Add(T item)
    {
        throw new NotImplementedException();
    }

    public T Peek()
    {
        throw new NotImplementedException();
    }

    public T Poll()
    {
        throw new NotImplementedException();
    }
}
