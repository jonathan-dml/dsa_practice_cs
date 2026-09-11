namespace DsaPractice.Sorting.Tests;

public class Exercise08_QuickSelectTests
{
    [Theory]
    [InlineData(new[] { 3, 2, 1, 5, 6, 4 }, 2, 5)]
    [InlineData(new[] { 3, 2, 3, 1, 2, 4, 5, 5, 6 }, 4, 4)]
    [InlineData(new[] { 1 }, 1, 1)]
    [InlineData(new[] { 2, 1 }, 2, 1)]
    [InlineData(new[] { 7, 7, 7 }, 2, 7)]
    [InlineData(new[] { -1, -5, int.MaxValue, int.MinValue }, 1, int.MaxValue)]
    [InlineData(new[] { -1, -5, int.MaxValue, int.MinValue }, 4, int.MinValue)]
    public void FindsKthLargest(int[] numbers, int k, int expected)
    {
        Assert.Equal(expected, QuickSelect.KthLargest(numbers, k));
    }

    [Fact]
    public void MatchesSortingOnRandomInputs()
    {
        var random = new Random(80);
        for (int round = 0; round < 100; round++)
        {
            int[] numbers = SortingTestData.Random(seed: round, length: random.Next(1, 100), min: -30, max: 30);
            int[] descending = numbers.OrderDescending().ToArray();
            int k = random.Next(1, numbers.Length + 1);

            Assert.Equal(descending[k - 1], QuickSelect.KthLargest(numbers, k));
        }
    }

    [Fact]
    public void DoesNotModifyTheInput()
    {
        int[] numbers = [5, 1, 4, 2, 3];

        QuickSelect.KthLargest(numbers, 3);

        Assert.Equal([5, 1, 4, 2, 3], numbers);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 }, 0)]
    [InlineData(new[] { 1, 2, 3 }, 4)]
    [InlineData(new int[] { }, 1)]
    public void ThrowsForInvalidK(int[] numbers, int k)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => QuickSelect.KthLargest(numbers, k));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => QuickSelect.KthLargest(null!, 1));
    }

    [Fact]
    public void HandlesLargeInputs()
    {
        int[] numbers = SortingTestData.Random(seed: 81, length: 2_000_000, min: int.MinValue, max: int.MaxValue);
        int expected = numbers.OrderDescending().ElementAt(999_999);

        int actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => QuickSelect.KthLargest(numbers, 1_000_000));

        Assert.Equal(expected, actual);
    }
}
