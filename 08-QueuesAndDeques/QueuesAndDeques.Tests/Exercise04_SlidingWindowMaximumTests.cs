namespace DsaPractice.QueuesAndDeques.Tests;

public class Exercise04_SlidingWindowMaximumTests
{
    [Theory]
    [InlineData(new[] { 1, 3, -1, -3, 5, 3, 6, 7 }, 3, new[] { 3, 3, 5, 5, 6, 7 })]
    [InlineData(new[] { 1 }, 1, new[] { 1 })]
    [InlineData(new[] { 9, 8, 7 }, 1, new[] { 9, 8, 7 })]
    [InlineData(new[] { 1, 2, 3 }, 3, new[] { 3 })]
    [InlineData(new[] { 4, 4, 4 }, 2, new[] { 4, 4 })]
    [InlineData(new[] { 1, -1 }, 1, new[] { 1, -1 })]
    [InlineData(new[] { 7, 2, 4 }, 2, new[] { 7, 4 })]
    [InlineData(new[] { 1, 3, 1, 2, 0, 5 }, 3, new[] { 3, 3, 2, 5 })]
    public void ComputesWindowMaximums(int[] numbers, int k, int[] expected)
    {
        Assert.Equal(expected, SlidingWindowMaximum.MaxInWindows(numbers, k));
    }

    [Fact]
    public void MatchesBruteForceOnRandomInputs()
    {
        var random = new Random(34);
        for (int round = 0; round < 50; round++)
        {
            int[] numbers = Enumerable.Range(0, random.Next(1, 100)).Select(_ => random.Next(-20, 20)).ToArray();
            int k = random.Next(1, numbers.Length + 1);
            int[] expected = Enumerable.Range(0, numbers.Length - k + 1).Select(i => numbers.Skip(i).Take(k).Max()).ToArray();

            Assert.Equal(expected, SlidingWindowMaximum.MaxInWindows(numbers, k));
        }
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 }, 0)]
    [InlineData(new[] { 1, 2, 3 }, 4)]
    [InlineData(new int[] { }, 1)]
    public void ThrowsForInvalidWindowSize(int[] numbers, int k)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SlidingWindowMaximum.MaxInWindows(numbers, k));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => SlidingWindowMaximum.MaxInWindows(null!, 1));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 500_000;
        const int k = 100_000;
        int[] numbers = Enumerable.Range(0, n).Select(i => n - i).ToArray();
        int[] expected = Enumerable.Range(0, n - k + 1).Select(i => n - i).ToArray();

        int[] actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => SlidingWindowMaximum.MaxInWindows(numbers, k),
            "Keep a deque of indices with decreasing values.");

        Assert.Equal(expected, actual);
    }
}
