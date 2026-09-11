namespace DsaPractice.Complexity.Tests;

public class Exercise01_CountPairsWithSumTests
{
    [Theory]
    [InlineData(new int[] { }, 5, 0)]
    [InlineData(new[] { 5 }, 5, 0)]
    [InlineData(new[] { 1, 2, 3, 4 }, 5, 2)]
    [InlineData(new[] { 1, 1, 1, 1 }, 2, 6)]
    [InlineData(new[] { 3, 3, 3 }, 7, 0)]
    [InlineData(new[] { -1, 6, 0, 5, 5 }, 5, 3)]
    [InlineData(new[] { 2, 2, 3, 3 }, 5, 4)]
    public void CountsPairs(int[] numbers, int target, long expected)
    {
        Assert.Equal(expected, CountPairsWithSum.Count(numbers, target));
    }

    [Fact]
    public void HandlesLargeValuesWithoutOverflow()
    {
        Assert.Equal(1, CountPairsWithSum.Count([1_000_000_000, 1_000_000_000], 2_000_000_000));
        Assert.Equal(1, CountPairsWithSum.Count([-1_000_000_000, -1_000_000_000], -2_000_000_000));
        // 1e9 + 1e9 wraps around to -294967296 when computed with int arithmetic.
        Assert.Equal(0, CountPairsWithSum.Count([1_000_000_000, 1_000_000_000], -294_967_296));
    }

    [Fact]
    public void MatchesBruteForceOnRandomInputs()
    {
        var random = new Random(42);
        for (int round = 0; round < 50; round++)
        {
            int[] numbers = Enumerable.Range(0, random.Next(0, 60)).Select(_ => random.Next(-10, 11)).ToArray();
            int target = random.Next(-15, 16);

            long expected = 0;
            for (int i = 0; i < numbers.Length; i++)
                for (int j = i + 1; j < numbers.Length; j++)
                    if (numbers[i] + numbers[j] == target) expected++;

            Assert.Equal(expected, CountPairsWithSum.Count(numbers, target));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => CountPairsWithSum.Count(null!, 1));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 200_000;
        int[] ones = Enumerable.Repeat(1, n).ToArray();
        int[] distinct = Enumerable.Range(0, n).ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            Assert.Equal((long)n * (n - 1) / 2, CountPairsWithSum.Count(ones, 2));
            Assert.Equal(n / 2, CountPairsWithSum.Count(distinct, n - 1));
        }, "Use a dictionary of counts instead of a nested loop.");
    }
}
