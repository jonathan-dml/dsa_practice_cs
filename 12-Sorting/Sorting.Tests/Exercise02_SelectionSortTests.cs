namespace DsaPractice.Sorting.Tests;

public class Exercise02_SelectionSortTests
{
    [Theory]
    [MemberData(nameof(SortingTestData.Arrays), MemberType = typeof(SortingTestData))]
    public void SortsArrays(int[] input)
    {
        SortingTestData.AssertSortsLike(input, SelectionSort.Sort);
    }

    [Fact]
    public void SortsInPlace()
    {
        int[] numbers = [3, 1, 2];
        int[] same = numbers;

        SelectionSort.Sort(numbers);

        Assert.Same(same, numbers);
        Assert.Equal([1, 2, 3], numbers);
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => SelectionSort.Sort(null!));
    }
}
