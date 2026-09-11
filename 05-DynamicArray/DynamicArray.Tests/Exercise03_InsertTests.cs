using static DsaPractice.DynamicArray.Tests.ListTestHelpers;

namespace DsaPractice.DynamicArray.Tests;

public class Exercise03_InsertTests
{
    [Theory]
    [InlineData(0, new[] { 9, 1, 2, 3 })]
    [InlineData(1, new[] { 1, 9, 2, 3 })]
    [InlineData(2, new[] { 1, 2, 9, 3 })]
    [InlineData(3, new[] { 1, 2, 3, 9 })]
    public void InsertsAtPosition(int index, int[] expected)
    {
        var list = ListOf(1, 2, 3);

        list.Insert(index, 9);

        Assert.Equal(expected, list.Contents());
    }

    [Fact]
    public void InsertsIntoEmptyList()
    {
        var list = new MyList<int>();

        list.Insert(0, 42);

        Assert.Equal([42], list.Contents());
    }

    [Fact]
    public void GrowsWhenFull()
    {
        var list = ListOf(1, 2, 3, 4);

        list.Insert(2, 99);

        Assert.Equal([1, 2, 99, 3, 4], list.Contents());
        Assert.Equal(8, list.Capacity);
    }

    [Fact]
    public void RepeatedInsertAtFrontReversesOrder()
    {
        var list = new MyList<int>();

        for (int i = 0; i < 20; i++) list.Insert(0, i);

        Assert.Equal(Enumerable.Range(0, 20).Reverse(), list.Contents());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(4)]
    public void ThrowsForInvalidIndex(int index)
    {
        var list = ListOf(1, 2, 3);

        Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(index, 0));
    }
}
