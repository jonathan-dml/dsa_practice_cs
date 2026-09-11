namespace DsaPractice.Sorting.Tests;

public class Exercise01_BubbleSortTests
{
    [Theory]
    [MemberData(nameof(SortingTestData.Arrays), MemberType = typeof(SortingTestData))]
    public void SortsArrays(int[] input)
    {
        SortingTestData.AssertSortsLike(input, BubbleSort.Sort);
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => BubbleSort.Sort(null!));
    }

    [Fact]
    public void StopsEarlyOnSortedInput()
    {
        int[] sorted = Enumerable.Range(0, 1_000_000).ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => BubbleSort.Sort(sorted),
            "Track whether a pass swapped anything and stop when it didn't.");

        Assert.Equal(Enumerable.Range(0, 1_000_000), sorted);
    }
}
