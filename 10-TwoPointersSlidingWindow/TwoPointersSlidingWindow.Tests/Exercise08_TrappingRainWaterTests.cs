namespace DsaPractice.TwoPointersSlidingWindow.Tests;

public class Exercise08_TrappingRainWaterTests
{
    private static long Expected(int[] heights)
    {
        int n = heights.Length;
        if (n == 0) return 0;
        var left = new int[n];
        var right = new int[n];
        left[0] = heights[0];
        for (int i = 1; i < n; i++) left[i] = Math.Max(left[i - 1], heights[i]);
        right[n - 1] = heights[n - 1];
        for (int i = n - 2; i >= 0; i--) right[i] = Math.Max(right[i + 1], heights[i]);
        long water = 0;
        for (int i = 0; i < n; i++) water += Math.Min(left[i], right[i]) - heights[i];
        return water;
    }

    [Theory]
    [InlineData(new int[] { }, 0)]
    [InlineData(new[] { 5 }, 0)]
    [InlineData(new[] { 1, 2, 3 }, 0)]
    [InlineData(new[] { 3, 0, 3 }, 3)]
    [InlineData(new[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 }, 6)]
    [InlineData(new[] { 4, 2, 0, 3, 2, 5 }, 9)]
    [InlineData(new[] { 5, 4, 1, 2 }, 1)]
    [InlineData(new[] { 2, 0, 2, 0, 2 }, 4)]
    public void ComputesTrappedWater(int[] heights, long expected)
    {
        Assert.Equal(expected, TrappingRainWater.Trap(heights));
    }

    [Fact]
    public void MatchesPrefixMaximumSolutionOnRandomInputs()
    {
        var random = new Random(58);
        for (int round = 0; round < 50; round++)
        {
            int[] heights = Enumerable.Range(0, random.Next(0, 80)).Select(_ => random.Next(0, 20)).ToArray();

            Assert.Equal(Expected(heights), TrappingRainWater.Trap(heights));
        }
    }

    [Fact]
    public void UsesLongForLargeVolumes()
    {
        int[] heights = [int.MaxValue, 0, 0, int.MaxValue];

        Assert.Equal(2L * int.MaxValue, TrappingRainWater.Trap(heights));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => TrappingRainWater.Trap(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        var random = new Random(59);
        int[] heights = Enumerable.Range(0, 1_000_000).Select(_ => random.Next(0, 100_000)).ToArray();
        long expected = Expected(heights);

        long actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => TrappingRainWater.Trap(heights),
            "Don't scan left and right for every bar; move two pointers inward keeping the running maximums.");

        Assert.Equal(expected, actual);
    }
}
