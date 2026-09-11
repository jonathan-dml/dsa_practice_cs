namespace DsaPractice.BinarySearch.Tests;

public class Exercise05_SearchRotatedTests
{
    private static int[] Rotate(int[] sorted, int shift) =>
        sorted.Skip(shift).Concat(sorted.Take(shift)).ToArray();

    [Theory]
    [InlineData(new[] { 4, 5, 6, 7, 0, 1, 2 }, 0, 4)]
    [InlineData(new[] { 4, 5, 6, 7, 0, 1, 2 }, 4, 0)]
    [InlineData(new[] { 4, 5, 6, 7, 0, 1, 2 }, 2, 6)]
    [InlineData(new[] { 4, 5, 6, 7, 0, 1, 2 }, 3, -1)]
    [InlineData(new int[] { }, 0, -1)]
    [InlineData(new[] { 1 }, 0, -1)]
    [InlineData(new[] { 1 }, 1, 0)]
    [InlineData(new[] { 3, 1 }, 1, 1)]
    [InlineData(new[] { 5, 1, 3 }, 5, 0)]
    public void SearchesRotatedArrays(int[] rotated, int target, int expected)
    {
        Assert.Equal(expected, SearchRotated.IndexOf(rotated, target));
    }

    [Fact]
    public void WorksForEveryRotation()
    {
        for (int length = 1; length <= 12; length++)
        {
            int[] sorted = Enumerable.Range(0, length).Select(i => i * 2).ToArray();
            for (int shift = 0; shift < length; shift++)
            {
                int[] rotated = Rotate(sorted, shift);
                for (int target = -1; target <= length * 2; target++)
                {
                    Assert.Equal(Array.IndexOf(rotated, target), SearchRotated.IndexOf(rotated, target));
                }
            }
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => SearchRotated.IndexOf(null!, 1));
    }

    [Fact]
    public void RunsInLogarithmicTime()
    {
        const int n = 1_000_000;
        int[] rotated = Rotate(Enumerable.Range(0, n).ToArray(), 333_333);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int target = 0; target < n; target += 3)
            {
                int expected = (target - 333_333 + n) % n;
                if (SearchRotated.IndexOf(rotated, target) != expected) Assert.Fail($"Wrong index for {target}.");
            }
        }, "Decide which half is sorted, then check whether the target lies in it.");
    }
}
