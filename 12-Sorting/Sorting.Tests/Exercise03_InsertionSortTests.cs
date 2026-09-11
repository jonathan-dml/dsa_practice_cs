namespace DsaPractice.Sorting.Tests;

public class Exercise03_InsertionSortTests
{
    [Theory]
    [MemberData(nameof(SortingTestData.Arrays), MemberType = typeof(SortingTestData))]
    public void SortsArrays(int[] input)
    {
        SortingTestData.AssertSortsLike(input, InsertionSort.Sort);
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => InsertionSort.Sort(null!));
    }

    [Fact]
    public void IsFastOnNearlySortedInput()
    {
        const int n = 1_000_000;
        int[] numbers = Enumerable.Range(0, n).ToArray();
        var random = new Random(71);
        for (int i = 0; i < 1000; i++)
        {
            int j = random.Next(n - 1);
            (numbers[j], numbers[j + 1]) = (numbers[j + 1], numbers[j]);
        }

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => InsertionSort.Sort(numbers),
            "Stop shifting as soon as the element being inserted is not smaller than its left neighbour.");

        Assert.Equal(Enumerable.Range(0, n), numbers);
    }
}
