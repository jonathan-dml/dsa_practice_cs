namespace DsaPractice.BinarySearch.Tests;

public class Exercise07_MinInRotatedTests
{
    [Theory]
    [InlineData(new[] { 3, 4, 5, 1, 2 }, 1)]
    [InlineData(new[] { 4, 5, 6, 7, 0, 1, 2 }, 0)]
    [InlineData(new[] { 11, 13, 15, 17 }, 11)]
    [InlineData(new[] { 2, 1 }, 1)]
    [InlineData(new[] { 1, 2 }, 1)]
    [InlineData(new[] { 1 }, 1)]
    [InlineData(new[] { 5, int.MinValue, 0 }, int.MinValue)]
    public void FindsMinimum(int[] rotated, int expected)
    {
        Assert.Equal(expected, MinInRotated.Find(rotated));
    }

    [Fact]
    public void WorksForEveryRotation()
    {
        for (int length = 1; length <= 15; length++)
        {
            int[] sorted = Enumerable.Range(-7, length).ToArray();
            for (int shift = 0; shift < length; shift++)
            {
                int[] rotated = sorted.Skip(shift).Concat(sorted.Take(shift)).ToArray();

                Assert.Equal(-7, MinInRotated.Find(rotated));
            }
        }
    }

    [Fact]
    public void ThrowsForInvalidInput()
    {
        Assert.Throws<ArgumentNullException>(() => MinInRotated.Find(null!));
        Assert.Throws<ArgumentException>(() => MinInRotated.Find([]));
    }

    [Fact]
    public void RunsInLogarithmicTime()
    {
        const int n = 1_000_000;
        int[] rotated = Enumerable.Range(0, n).Select(i => (i + 700_001) % n).ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < 5000; i++)
            {
                if (MinInRotated.Find(rotated) != 0) Assert.Fail("Wrong minimum.");
            }
        }, "Compare the middle element with the last element to decide which half holds the minimum.");
    }
}
