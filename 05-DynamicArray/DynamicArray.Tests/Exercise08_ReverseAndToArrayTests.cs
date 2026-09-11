using static DsaPractice.DynamicArray.Tests.ListTestHelpers;

namespace DsaPractice.DynamicArray.Tests;

public class Exercise08_ReverseAndToArrayTests
{
    [Fact]
    public void ToArrayReturnsExactlyCountItems()
    {
        var list = ListOf(1, 2, 3); // capacity 4

        int[] array = list.ToArray();

        Assert.Equal([1, 2, 3], array);
    }

    [Fact]
    public void ToArrayReturnsACopy()
    {
        var list = ListOf(1, 2, 3);

        int[] array = list.ToArray();
        array[0] = 100;

        Assert.Equal(1, list[0]);
        Assert.NotSame(list.ToArray(), list.ToArray());
    }

    [Fact]
    public void ToArrayOfEmptyListIsEmpty()
    {
        Assert.Empty(new MyList<int>(5).ToArray());
    }

    [Theory]
    [InlineData(new int[] { }, new int[] { })]
    [InlineData(new[] { 1 }, new[] { 1 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    [InlineData(new[] { 1, 2, 3 }, new[] { 3, 2, 1 })]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6 }, new[] { 6, 5, 4, 3, 2, 1 })]
    public void ReverseReversesInPlace(int[] items, int[] expected)
    {
        var list = ListOf(items);

        list.Reverse();

        Assert.Equal(expected, list.ToArray());
    }

    [Fact]
    public void ReverseOnlyTouchesCountItems()
    {
        var list = ListOf(1, 2, 3, 4, 5);
        list.RemoveAt(4);

        list.Reverse();

        Assert.Equal([4, 3, 2, 1], list.ToArray());
    }
}
