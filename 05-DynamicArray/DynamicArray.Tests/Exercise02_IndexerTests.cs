using static DsaPractice.DynamicArray.Tests.ListTestHelpers;

namespace DsaPractice.DynamicArray.Tests;

public class Exercise02_IndexerTests
{
    [Fact]
    public void GetReturnsItemsInInsertionOrder()
    {
        var list = ListOf(10, 20, 30);

        Assert.Equal(10, list[0]);
        Assert.Equal(20, list[1]);
        Assert.Equal(30, list[2]);
    }

    [Fact]
    public void SetReplacesItems()
    {
        var list = ListOf("a", "b", "c");

        list[1] = "B";
        list[2] = "C";

        Assert.Equal(["a", "B", "C"], list.Contents());
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void KeepsItemsAfterGrowing()
    {
        var list = new MyList<int>();
        for (int i = 0; i < 1000; i++) list.Add(i * 3);

        for (int i = 0; i < 1000; i++) Assert.Equal(i * 3, list[i]);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]  // within Capacity (4) but not within Count (3)
    [InlineData(4)]
    [InlineData(int.MaxValue)]
    public void ThrowsForIndexesOutsideCount(int index)
    {
        var list = ListOf(1, 2, 3);

        Assert.Throws<ArgumentOutOfRangeException>(() => list[index]);
        Assert.Throws<ArgumentOutOfRangeException>(() => list[index] = 0);
    }

    [Fact]
    public void EmptyListThrowsForIndexZero()
    {
        var list = new MyList<int>(10);

        Assert.Throws<ArgumentOutOfRangeException>(() => list[0]);
    }

    [Fact]
    public void StoresNullValues()
    {
        var list = ListOf<string?>("x", null);

        Assert.Null(list[1]);
        list[0] = null;
        Assert.Null(list[0]);
    }
}
