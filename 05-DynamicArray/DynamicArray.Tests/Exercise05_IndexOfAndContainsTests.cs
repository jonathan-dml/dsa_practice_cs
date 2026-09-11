using static DsaPractice.DynamicArray.Tests.ListTestHelpers;

namespace DsaPractice.DynamicArray.Tests;

public class Exercise05_IndexOfAndContainsTests
{
    [Theory]
    [InlineData(5, 0)]
    [InlineData(7, 1)]
    [InlineData(9, 3)]
    [InlineData(4, -1)]
    public void IndexOfReturnsFirstMatchingIndex(int item, int expected)
    {
        var list = ListOf(5, 7, 7, 9);

        Assert.Equal(expected, list.IndexOf(item));
    }

    [Fact]
    public void ContainsReportsPresence()
    {
        var list = ListOf("apple", "banana");

        Assert.True(list.Contains("banana"));
        Assert.False(list.Contains("cherry"));
        Assert.False(list.Contains("Apple"));
    }

    [Fact]
    public void EmptyListContainsNothing()
    {
        var list = new MyList<int>(8);

        Assert.Equal(-1, list.IndexOf(0));
        Assert.False(list.Contains(0));
    }

    [Fact]
    public void FindsNull()
    {
        var list = ListOf<string?>("a", null);

        Assert.Equal(1, list.IndexOf(null));
        Assert.True(list.Contains(null));
        Assert.False(ListOf<string?>("a").Contains(null));
    }

    [Fact]
    public void UsesValueEqualityForRecords()
    {
        var list = ListOf(new Person("Ada", 36), new Person("Alan", 41));

        Assert.Equal(1, list.IndexOf(new Person("Alan", 41)));
        Assert.False(list.Contains(new Person("Alan", 42)));
    }

    [Fact]
    public void IgnoresStaleSlotsBeyondCount()
    {
        var list = ListOf(1, 2, 3);
        list.RemoveAt(2);

        Assert.Equal(-1, list.IndexOf(3));
        Assert.False(list.Contains(3));
    }

    [Fact]
    public void IgnoresDefaultValuesInUnusedCapacity()
    {
        var list = ListOf(1, 2, 3); // capacity 4, slot 3 holds default(int) == 0

        Assert.False(list.Contains(0));
    }

    private record Person(string Name, int Age);
}
