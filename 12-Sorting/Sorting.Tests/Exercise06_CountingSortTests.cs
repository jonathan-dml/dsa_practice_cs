namespace DsaPractice.Sorting.Tests;

public class Exercise06_CountingSortTests
{
    [Theory]
    [InlineData(new int[] { })]
    [InlineData(new[] { 0 })]
    [InlineData(new[] { 3, 1, 2 })]
    [InlineData(new[] { 5, 0, 5, 0 })]
    [InlineData(new[] { 1_000_000, 0, 999_999 })]
    [InlineData(new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 })]
    public void SortsNonNegativeValues(int[] input)
    {
        SortingTestData.AssertSortsLike(input, CountingSort.Sort);
    }

    [Fact]
    public void SortsRandomValues()
    {
        SortingTestData.AssertSortsLike(SortingTestData.Random(seed: 76, length: 10_000, min: 0, max: 100), CountingSort.Sort);
    }

    [Fact]
    public void ThrowsForNegativeValues()
    {
        Assert.Throws<ArgumentException>(() => CountingSort.Sort([3, -1, 2]));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => CountingSort.Sort(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        int[] values = SortingTestData.Random(seed: 77, length: 5_000_000, min: 0, max: 1001);
        int[] expected = values.Order().ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => CountingSort.Sort(values),
            "Count occurrences of each value, then write the values back in order.");

        Assert.Equal(expected, values);
    }
}
