namespace DsaPractice.BinarySearch.Tests;

public class Exercise06_FindPeakTests
{
    private static void AssertIsPeak(int[] numbers, int index)
    {
        Assert.InRange(index, 0, numbers.Length - 1);
        long left = index > 0 ? numbers[index - 1] : long.MinValue;
        long right = index < numbers.Length - 1 ? numbers[index + 1] : long.MinValue;
        Assert.True(numbers[index] > left && numbers[index] > right, $"Index {index} (value {numbers[index]}) is not a peak.");
    }

    [Theory]
    [InlineData(new[] { 7 })]
    [InlineData(new[] { 1, 2 })]
    [InlineData(new[] { 2, 1 })]
    [InlineData(new[] { 1, 2, 3, 1 })]
    [InlineData(new[] { 1, 2, 1, 3, 5, 6, 4 })]
    [InlineData(new[] { 5, 4, 3, 2, 1 })]
    [InlineData(new[] { 1, 2, 3, 4, 5 })]
    [InlineData(new[] { int.MinValue, int.MinValue + 1 })]
    public void ReturnsAPeak(int[] numbers)
    {
        AssertIsPeak(numbers, FindPeak.Index(numbers));
    }

    [Fact]
    public void FindsPeaksInRandomArrays()
    {
        var random = new Random(65);
        for (int round = 0; round < 200; round++)
        {
            var list = new List<int> { random.Next(-50, 50) };
            int length = random.Next(1, 60);
            while (list.Count < length)
            {
                int next = random.Next(-50, 50);
                if (next != list[^1]) list.Add(next);
            }

            int[] numbers = list.ToArray();
            AssertIsPeak(numbers, FindPeak.Index(numbers));
        }
    }

    [Fact]
    public void ThrowsForInvalidInput()
    {
        Assert.Throws<ArgumentNullException>(() => FindPeak.Index(null!));
        Assert.Throws<ArgumentException>(() => FindPeak.Index([]));
    }

    [Fact]
    public void RunsInLogarithmicTime()
    {
        int[] increasing = Enumerable.Range(0, 1_000_000).ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < 5000; i++)
            {
                if (FindPeak.Index(increasing) != increasing.Length - 1) Assert.Fail("The only peak is the last element.");
            }
        }, "Move towards the larger neighbour instead of scanning.");
    }
}
