namespace DsaPractice.TwoPointersSlidingWindow.Tests;

public class Exercise04_MaxSumSubarrayTests
{
    [Theory]
    [InlineData(new[] { 2, 1, 5, 1, 3, 2 }, 3, 9)]
    [InlineData(new[] { 2, 3, 4, 1, 5 }, 2, 7)]
    [InlineData(new[] { -1, -2, -3 }, 2, -3)]
    [InlineData(new[] { 5 }, 1, 5)]
    [InlineData(new[] { 1, 2, 3 }, 3, 6)]
    [InlineData(new[] { 4, -10, 4, 4 }, 2, 8)]
    [InlineData(new[] { int.MaxValue, int.MaxValue }, 2, 4_294_967_294L)]
    public void FindsMaximumWindowSum(int[] numbers, int k, long expected)
    {
        Assert.Equal(expected, MaxSumSubarray.MaxSumOfSizeK(numbers, k));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 }, 0)]
    [InlineData(new[] { 1, 2, 3 }, 4)]
    [InlineData(new int[] { }, 1)]
    public void ThrowsForInvalidK(int[] numbers, int k)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MaxSumSubarray.MaxSumOfSizeK(numbers, k));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => MaxSumSubarray.MaxSumOfSizeK(null!, 1));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 1_000_000;
        const int k = 500_000;
        var random = new Random(54);
        int[] numbers = Enumerable.Range(0, n).Select(_ => random.Next(-1000, 1001)).ToArray();
        long[] prefix = new long[n + 1];
        for (int i = 0; i < n; i++) prefix[i + 1] = prefix[i] + numbers[i];
        long expected = Enumerable.Range(0, n - k + 1).Max(i => prefix[i + k] - prefix[i]);

        long actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => MaxSumSubarray.MaxSumOfSizeK(numbers, k),
            "Add the entering element and subtract the leaving one instead of re-summing each window.");

        Assert.Equal(expected, actual);
    }
}
