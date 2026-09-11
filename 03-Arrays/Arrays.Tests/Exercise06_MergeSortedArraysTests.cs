namespace DsaPractice.Arrays.Tests;

public class Exercise06_MergeSortedArraysTests
{
    [Theory]
    [InlineData(new int[] { }, new int[] { }, new int[] { })]
    [InlineData(new int[] { }, new[] { 1 }, new[] { 1 })]
    [InlineData(new[] { 1 }, new int[] { }, new[] { 1 })]
    [InlineData(new[] { 1, 3, 5 }, new[] { 2, 4, 6, 8 }, new[] { 1, 2, 3, 4, 5, 6, 8 })]
    [InlineData(new[] { 1, 2, 2 }, new[] { 2, 2, 3 }, new[] { 1, 2, 2, 2, 2, 3 })]
    [InlineData(new[] { 10, 20 }, new[] { 1, 2 }, new[] { 1, 2, 10, 20 })]
    [InlineData(new[] { -5, 0, 5 }, new[] { -10, -5, 100 }, new[] { -10, -5, -5, 0, 5, 100 })]
    public void MergesSortedArrays(int[] first, int[] second, int[] expected)
    {
        Assert.Equal(expected, SortedMerger.Merge(first, second));
    }

    [Fact]
    public void DoesNotModifyTheInputs()
    {
        int[] first = [1, 4];
        int[] second = [2, 3];

        SortedMerger.Merge(first, second);

        Assert.Equal([1, 4], first);
        Assert.Equal([2, 3], second);
    }

    [Fact]
    public void MatchesSortOnRandomInputs()
    {
        var random = new Random(9);
        for (int round = 0; round < 50; round++)
        {
            int[] first = Enumerable.Range(0, random.Next(0, 40)).Select(_ => random.Next(-50, 50)).Order().ToArray();
            int[] second = Enumerable.Range(0, random.Next(0, 40)).Select(_ => random.Next(-50, 50)).Order().ToArray();

            Assert.Equal(first.Concat(second).Order(), SortedMerger.Merge(first, second));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => SortedMerger.Merge(null!, [1]));
        Assert.Throws<ArgumentNullException>(() => SortedMerger.Merge([1], null!));
    }
}
