namespace DsaPractice.Arrays.Tests;

public class Exercise01_MinMaxTests
{
    [Theory]
    [InlineData(new[] { 5 }, 5, 5)]
    [InlineData(new[] { 3, -1, 7, 0 }, -1, 7)]
    [InlineData(new[] { 2, 2, 2 }, 2, 2)]
    [InlineData(new[] { -5, -9, -1 }, -9, -1)]
    [InlineData(new[] { int.MaxValue, int.MinValue }, int.MinValue, int.MaxValue)]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 1, 5)]
    [InlineData(new[] { 5, 4, 3, 2, 1 }, 1, 5)]
    public void FindsMinAndMax(int[] numbers, int expectedMin, int expectedMax)
    {
        var (min, max) = MinMax.Find(numbers);

        Assert.Equal(expectedMin, min);
        Assert.Equal(expectedMax, max);
    }

    [Fact]
    public void ThrowsForEmptyArray()
    {
        Assert.Throws<ArgumentException>(() => MinMax.Find([]));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => MinMax.Find(null!));
    }
}
