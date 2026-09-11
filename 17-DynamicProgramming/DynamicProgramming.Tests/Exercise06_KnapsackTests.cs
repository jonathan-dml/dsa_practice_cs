namespace DsaPractice.DynamicProgramming.Tests;

public class Exercise06_KnapsackTests
{
    [Theory]
    [InlineData(new[] { 1, 3, 4, 5 }, new[] { 1, 4, 5, 7 }, 7, 9)]
    [InlineData(new[] { 10, 20, 30 }, new[] { 60, 100, 120 }, 50, 220)]
    [InlineData(new[] { 1, 2, 3 }, new[] { 10, 15, 40 }, 6, 65)]
    [InlineData(new[] { 5 }, new[] { 10 }, 4, 0)]
    [InlineData(new[] { 5 }, new[] { 10 }, 5, 10)]
    [InlineData(new int[] { }, new int[] { }, 10, 0)]
    [InlineData(new[] { 1, 2 }, new[] { 3, 4 }, 0, 0)]
    [InlineData(new[] { 0, 0 }, new[] { 3, 4 }, 0, 7)]
    public void FindsMaximumValue(int[] weights, int[] values, int capacity, long expected)
    {
        Assert.Equal(expected, Knapsack.MaxValue(weights, values, capacity));
    }

    [Fact]
    public void EachItemIsUsedAtMostOnce()
    {
        Assert.Equal(10, Knapsack.MaxValue([2], [10], 100));
    }

    [Fact]
    public void MatchesBruteForceOnRandomInputs()
    {
        var random = new Random(165);
        for (int round = 0; round < 50; round++)
        {
            int n = random.Next(0, 13);
            int[] weights = Enumerable.Range(0, n).Select(_ => random.Next(1, 20)).ToArray();
            int[] values = Enumerable.Range(0, n).Select(_ => random.Next(0, 50)).ToArray();
            int capacity = random.Next(0, 60);

            long expected = 0;
            for (int mask = 0; mask < 1 << n; mask++)
            {
                long weight = 0, value = 0;
                for (int i = 0; i < n; i++) if ((mask >> i & 1) == 1) { weight += weights[i]; value += values[i]; }
                if (weight <= capacity) expected = Math.Max(expected, value);
            }

            Assert.Equal(expected, Knapsack.MaxValue(weights, values, capacity));
        }
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => Knapsack.MaxValue(null!, [1], 1));
        Assert.Throws<ArgumentNullException>(() => Knapsack.MaxValue([1], null!, 1));
        Assert.Throws<ArgumentException>(() => Knapsack.MaxValue([1, 2], [1], 1));
        Assert.Throws<ArgumentException>(() => Knapsack.MaxValue([-1], [1], 1));
        Assert.Throws<ArgumentException>(() => Knapsack.MaxValue([1], [-1], 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => Knapsack.MaxValue([1], [1], -1));
    }

    [Fact]
    public void RunsInPseudoPolynomialTime()
    {
        var random = new Random(166);
        int[] weights = Enumerable.Range(0, 1000).Select(_ => random.Next(1, 100)).ToArray();
        int[] values = Enumerable.Range(0, 1000).Select(_ => random.Next(1, 1000)).ToArray();

        long best = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => Knapsack.MaxValue(weights, values, 10_000),
            "Use a one-dimensional table over capacities, iterating capacities from high to low.");

        Assert.True(best > 0 && best <= values.Sum(v => (long)v));
    }
}
