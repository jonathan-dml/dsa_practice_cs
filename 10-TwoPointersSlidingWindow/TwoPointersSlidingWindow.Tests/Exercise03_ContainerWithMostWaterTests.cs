namespace DsaPractice.TwoPointersSlidingWindow.Tests;

public class Exercise03_ContainerWithMostWaterTests
{
    [Theory]
    [InlineData(new int[] { }, 0)]
    [InlineData(new[] { 5 }, 0)]
    [InlineData(new[] { 1, 1 }, 1)]
    [InlineData(new[] { 1, 8, 6, 2, 5, 4, 8, 3, 7 }, 49)]
    [InlineData(new[] { 4, 3, 2, 1, 4 }, 16)]
    [InlineData(new[] { 1, 2, 1 }, 2)]
    [InlineData(new[] { 0, 0, 0 }, 0)]
    [InlineData(new[] { 2, 3, 10, 5, 7, 8, 9 }, 36)]
    public void FindsMaximumArea(int[] heights, long expected)
    {
        Assert.Equal(expected, ContainerWithMostWater.MaxArea(heights));
    }

    [Fact]
    public void UsesLongForLargeAreas()
    {
        Assert.Equal((long)int.MaxValue * 2, ContainerWithMostWater.MaxArea([int.MaxValue, 1, int.MaxValue]));
    }

    [Fact]
    public void MatchesBruteForceOnRandomInputs()
    {
        var random = new Random(53);
        for (int round = 0; round < 50; round++)
        {
            int[] heights = Enumerable.Range(0, random.Next(0, 60)).Select(_ => random.Next(0, 50)).ToArray();
            long expected = 0;
            for (int i = 0; i < heights.Length; i++)
                for (int j = i + 1; j < heights.Length; j++)
                    expected = Math.Max(expected, (long)(j - i) * Math.Min(heights[i], heights[j]));

            Assert.Equal(expected, ContainerWithMostWater.MaxArea(heights));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => ContainerWithMostWater.MaxArea(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 1_000_000;
        int[] heights = Enumerable.Range(0, n).Select(i => Math.Min(i, n - 1 - i)).ToArray();

        long actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => ContainerWithMostWater.MaxArea(heights),
            "Always move the pointer at the shorter line.");

        Assert.Equal(250_000L * 499_999, actual);
    }
}
