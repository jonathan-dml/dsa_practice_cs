namespace DsaPractice.HashTables.Tests;

public class Exercise08_LruCacheTests
{
    [Fact]
    public void EvictsTheLeastRecentlyUsedKey()
    {
        var cache = new LruCache(2);

        cache.Put(1, 1);
        cache.Put(2, 2);
        Assert.True(cache.TryGet(1, out int one));
        Assert.Equal(1, one);

        cache.Put(3, 3); // evicts 2
        Assert.False(cache.TryGet(2, out _));

        cache.Put(4, 4); // evicts 1
        Assert.False(cache.TryGet(1, out _));
        Assert.True(cache.TryGet(3, out int three));
        Assert.Equal(3, three);
        Assert.True(cache.TryGet(4, out int four));
        Assert.Equal(4, four);
        Assert.Equal(2, cache.Count);
    }

    [Fact]
    public void UpdatingAKeyRefreshesItWithoutEviction()
    {
        var cache = new LruCache(2);
        cache.Put(1, 1);
        cache.Put(2, 2);

        cache.Put(1, 10); // 1 becomes most recent
        cache.Put(3, 3);  // evicts 2

        Assert.True(cache.TryGet(1, out int value));
        Assert.Equal(10, value);
        Assert.False(cache.TryGet(2, out _));
        Assert.Equal(2, cache.Count);
    }

    [Fact]
    public void CapacityOfOneKeepsOnlyTheLatestKey()
    {
        var cache = new LruCache(1);

        cache.Put(1, 1);
        cache.Put(2, 2);

        Assert.False(cache.TryGet(1, out _));
        Assert.True(cache.TryGet(2, out _));
        Assert.Equal(1, cache.Count);
    }

    [Fact]
    public void MissingKeysAreNotFound()
    {
        var cache = new LruCache(3);

        Assert.False(cache.TryGet(42, out _));
        Assert.Equal(0, cache.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void ThrowsForInvalidCapacity(int capacity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new LruCache(capacity));
    }

    [Fact]
    public void MatchesSimpleModelOnRandomOperations()
    {
        const int capacity = 10;
        var random = new Random(47);
        var cache = new LruCache(capacity);
        var model = new List<(int Key, int Value)>(); // most recent first

        for (int i = 0; i < 20_000; i++)
        {
            int key = random.Next(25);
            int index = model.FindIndex(e => e.Key == key);
            if (random.Next(2) == 0)
            {
                cache.Put(key, i);
                if (index >= 0) model.RemoveAt(index);
                model.Insert(0, (key, i));
                if (model.Count > capacity) model.RemoveAt(model.Count - 1);
            }
            else
            {
                bool found = cache.TryGet(key, out int value);
                Assert.Equal(index >= 0, found);
                if (index >= 0)
                {
                    var entry = model[index];
                    Assert.Equal(entry.Value, value);
                    model.RemoveAt(index);
                    model.Insert(0, entry);
                }
            }

            Assert.Equal(model.Count, cache.Count);
        }
    }

    [Fact]
    public void OperationsAreConstantTime()
    {
        const int capacity = 100_000;
        const int total = 400_000;
        var cache = new LruCache(capacity);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            for (int i = 0; i < total; i++) cache.Put(i, i);

            for (int i = 0; i < total; i++)
            {
                bool found = cache.TryGet(i, out int value);
                if (found != (i >= total - capacity) || (found && value != i)) Assert.Fail($"Unexpected result for key {i}.");
            }
        }, "Pair a dictionary with a doubly linked list so moves and evictions are O(1).");

        Assert.Equal(capacity, cache.Count);
    }
}
