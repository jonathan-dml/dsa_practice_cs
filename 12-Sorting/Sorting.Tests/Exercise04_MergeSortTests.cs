namespace DsaPractice.Sorting.Tests;

public class Exercise04_MergeSortTests
{
    [Theory]
    [MemberData(nameof(SortingTestData.Arrays), MemberType = typeof(SortingTestData))]
    public void SortsIntegers(int[] input)
    {
        SortingTestData.AssertSortsLike(input, a => MergeSort.Sort(a, (x, y) => x.CompareTo(y)));
    }

    [Fact]
    public void UsesTheComparison()
    {
        int[] numbers = [3, 9, 1, 7];

        MergeSort.Sort(numbers, (x, y) => y.CompareTo(x));

        Assert.Equal([9, 7, 3, 1], numbers);
    }

    [Fact]
    public void SortsStrings()
    {
        string[] words = ["pear", "Apple", "fig", "apple", "banana"];

        MergeSort.Sort(words, string.CompareOrdinal);

        Assert.Equal(["Apple", "apple", "banana", "fig", "pear"], words);
    }

    [Fact]
    public void IsStable()
    {
        var random = new Random(72);
        Item[] items = Enumerable.Range(0, 2000).Select(i => new Item(random.Next(0, 20), i)).ToArray();
        Item[] expected = items.OrderBy(i => i.Key).ToArray(); // LINQ's OrderBy is stable

        MergeSort.Sort(items, (a, b) => a.Key.CompareTo(b.Key));

        Assert.Equal(expected, items);
    }

    [Fact]
    public void ThrowsForNullArguments()
    {
        Assert.Throws<ArgumentNullException>(() => MergeSort.Sort<int>(null!, (a, b) => a.CompareTo(b)));
        Assert.Throws<ArgumentNullException>(() => MergeSort.Sort(new[] { 1 }, null!));
    }

    [Fact]
    public void RunsInLinearithmicTime()
    {
        int[] numbers = SortingTestData.Random(seed: 73, length: 1_000_000, min: int.MinValue, max: int.MaxValue);
        int[] expected = numbers.Order().ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(3), () => MergeSort.Sort(numbers, (a, b) => a.CompareTo(b)),
            "Split in halves recursively and reuse one buffer for merging.");

        Assert.Equal(expected, numbers);
    }

    private record Item(int Key, int Sequence);
}
