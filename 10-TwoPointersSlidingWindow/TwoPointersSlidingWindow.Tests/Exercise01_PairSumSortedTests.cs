namespace DsaPractice.TwoPointersSlidingWindow.Tests;

public class Exercise01_PairSumSortedTests
{
    private static void AssertValidPair(int[] sorted, int target, (int Left, int Right)? result)
    {
        Assert.NotNull(result);
        var (left, right) = result.Value;
        Assert.True(0 <= left && left < right && right < sorted.Length, $"Invalid indices ({left}, {right}).");
        Assert.Equal((long)target, (long)sorted[left] + sorted[right]);
    }

    [Theory]
    [InlineData(new[] { 2, 5, 9, 11 }, 11)]
    [InlineData(new[] { 1, 2, 3, 4, 6 }, 6)]
    [InlineData(new[] { 1, 1 }, 2)]
    [InlineData(new[] { -8, -3, 0, 4, 10 }, -11)]
    [InlineData(new[] { -8, -3, 0, 4, 10 }, 2)]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 9)]
    public void FindsAValidPair(int[] sorted, int target)
    {
        AssertValidPair(sorted, target, PairSumSorted.FindPair(sorted, target));
    }

    [Theory]
    [InlineData(new int[] { }, 0)]
    [InlineData(new[] { 5 }, 10)]
    [InlineData(new[] { 1, 2 }, 10)]
    [InlineData(new[] { 1, 3, 5 }, 7)]
    [InlineData(new[] { int.MaxValue - 1, int.MaxValue }, -3)] // the int sum would overflow to -3
    public void ReturnsNullWhenNoPairExists(int[] sorted, int target)
    {
        Assert.Null(PairSumSorted.FindPair(sorted, target));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => PairSumSorted.FindPair(null!, 0));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 1_000_000;
        int[] evens = Enumerable.Range(0, n).Select(i => i * 2).ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            Assert.Null(PairSumSorted.FindPair(evens, 999_999));
            AssertValidPair(evens, 1_000_000, PairSumSorted.FindPair(evens, 1_000_000));
        }, "Move the left pointer when the sum is too small and the right pointer when it's too large.");
    }
}
