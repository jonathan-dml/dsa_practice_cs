namespace DsaPractice.DynamicProgramming.Tests;

public class Exercise03_HouseRobberTests
{
    [Theory]
    [InlineData(new int[] { }, 0)]
    [InlineData(new[] { 5 }, 5)]
    [InlineData(new[] { 1, 2 }, 2)]
    [InlineData(new[] { 1, 2, 3, 1 }, 4)]
    [InlineData(new[] { 2, 7, 9, 3, 1 }, 12)]
    [InlineData(new[] { 2, 1, 1, 2 }, 4)]
    [InlineData(new[] { 2, 3, 2 }, 4)]
    [InlineData(new[] { 0, 0, 0 }, 0)]
    public void RobsHousesInALine(int[] houses, long expected)
    {
        Assert.Equal(expected, HouseRobber.MaxLoot(houses));
    }

    [Theory]
    [InlineData(new int[] { }, 0)]
    [InlineData(new[] { 5 }, 5)]
    [InlineData(new[] { 1, 2 }, 2)]
    [InlineData(new[] { 2, 3, 2 }, 3)]
    [InlineData(new[] { 1, 2, 3, 1 }, 4)]
    [InlineData(new[] { 1, 2, 3 }, 3)]
    [InlineData(new[] { 2, 7, 9, 3, 1 }, 11)]
    [InlineData(new[] { 200, 3, 140, 20, 10 }, 340)]
    public void RobsHousesInACircle(int[] houses, long expected)
    {
        Assert.Equal(expected, HouseRobber.MaxLootCircular(houses));
    }

    [Fact]
    public void MatchesBruteForceOnRandomInputs()
    {
        var random = new Random(162);
        for (int round = 0; round < 50; round++)
        {
            int[] houses = Enumerable.Range(0, random.Next(0, 16)).Select(_ => random.Next(0, 100)).ToArray();
            int n = houses.Length;
            long line = 0, circle = 0;
            for (int mask = 0; mask < 1 << n; mask++)
            {
                if ((mask & (mask >> 1)) != 0) continue;
                long total = Enumerable.Range(0, n).Where(i => (mask >> i & 1) == 1).Sum(i => (long)houses[i]);
                line = Math.Max(line, total);
                bool wraps = n > 1 && (mask & 1) == 1 && (mask >> (n - 1) & 1) == 1;
                if (!wraps) circle = Math.Max(circle, total);
            }

            Assert.Equal(line, HouseRobber.MaxLoot(houses));
            Assert.Equal(circle, HouseRobber.MaxLootCircular(houses));
        }
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => HouseRobber.MaxLoot(null!));
        Assert.Throws<ArgumentNullException>(() => HouseRobber.MaxLootCircular(null!));
        Assert.Throws<ArgumentException>(() => HouseRobber.MaxLoot([1, -1]));
        Assert.Throws<ArgumentException>(() => HouseRobber.MaxLootCircular([1, -1]));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        int[] houses = Enumerable.Repeat(int.MaxValue, 1_000_000).ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            Assert.Equal(500_000L * int.MaxValue, HouseRobber.MaxLoot(houses));
            Assert.Equal(500_000L * int.MaxValue, HouseRobber.MaxLootCircular(houses));
        }, "Keep only the best totals for the previous two houses.");
    }
}
