namespace DsaPractice.HashTables.Tests;

public class Exercise04_LongestConsecutiveSequenceTests
{
    [Theory]
    [InlineData(new int[] { }, 0)]
    [InlineData(new[] { 5 }, 1)]
    [InlineData(new[] { 100, 4, 200, 1, 3, 2 }, 4)]
    [InlineData(new[] { 0, 3, 7, 2, 5, 8, 4, 6, 0, 1 }, 9)]
    [InlineData(new[] { 1, 2, 0, 1 }, 3)]
    [InlineData(new[] { 9, 1, -3, 2, 4, 8, 3, -1, 6, -2, -4, 7 }, 4)]
    [InlineData(new[] { 10, 30, 20 }, 1)]
    [InlineData(new[] { int.MaxValue, int.MinValue }, 1)]
    [InlineData(new[] { int.MaxValue, int.MaxValue - 1 }, 2)]
    public void FindsLongestRun(int[] numbers, int expected)
    {
        Assert.Equal(expected, LongestConsecutiveSequence.Length(numbers));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => LongestConsecutiveSequence.Length(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 300_000;
        int[] numbers = Enumerable.Range(-n / 2, n).ToArray();
        new Random(44).Shuffle(numbers);

        int actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => LongestConsecutiveSequence.Length(numbers),
            "Only start counting a run at values whose predecessor is not in the set.");

        Assert.Equal(n, actual);
    }
}
