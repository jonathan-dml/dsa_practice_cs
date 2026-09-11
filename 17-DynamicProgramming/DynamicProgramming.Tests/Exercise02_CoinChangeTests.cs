namespace DsaPractice.DynamicProgramming.Tests;

public class Exercise02_CoinChangeTests
{
    [Theory]
    [InlineData(new[] { 1, 2, 5 }, 11, 3)]
    [InlineData(new[] { 2 }, 3, -1)]
    [InlineData(new[] { 1 }, 0, 0)]
    [InlineData(new int[] { }, 0, 0)]
    [InlineData(new int[] { }, 7, -1)]
    [InlineData(new[] { 2, 5, 10, 1 }, 27, 4)]
    [InlineData(new[] { 186, 419, 83, 408 }, 6249, 20)]
    [InlineData(new[] { 1, 3, 4 }, 6, 2)]
    public void FindsMinimumCoins(int[] coins, int amount, int expected)
    {
        Assert.Equal(expected, CoinChange.MinCoins(coins, amount));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 5 }, 5, 4)]
    [InlineData(new[] { 1, 2, 5 }, 11, 11)]
    [InlineData(new[] { 2 }, 3, 0)]
    [InlineData(new[] { 10 }, 10, 1)]
    [InlineData(new int[] { }, 0, 1)]
    [InlineData(new[] { 3, 7 }, 0, 1)]
    [InlineData(new[] { 1, 2, 5, 10, 20, 50, 100, 200 }, 200, 73_682)]
    public void CountsCombinations(int[] coins, int amount, long expected)
    {
        Assert.Equal(expected, CoinChange.CountWays(coins, amount));
    }

    [Fact]
    public void MatchesBruteForceOnSmallInputs()
    {
        var random = new Random(161);
        for (int round = 0; round < 50; round++)
        {
            int[] coins = Enumerable.Range(0, random.Next(1, 4)).Select(_ => random.Next(1, 10)).Distinct().ToArray();
            int amount = random.Next(0, 30);

            int best = int.MaxValue;
            long ways = 0;
            void Explore(int index, int remaining, int used)
            {
                if (remaining == 0) { best = Math.Min(best, used); ways++; return; }
                if (index == coins.Length) return;
                for (int count = 0; count * coins[index] <= remaining; count++)
                    Explore(index + 1, remaining - count * coins[index], used + count);
            }
            Explore(0, amount, 0);

            Assert.Equal(best == int.MaxValue ? -1 : best, CoinChange.MinCoins(coins, amount));
            Assert.Equal(ways, CoinChange.CountWays(coins, amount));
        }
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => CoinChange.MinCoins(null!, 1));
        Assert.Throws<ArgumentNullException>(() => CoinChange.CountWays(null!, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => CoinChange.MinCoins([1], -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => CoinChange.CountWays([1], -1));
        Assert.Throws<ArgumentException>(() => CoinChange.MinCoins([1, 0], 5));
        Assert.Throws<ArgumentException>(() => CoinChange.CountWays([-2], 5));
    }

    [Fact]
    public void RunsInPseudoPolynomialTime()
    {
        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            Assert.Equal(4004, CoinChange.MinCoins([1, 5, 10, 25], 100_004)); // 4000 × 25 + 4 × 1
            Assert.True(CoinChange.CountWays([1, 2, 5, 10, 20, 50, 100, 200], 5000) > 0);
        }, "Fill a table indexed by amount instead of exploring every combination recursively.");
    }
}
