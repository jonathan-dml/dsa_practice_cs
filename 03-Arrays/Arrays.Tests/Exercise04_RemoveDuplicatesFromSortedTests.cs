namespace DsaPractice.Arrays.Tests;

public class Exercise04_RemoveDuplicatesFromSortedTests
{
    [Theory]
    [InlineData(new int[] { }, new int[] { })]
    [InlineData(new[] { 1 }, new[] { 1 })]
    [InlineData(new[] { 1, 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 0, 0, 1, 1, 1, 2, 2, 3, 3, 4 }, new[] { 0, 1, 2, 3, 4 })]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
    [InlineData(new[] { 7, 7, 7, 7 }, new[] { 7 })]
    [InlineData(new[] { -3, -3, -1, 0, 0 }, new[] { -3, -1, 0 })]
    public void CompactsDistinctValues(int[] sorted, int[] expectedPrefix)
    {
        int k = SortedDuplicates.Remove(sorted);

        Assert.Equal(expectedPrefix.Length, k);
        Assert.Equal(expectedPrefix, sorted[..k]);
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => SortedDuplicates.Remove(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int distinct = 500_000;
        int[] sorted = Enumerable.Range(0, distinct).SelectMany(x => new[] { x, x }).ToArray();

        int k = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => SortedDuplicates.Remove(sorted),
            "Avoid shifting elements; use a read index and a write index.");

        Assert.Equal(distinct, k);
        Assert.Equal(Enumerable.Range(0, distinct), sorted[..k]);
    }
}
