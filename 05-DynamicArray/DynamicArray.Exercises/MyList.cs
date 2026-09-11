using System.Collections;

namespace DsaPractice.DynamicArray;

/// <summary>
/// A growable array, like <see cref="List{T}"/>. Exercises 01–08 implement it step by step;
/// each region below corresponds to one exercise. See README.md for details.
/// </summary>
public class MyList<T> : IEnumerable<T>
{
    // Suggested fields:
    // private T[] _items;
    // private int _count;
    // private int _version;   // incremented by every modification (Exercise 07)

    public const int DefaultCapacity = 4;

    #region Exercise 01 — Construction, Add, Count and Capacity

    public MyList()
    {
        // TODO: start with an empty backing array.
    }

    public MyList(int capacity)
    {
        // TODO: validate capacity and allocate the backing array.
    }

    public int Count => throw new NotImplementedException();

    public int Capacity => throw new NotImplementedException();

    public void Add(T item)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Exercise 02 — Indexer

    public T this[int index]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    #endregion

    #region Exercise 03 — Insert

    public void Insert(int index, T item)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Exercise 04 — RemoveAt and Remove

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Exercise 05 — IndexOf and Contains

    public int IndexOf(T item)
    {
        throw new NotImplementedException();
    }

    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Exercise 06 — Clear and TrimExcess

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public void TrimExcess()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Exercise 07 — Enumeration

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    // Provided: the non-generic version simply forwards to the generic one.
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region Exercise 08 — Reverse and ToArray

    public void Reverse()
    {
        throw new NotImplementedException();
    }

    public T[] ToArray()
    {
        throw new NotImplementedException();
    }

    #endregion
}
