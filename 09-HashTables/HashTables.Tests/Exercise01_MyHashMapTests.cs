namespace DsaPractice.HashTables.Tests;

public class Exercise01_MyHashMapTests
{
    [Fact]
    public void NewMapIsEmpty()
    {
        var map = new MyHashMap<string, int>();

        Assert.Equal(0, map.Count);
        Assert.Equal(16, map.BucketCount);
    }

    [Fact]
    public void PutThenGet()
    {
        var map = new MyHashMap<string, int>();

        map.Put("one", 1);
        map.Put("two", 2);

        Assert.Equal(1, map.Get("one"));
        Assert.Equal(2, map.Get("two"));
        Assert.Equal(2, map.Count);
    }

    [Fact]
    public void PutReplacesExistingValue()
    {
        var map = new MyHashMap<string, string>();

        map.Put("key", "old");
        map.Put("key", "new");

        Assert.Equal("new", map.Get("key"));
        Assert.Equal(1, map.Count);
    }

    [Fact]
    public void GetThrowsForMissingKey()
    {
        var map = new MyHashMap<int, int>();
        map.Put(1, 1);

        Assert.Throws<KeyNotFoundException>(() => map.Get(2));
    }

    [Fact]
    public void TryGetReportsPresence()
    {
        var map = new MyHashMap<int, string>();
        map.Put(7, "seven");

        Assert.True(map.TryGet(7, out var found));
        Assert.Equal("seven", found);
        Assert.False(map.TryGet(8, out _));
    }

    [Fact]
    public void ContainsKeyReportsPresence()
    {
        var map = new MyHashMap<string, int>();
        map.Put("a", 0);

        Assert.True(map.ContainsKey("a"));
        Assert.False(map.ContainsKey("A"));
    }

    [Fact]
    public void RemoveDeletesKeys()
    {
        var map = new MyHashMap<string, int>();
        map.Put("a", 1);
        map.Put("b", 2);

        Assert.True(map.Remove("a"));
        Assert.False(map.Remove("a"));
        Assert.False(map.ContainsKey("a"));
        Assert.True(map.ContainsKey("b"));
        Assert.Equal(1, map.Count);
    }

    [Fact]
    public void NullKeysThrow()
    {
        var map = new MyHashMap<string, int>();

        Assert.Throws<ArgumentNullException>(() => map.Put(null!, 1));
        Assert.Throws<ArgumentNullException>(() => map.Get(null!));
        Assert.Throws<ArgumentNullException>(() => map.TryGet(null!, out _));
        Assert.Throws<ArgumentNullException>(() => map.ContainsKey(null!));
        Assert.Throws<ArgumentNullException>(() => map.Remove(null!));
    }

    [Fact]
    public void HandlesKeysThatAllCollide()
    {
        var map = new MyHashMap<FixedHashKey, int>();
        for (int i = 0; i < 100; i++) map.Put(new FixedHashKey(i, 42), i * 10);

        for (int i = 0; i < 100; i += 2) Assert.True(map.Remove(new FixedHashKey(i, 42)));

        Assert.Equal(50, map.Count);
        for (int i = 0; i < 100; i++)
        {
            Assert.Equal(i % 2 == 1, map.TryGet(new FixedHashKey(i, 42), out int value));
            if (i % 2 == 1) Assert.Equal(i * 10, value);
        }
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(-17)]
    [InlineData(int.MaxValue)]
    public void HandlesNegativeAndExtremeHashCodes(int hash)
    {
        var map = new MyHashMap<FixedHashKey, string>();
        var key = new FixedHashKey(1, hash);

        map.Put(key, "value");

        Assert.Equal("value", map.Get(new FixedHashKey(1, hash)));
        Assert.True(map.Remove(key));
    }

    [Theory]
    [InlineData(12, 16)]
    [InlineData(13, 32)]
    [InlineData(24, 32)]
    [InlineData(25, 64)]
    [InlineData(100, 256)]
    public void DoublesBucketsWhenLoadFactorIsExceeded(int keys, int expectedBuckets)
    {
        var map = new MyHashMap<int, int>();

        for (int i = 0; i < keys; i++) map.Put(i, i);

        Assert.Equal(expectedBuckets, map.BucketCount);
        for (int i = 0; i < keys; i++) Assert.Equal(i, map.Get(i));
    }

    [Fact]
    public void ReplacingValuesDoesNotResize()
    {
        var map = new MyHashMap<int, int>();
        for (int i = 0; i < 12; i++) map.Put(i, i);

        for (int i = 0; i < 12; i++) map.Put(i, -i);

        Assert.Equal(16, map.BucketCount);
        Assert.Equal(12, map.Count);
    }

    [Fact]
    public void MatchesDictionaryOnRandomOperations()
    {
        var random = new Random(41);
        var map = new MyHashMap<int, int>();
        var expected = new Dictionary<int, int>();

        for (int i = 0; i < 20_000; i++)
        {
            int key = random.Next(-500, 500);
            switch (random.Next(3))
            {
                case 0:
                    map.Put(key, i);
                    expected[key] = i;
                    break;
                case 1:
                    Assert.Equal(expected.Remove(key), map.Remove(key));
                    break;
                default:
                    Assert.Equal(expected.TryGetValue(key, out int e), map.TryGet(key, out int a));
                    Assert.Equal(e, a);
                    break;
            }

            Assert.Equal(expected.Count, map.Count);
        }
    }

    [Fact]
    public void OperationsAreConstantTimeOnAverage()
    {
        const int n = 300_000;
        var map = new MyHashMap<int, int>();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            for (int i = 0; i < n; i++) map.Put(i * 7, i);
            for (int i = 0; i < n; i++)
            {
                if (map.Get(i * 7) != i) Assert.Fail("Wrong value.");
            }
        }, "Resize when the load factor is exceeded so chains stay short.");

        Assert.Equal(n, map.Count);
    }

    private sealed class FixedHashKey(int id, int hash) : IEquatable<FixedHashKey>
    {
        public int Id { get; } = id;

        public bool Equals(FixedHashKey? other) => other is not null && other.Id == Id;

        public override bool Equals(object? obj) => Equals(obj as FixedHashKey);

        public override int GetHashCode() => hash;
    }
}
