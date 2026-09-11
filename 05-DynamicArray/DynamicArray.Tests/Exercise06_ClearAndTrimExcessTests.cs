using static DsaPractice.DynamicArray.Tests.ListTestHelpers;

namespace DsaPractice.DynamicArray.Tests;

public class Exercise06_ClearAndTrimExcessTests
{
    [Fact]
    public void ClearRemovesAllItemsButKeepsCapacity()
    {
        var list = ListOf(1, 2, 3, 4, 5);

        list.Clear();

        Assert.Equal(0, list.Count);
        Assert.Equal(8, list.Capacity);
        Assert.False(list.Contains(1));
    }

    [Fact]
    public void ListIsUsableAfterClear()
    {
        var list = ListOf("a", "b");
        list.Clear();

        list.Add("c");

        Assert.Equal(["c"], list.Contents());
    }

    [Fact]
    public void ClearOnEmptyListDoesNothing()
    {
        var list = new MyList<int>();

        list.Clear();

        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void TrimExcessShrinksCapacityToCount()
    {
        var list = ListOf(1, 2, 3, 4, 5);

        list.TrimExcess();

        Assert.Equal(5, list.Capacity);
        Assert.Equal([1, 2, 3, 4, 5], list.Contents());
    }

    [Fact]
    public void AddAfterTrimExcessDoublesTheTrimmedCapacity()
    {
        var list = ListOf(1, 2, 3, 4, 5);
        list.TrimExcess();

        list.Add(6);

        Assert.Equal(10, list.Capacity);
        Assert.Equal([1, 2, 3, 4, 5, 6], list.Contents());
    }

    [Fact]
    public void TrimExcessOnEmptyListGivesZeroCapacity()
    {
        var list = new MyList<int>(16);

        list.TrimExcess();

        Assert.Equal(0, list.Capacity);
        list.Add(1);
        Assert.Equal(4, list.Capacity);
    }
}
