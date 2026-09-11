using static DsaPractice.DynamicArray.Tests.ListTestHelpers;

namespace DsaPractice.DynamicArray.Tests;

public class Exercise04_RemoveTests
{
    [Theory]
    [InlineData(0, new[] { 2, 3, 4 })]
    [InlineData(1, new[] { 1, 3, 4 })]
    [InlineData(3, new[] { 1, 2, 3 })]
    public void RemoveAtShiftsElementsLeft(int index, int[] expected)
    {
        var list = ListOf(1, 2, 3, 4);

        list.RemoveAt(index);

        Assert.Equal(expected, list.Contents());
    }

    [Fact]
    public void RemoveAtKeepsCapacity()
    {
        var list = ListOf(1, 2, 3, 4, 5);

        list.RemoveAt(0);
        list.RemoveAt(0);

        Assert.Equal(3, list.Count);
        Assert.Equal(8, list.Capacity);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]
    public void RemoveAtThrowsForInvalidIndex(int index)
    {
        var list = ListOf(1, 2, 3);

        Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(index));
    }

    [Fact]
    public void RemoveDeletesOnlyTheFirstOccurrence()
    {
        var list = ListOf(1, 2, 3, 2);

        Assert.True(list.Remove(2));

        Assert.Equal([1, 3, 2], list.Contents());
    }

    [Fact]
    public void RemoveReturnsFalseWhenMissing()
    {
        var list = ListOf(1, 2, 3);

        Assert.False(list.Remove(7));

        Assert.Equal([1, 2, 3], list.Contents());
    }

    [Fact]
    public void RemoveHandlesNull()
    {
        var list = ListOf<string?>("a", null, "b");

        Assert.True(list.Remove(null));
        Assert.False(list.Remove(null));

        Assert.Equal(new List<string?> { "a", "b" }, list.Contents());
    }

    [Fact]
    public void RemoveUsesValueEquality()
    {
        var list = ListOf(new Point(1, 2), new Point(3, 4));

        Assert.True(list.Remove(new Point(3, 4)));

        Assert.Equal([new Point(1, 2)], list.Contents());
    }

    [Fact]
    public void CanAddAfterRemovingEverything()
    {
        var list = ListOf(1, 2);
        list.RemoveAt(1);
        list.RemoveAt(0);

        list.Add(5);

        Assert.Equal([5], list.Contents());
    }

    private record Point(int X, int Y);
}
