using System.Diagnostics.CodeAnalysis;

namespace DsaPractice.HashTables;

/// <summary>
/// Exercise 01 — A hash map using separate chaining and automatic resizing. See README.md for details.
/// </summary>
public class MyHashMap<TKey, TValue>
    where TKey : notnull
{
    public const int InitialBucketCount = 16;
    public const double MaxLoadFactor = 0.75;

    // Suggested design:
    // private sealed class Entry(TKey key, TValue value) { public TKey Key = key; public TValue Value = value; public Entry? Next; }
    // private Entry?[] _buckets = new Entry?[InitialBucketCount];
    // private int _count;

    public int Count => throw new NotImplementedException();

    public int BucketCount => throw new NotImplementedException();

    public void Put(TKey key, TValue value)
    {
        throw new NotImplementedException();
    }

    public TValue Get(TKey key)
    {
        throw new NotImplementedException();
    }

    public bool TryGet(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        throw new NotImplementedException();
    }

    public bool ContainsKey(TKey key)
    {
        throw new NotImplementedException();
    }

    public bool Remove(TKey key)
    {
        throw new NotImplementedException();
    }
}
