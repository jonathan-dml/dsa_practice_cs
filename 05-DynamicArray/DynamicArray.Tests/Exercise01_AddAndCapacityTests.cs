namespace DsaPractice.DynamicArray.Tests;

public class Exercise01_AddAndCapacityTests
{
    [Fact]
    public void NewListIsEmptyWithZeroCapacity()
    {
        var list = new MyList<int>();

        Assert.Equal(0, list.Count);
        Assert.Equal(0, list.Capacity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void CapacityConstructorAllocatesExactCapacity(int capacity)
    {
        var list = new MyList<string>(capacity);

        Assert.Equal(0, list.Count);
        Assert.Equal(capacity, list.Capacity);
    }

    [Fact]
    public void NegativeCapacityThrows()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MyList<int>(-1));
    }

    [Fact]
    public void AddIncrementsCount()
    {
        var list = new MyList<int>();

        list.Add(10);
        list.Add(20);
        list.Add(30);

        Assert.Equal(3, list.Count);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(4, 4)]
    [InlineData(5, 8)]
    [InlineData(8, 8)]
    [InlineData(9, 16)]
    [InlineData(17, 32)]
    [InlineData(100, 128)]
    public void CapacityStartsAtFourAndDoubles(int itemsAdded, int expectedCapacity)
    {
        var list = new MyList<int>();

        for (int i = 0; i < itemsAdded; i++) list.Add(i);

        Assert.Equal(itemsAdded, list.Count);
        Assert.Equal(expectedCapacity, list.Capacity);
    }

    [Fact]
    public void CustomCapacityDoublesWhenFull()
    {
        var list = new MyList<int>(3);

        for (int i = 0; i < 4; i++) list.Add(i);

        Assert.Equal(6, list.Capacity);
    }

    [Fact]
    public void ZeroCapacityGrowsToDefaultCapacity()
    {
        var list = new MyList<int>(0);

        list.Add(1);

        Assert.Equal(MyList<int>.DefaultCapacity, list.Capacity);
    }

    [Fact]
    public void AcceptsNullForReferenceTypes()
    {
        var list = new MyList<string?>();

        list.Add(null);

        Assert.Equal(1, list.Count);
    }

    [Fact]
    public void AddIsAmortizedConstantTime()
    {
        var list = new MyList<int>();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < 2_000_000; i++) list.Add(i);
        }, "Double the capacity when the array is full instead of growing by one.");

        Assert.Equal(2_000_000, list.Count);
        Assert.Equal(2_097_152, list.Capacity);
    }
}
