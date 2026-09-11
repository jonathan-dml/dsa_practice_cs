namespace DsaPractice.Heaps.Tests;

public class Exercise07_MedianFinderTests
{
    [Fact]
    public void ComputesRunningMedian()
    {
        var finder = new MedianFinder();

        finder.Add(1);
        Assert.Equal(1.0, finder.Median());
        finder.Add(2);
        Assert.Equal(1.5, finder.Median());
        finder.Add(3);
        Assert.Equal(2.0, finder.Median());
        Assert.Equal(3, finder.Count);
    }

    [Fact]
    public void HandlesUnorderedAndNegativeValues()
    {
        var finder = new MedianFinder();

        foreach (int value in new[] { 5, -10, 3, 8, -1 }) finder.Add(value);
        Assert.Equal(3.0, finder.Median());

        finder.Add(100);
        Assert.Equal(4.0, finder.Median());
    }

    [Fact]
    public void AveragesLargeValuesWithoutOverflow()
    {
        var finder = new MedianFinder();
        finder.Add(int.MaxValue);
        finder.Add(int.MaxValue);

        Assert.Equal(int.MaxValue, finder.Median());

        finder.Add(int.MinValue);
        finder.Add(int.MinValue);
        Assert.Equal(-0.5, finder.Median());
    }

    [Fact]
    public void ThrowsWhenEmpty()
    {
        Assert.Throws<InvalidOperationException>(() => new MedianFinder().Median());
    }

    [Fact]
    public void MatchesSortingOnRandomInputs()
    {
        var random = new Random(112);
        var finder = new MedianFinder();
        var values = new List<int>();

        for (int i = 0; i < 1000; i++)
        {
            int value = random.Next(-500, 500);
            finder.Add(value);
            values.Add(value);

            int[] sorted = values.Order().ToArray();
            double expected = sorted.Length % 2 == 1
                ? sorted[sorted.Length / 2]
                : ((long)sorted[sorted.Length / 2 - 1] + sorted[sorted.Length / 2]) / 2.0;
            Assert.Equal(expected, finder.Median());
        }
    }

    [Fact]
    public void AddIsLogarithmic()
    {
        const int n = 300_000;
        var finder = new MedianFinder();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            for (int i = 0; i < n; i++)
            {
                finder.Add(i % 2 == 0 ? i : -i);
                _ = finder.Median();
            }
        }, "Balance a max-heap of the lower half with a min-heap of the upper half.");

        Assert.Equal(n, finder.Count);
    }
}
