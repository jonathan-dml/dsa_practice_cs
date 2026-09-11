namespace DsaPractice.Heaps.Tests;

public class Exercise03_KthLargestInStreamTests
{
    [Fact]
    public void TracksTheKthLargestValue()
    {
        var stream = new KthLargestInStream(3, [4, 5, 8, 2]);

        Assert.Equal(4, stream.Add(3));
        Assert.Equal(5, stream.Add(5));
        Assert.Equal(5, stream.Add(10));
        Assert.Equal(8, stream.Add(9));
        Assert.Equal(8, stream.Add(4));
    }

    [Fact]
    public void KOfOneTracksTheMaximum()
    {
        var stream = new KthLargestInStream(1, []);

        Assert.Equal(-3, stream.Add(-3));
        Assert.Equal(-2, stream.Add(-2));
        Assert.Equal(-2, stream.Add(-4));
        Assert.Equal(0, stream.Add(0));
    }

    [Fact]
    public void ThrowsUntilKValuesWereSeen()
    {
        var stream = new KthLargestInStream(2, []);

        Assert.Throws<InvalidOperationException>(() => stream.Add(5));

        Assert.Equal(3, stream.Add(3));
        Assert.Equal(5, stream.Add(7));
    }

    [Fact]
    public void ThrowsForInvalidArguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new KthLargestInStream(0, [1]));
        Assert.Throws<ArgumentNullException>(() => new KthLargestInStream(1, null!));
    }

    [Fact]
    public void MatchesSortingOnRandomStreams()
    {
        var random = new Random(105);
        for (int round = 0; round < 20; round++)
        {
            int k = random.Next(1, 10);
            var seen = Enumerable.Range(0, k).Select(_ => random.Next(-50, 50)).ToList();
            var stream = new KthLargestInStream(k, seen);

            for (int i = 0; i < 200; i++)
            {
                int value = random.Next(-50, 50);
                seen.Add(value);

                Assert.Equal(seen.OrderDescending().ElementAt(k - 1), stream.Add(value));
            }
        }
    }

    [Fact]
    public void AddIsLogarithmicInK()
    {
        const int k = 1000;
        var random = new Random(106);
        int[] initial = Enumerable.Range(0, k).Select(_ => random.Next()).ToArray();
        int[] values = Enumerable.Range(0, 300_000).Select(_ => random.Next()).ToArray();
        int expected = initial.Concat(values).OrderDescending().ElementAt(k - 1);

        int last = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            var stream = new KthLargestInStream(k, initial);
            int result = 0;
            foreach (int v in values) result = stream.Add(v);
            return result;
        }, "Keep a min-heap with only the k largest values.");

        Assert.Equal(expected, last);
    }
}
