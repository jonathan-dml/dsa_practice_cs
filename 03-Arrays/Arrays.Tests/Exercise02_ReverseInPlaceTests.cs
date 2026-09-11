namespace DsaPractice.Arrays.Tests;

public class Exercise02_ReverseInPlaceTests
{
    [Theory]
    [InlineData(new int[] { }, new int[] { })]
    [InlineData(new[] { 1 }, new[] { 1 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    [InlineData(new[] { 1, 2, 3 }, new[] { 3, 2, 1 })]
    [InlineData(new[] { 1, 2, 3, 4 }, new[] { 4, 3, 2, 1 })]
    [InlineData(new[] { 5, 5, -1, 5 }, new[] { 5, -1, 5, 5 })]
    public void ReversesTheSameArrayInstance(int[] numbers, int[] expected)
    {
        int[] original = numbers;

        ArrayReverser.Reverse(numbers);

        Assert.Same(original, numbers);
        Assert.Equal(expected, numbers);
    }

    [Fact]
    public void ReversesLargeArrays()
    {
        int[] numbers = Enumerable.Range(0, 1_000_001).ToArray();

        ArrayReverser.Reverse(numbers);

        Assert.Equal(Enumerable.Range(0, 1_000_001).Reverse(), numbers);
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => ArrayReverser.Reverse(null!));
    }
}
