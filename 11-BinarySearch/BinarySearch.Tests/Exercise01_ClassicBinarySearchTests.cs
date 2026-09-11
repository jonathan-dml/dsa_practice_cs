namespace DsaPractice.BinarySearch.Tests;

public class Exercise01_ClassicBinarySearchTests
{
    [Fact]
    public void FindsEveryElement()
    {
        int[] sorted = [1, 3, 5, 7, 9, 11];

        for (int i = 0; i < sorted.Length; i++)
        {
            Assert.Equal(i, ClassicBinarySearch.IndexOf(sorted, sorted[i]));
        }
    }

    [Theory]
    [InlineData(-5)]
    [InlineData(0)]
    [InlineData(4)]
    [InlineData(10)]
    [InlineData(12)]
    public void ReturnsMinusOneForMissingValues(int target)
    {
        Assert.Equal(-1, ClassicBinarySearch.IndexOf([1, 3, 5, 7, 9, 11], target));
    }

    [Fact]
    public void HandlesTinyArrays()
    {
        Assert.Equal(-1, ClassicBinarySearch.IndexOf([], 1));
        Assert.Equal(0, ClassicBinarySearch.IndexOf([4], 4));
        Assert.Equal(-1, ClassicBinarySearch.IndexOf([4], 5));
        Assert.Equal(1, ClassicBinarySearch.IndexOf([4, 5], 5));
    }

    [Fact]
    public void HandlesExtremeValues()
    {
        int[] sorted = [int.MinValue, -1, 0, int.MaxValue];

        Assert.Equal(0, ClassicBinarySearch.IndexOf(sorted, int.MinValue));
        Assert.Equal(3, ClassicBinarySearch.IndexOf(sorted, int.MaxValue));
    }

    [Fact]
    public void MatchesLinearSearchOnRandomInputs()
    {
        var random = new Random(61);
        for (int round = 0; round < 50; round++)
        {
            int[] sorted = Enumerable.Range(0, random.Next(0, 100)).Select(_ => random.Next(-200, 200)).Distinct().Order().ToArray();
            for (int target = -210; target <= 210; target += 7)
            {
                Assert.Equal(Array.IndexOf(sorted, target), ClassicBinarySearch.IndexOf(sorted, target));
            }
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => ClassicBinarySearch.IndexOf(null!, 1));
    }

    [Fact]
    public void RunsInLogarithmicTime()
    {
        const int n = 1_000_000;
        int[] evens = Enumerable.Range(0, n).Select(i => i * 2).ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < n; i++)
            {
                if (ClassicBinarySearch.IndexOf(evens, i * 2) != i) Assert.Fail($"Wrong index for {i * 2}.");
                if (ClassicBinarySearch.IndexOf(evens, i * 2 + 1) != -1) Assert.Fail($"Found missing value {i * 2 + 1}.");
            }
        }, "Halve the search range at every step.");
    }
}
