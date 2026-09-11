namespace DsaPractice.DynamicProgramming.Tests;

public class Exercise01_ClimbingStairsTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(4, 5)]
    [InlineData(5, 8)]
    [InlineData(10, 89)]
    [InlineData(45, 1_836_311_903)]
    public void CountsWays(int steps, long expected)
    {
        Assert.Equal(expected, ClimbingStairs.Ways(steps));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(91)]
    public void ThrowsOutsideValidRange(int steps)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ClimbingStairs.Ways(steps));
    }

    [Fact]
    public void HandlesLargeInputsInLinearTime()
    {
        var expected = new long[91];
        expected[0] = expected[1] = 1;
        for (int i = 2; i <= 90; i++) expected[i] = expected[i - 1] + expected[i - 2];

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int n = 0; n <= 90; n++) Assert.Equal(expected[n], ClimbingStairs.Ways(n));
        }, "Build the answer from ways(n-1) and ways(n-2) instead of recomputing them recursively.");

        Assert.Equal(4_660_046_610_375_530_309, expected[90]);
    }
}
