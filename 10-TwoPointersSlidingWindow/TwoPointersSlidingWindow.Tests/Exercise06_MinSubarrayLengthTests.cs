namespace DsaPractice.TwoPointersSlidingWindow.Tests;

public class Exercise06_MinSubarrayLengthTests
{
    [Theory]
    [InlineData(new[] { 2, 3, 1, 2, 4, 3 }, 7, 2)]
    [InlineData(new[] { 1, 4, 4 }, 4, 1)]
    [InlineData(new[] { 1, 1, 1, 1, 1, 1, 1, 1 }, 11, 0)]
    [InlineData(new int[] { }, 1, 0)]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 15, 5)]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 11, 3)]
    [InlineData(new[] { 10 }, 7, 1)]
    [InlineData(new[] { int.MaxValue, int.MaxValue }, int.MaxValue, 1)]
    public void FindsShortestSubarray(int[] positives, int target, int expected)
    {
        Assert.Equal(expected, MinSubarrayLength.MinLength(positives, target));
    }

    [Fact]
    public void MatchesBruteForceOnRandomInputs()
    {
        var random = new Random(56);
        for (int round = 0; round < 50; round++)
        {
            int[] positives = Enumerable.Range(0, random.Next(0, 60)).Select(_ => random.Next(1, 10)).ToArray();
            int target = random.Next(1, 60);
            int expected = 0;
            for (int i = 0; i < positives.Length; i++)
            {
                long sum = 0;
                for (int j = i; j < positives.Length; j++)
                {
                    sum += positives[j];
                    if (sum >= target)
                    {
                        if (expected == 0 || j - i + 1 < expected) expected = j - i + 1;
                        break;
                    }
                }
            }

            Assert.Equal(expected, MinSubarrayLength.MinLength(positives, target));
        }
    }

    [Fact]
    public void ThrowsForInvalidArguments()
    {
        Assert.Throws<ArgumentNullException>(() => MinSubarrayLength.MinLength(null!, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => MinSubarrayLength.MinLength([1, 2], 0));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        int[] ones = Enumerable.Repeat(1, 1_000_000).ToArray();

        int actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => MinSubarrayLength.MinLength(ones, 500_000),
            "Grow the window on the right and shrink it on the left while the sum is large enough.");

        Assert.Equal(500_000, actual);
    }
}
