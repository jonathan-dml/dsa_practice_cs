namespace DsaPractice.TwoPointersSlidingWindow.Tests;

public class Exercise07_SortColorsTests
{
    [Theory]
    [InlineData(new int[] { }, new int[] { })]
    [InlineData(new[] { 1 }, new[] { 1 })]
    [InlineData(new[] { 2, 0 }, new[] { 0, 2 })]
    [InlineData(new[] { 2, 0, 1 }, new[] { 0, 1, 2 })]
    [InlineData(new[] { 2, 0, 2, 1, 1, 0 }, new[] { 0, 0, 1, 1, 2, 2 })]
    [InlineData(new[] { 2, 2, 2, 0, 0, 0 }, new[] { 0, 0, 0, 2, 2, 2 })]
    [InlineData(new[] { 1, 1, 1 }, new[] { 1, 1, 1 })]
    [InlineData(new[] { 1, 2, 0, 2, 1, 0, 0, 2 }, new[] { 0, 0, 0, 1, 1, 2, 2, 2 })]
    public void SortsInPlace(int[] colors, int[] expected)
    {
        DutchFlag.Sort(colors);

        Assert.Equal(expected, colors);
    }

    [Fact]
    public void MatchesArraySortOnRandomInputs()
    {
        var random = new Random(57);
        for (int round = 0; round < 100; round++)
        {
            int[] colors = Enumerable.Range(0, random.Next(0, 50)).Select(_ => random.Next(3)).ToArray();
            int[] expected = colors.Order().ToArray();

            DutchFlag.Sort(colors);

            Assert.Equal(expected, colors);
        }
    }

    [Theory]
    [InlineData(new[] { 0, 3, 1 })]
    [InlineData(new[] { -1 })]
    public void ThrowsForInvalidColors(int[] colors)
    {
        Assert.Throws<ArgumentException>(() => DutchFlag.Sort(colors));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => DutchFlag.Sort(null!));
    }
}
