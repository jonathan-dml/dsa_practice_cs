namespace DsaPractice.Complexity.Tests;

public class Exercise03_RangeSumQueryTests
{
    [Theory]
    [InlineData(0, 2, 1)]
    [InlineData(2, 5, -1)]
    [InlineData(0, 5, -3)]
    [InlineData(3, 3, -5)]
    [InlineData(5, 5, -1)]
    public void AnswersQueries(int left, int right, long expected)
    {
        var query = new RangeSumQuery([-2, 0, 3, -5, 2, -1]);

        Assert.Equal(expected, query.SumRange(left, right));
    }

    [Fact]
    public void UsesLongForLargeSums()
    {
        var query = new RangeSumQuery([int.MaxValue, int.MaxValue, int.MaxValue]);

        Assert.Equal(3L * int.MaxValue, query.SumRange(0, 2));
    }

    [Theory]
    [InlineData(-1, 2)]
    [InlineData(0, 6)]
    [InlineData(4, 3)]
    public void ThrowsForInvalidRanges(int left, int right)
    {
        var query = new RangeSumQuery([1, 2, 3, 4, 5, 6]);

        Assert.Throws<ArgumentOutOfRangeException>(() => query.SumRange(left, right));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RangeSumQuery(null!));
    }

    [Fact]
    public void AnswersEachQueryInConstantTime()
    {
        const int n = 200_000;
        var random = new Random(3);
        int[] numbers = Enumerable.Range(0, n).Select(_ => random.Next(-1000, 1001)).ToArray();

        long total = numbers.Sum(x => (long)x);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            var query = new RangeSumQuery(numbers);
            for (int i = 0; i < n; i++)
            {
                Assert.Equal(total, query.SumRange(0, n - 1));
            }
        }, "Precompute prefix sums in the constructor.");
    }
}
