namespace DsaPractice.HashTables.Tests;

public class Exercise02_TwoSumTests
{
    private static void AssertValidPair(int[] numbers, int target, (int First, int Second)? result)
    {
        Assert.NotNull(result);
        var (first, second) = result.Value;
        Assert.InRange(first, 0, numbers.Length - 1);
        Assert.InRange(second, 0, numbers.Length - 1);
        Assert.True(first < second, $"Expected First < Second but got ({first}, {second}).");
        Assert.Equal((long)target, (long)numbers[first] + numbers[second]);
    }

    [Theory]
    [InlineData(new[] { 2, 7, 11, 15 }, 9)]
    [InlineData(new[] { 3, 2, 4 }, 6)]
    [InlineData(new[] { 3, 3 }, 6)]
    [InlineData(new[] { -3, 4, 3, 90 }, 0)]
    [InlineData(new[] { 0, 4, 3, 0 }, 0)]
    [InlineData(new[] { 1, 5, 9, 2 }, 11)]
    [InlineData(new[] { int.MinValue, int.MaxValue }, -1)]
    public void FindsAValidPair(int[] numbers, int target)
    {
        AssertValidPair(numbers, target, TwoSum.Find(numbers, target));
    }

    [Theory]
    [InlineData(new int[] { }, 0)]
    [InlineData(new[] { 3 }, 6)]
    [InlineData(new[] { 1, 2, 3 }, 7)]
    [InlineData(new[] { int.MaxValue, 1 }, int.MinValue)] // int.MaxValue + 1 overflows to int.MinValue
    public void ReturnsNullWhenNoPairExists(int[] numbers, int target)
    {
        Assert.Null(TwoSum.Find(numbers, target));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => TwoSum.Find(null!, 0));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 1_000_000;
        int[] evens = Enumerable.Range(0, n).Select(i => i * 2).ToArray();
        new Random(42).Shuffle(evens);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            Assert.Null(TwoSum.Find(evens, 7));
            AssertValidPair(evens, 1_999_996, TwoSum.Find(evens, 1_999_996));
        }, "For each value, look up target - value in a dictionary of values seen so far.");
    }
}
