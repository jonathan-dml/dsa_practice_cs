namespace DsaPractice.Sorting.Tests;

public class Exercise05_QuickSortTests
{
    [Theory]
    [MemberData(nameof(SortingTestData.Arrays), MemberType = typeof(SortingTestData))]
    public void SortsArrays(int[] input)
    {
        SortingTestData.AssertSortsLike(input, QuickSort.Sort);
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => QuickSort.Sort(null!));
    }

    public static TheoryData<string, int[]> LargeInputs => new()
    {
        { "random", SortingTestData.Random(seed: 74, length: 1_000_000, min: int.MinValue, max: int.MaxValue) },
        { "already sorted", Enumerable.Range(0, 1_000_000).ToArray() },
        { "reverse sorted", Enumerable.Range(0, 1_000_000).Reverse().ToArray() },
        { "few distinct values", SortingTestData.Random(seed: 75, length: 1_000_000, min: 0, max: 10) },
        { "all equal", Enumerable.Repeat(42, 1_000_000).ToArray() },
    };

    [Theory]
    [MemberData(nameof(LargeInputs), DisableDiscoveryEnumeration = true)]
    public void RunsInLinearithmicTime(string description, int[] input)
    {
        int[] expected = input.Order().ToArray();
        int[] actual = input.ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => QuickSort.Sort(actual),
            $"Input: {description}. Use a random pivot and three-way partitioning.");

        Assert.Equal(expected, actual);
    }
}
