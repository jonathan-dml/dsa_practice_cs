namespace DsaPractice.Arrays.Tests;

public class Exercise05_MoveZeroesTests
{
    [Theory]
    [InlineData(new int[] { }, new int[] { })]
    [InlineData(new[] { 0 }, new[] { 0 })]
    [InlineData(new[] { 1 }, new[] { 1 })]
    [InlineData(new[] { 0, 1, 0, 3, 12 }, new[] { 1, 3, 12, 0, 0 })]
    [InlineData(new[] { 0, 0, 0, 1 }, new[] { 1, 0, 0, 0 })]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
    [InlineData(new[] { 4, 0, -2, 0, 4, -7 }, new[] { 4, -2, 4, -7, 0, 0 })]
    public void MovesZeroesKeepingOrder(int[] numbers, int[] expected)
    {
        ZeroMover.MoveZeroes(numbers);

        Assert.Equal(expected, numbers);
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => ZeroMover.MoveZeroes(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 1_000_000;
        int[] numbers = Enumerable.Range(0, n).Select(i => i % 2 == 0 ? 0 : i).ToArray();
        int[] expected = numbers.Where(x => x != 0).Concat(Enumerable.Repeat(0, n / 2)).ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => ZeroMover.MoveZeroes(numbers),
            "Write each non-zero value to the next free position, then fill the rest with zeroes.");

        Assert.Equal(expected, numbers);
    }
}
