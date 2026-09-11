namespace DsaPractice.Arrays.Tests;

public class Exercise03_RotateRightTests
{
    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7 }, 3, new[] { 5, 6, 7, 1, 2, 3, 4 })]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7 }, 1, new[] { 7, 1, 2, 3, 4, 5, 6 })]
    [InlineData(new[] { 1, 2, 3 }, 0, new[] { 1, 2, 3 })]
    [InlineData(new[] { 1, 2, 3 }, 3, new[] { 1, 2, 3 })]
    [InlineData(new[] { 1, 2, 3 }, 10, new[] { 3, 1, 2 })]
    [InlineData(new[] { -1, -100, 3, 99 }, 2, new[] { 3, 99, -1, -100 })]
    [InlineData(new[] { 42 }, 5, new[] { 42 })]
    [InlineData(new int[] { }, 5, new int[] { })]
    public void RotatesInPlace(int[] numbers, int k, int[] expected)
    {
        ArrayRotation.RotateRight(numbers, k);

        Assert.Equal(expected, numbers);
    }

    [Fact]
    public void ThrowsForNegativeK()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ArrayRotation.RotateRight([1, 2, 3], -1));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => ArrayRotation.RotateRight(null!, 1));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 1_000_000;
        const int k = 500_001;
        int[] numbers = Enumerable.Range(0, n).ToArray();
        int[] expected = new int[n];
        for (int i = 0; i < n; i++) expected[(i + k) % n] = i;

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => ArrayRotation.RotateRight(numbers, k),
            "Use the triple-reverse trick instead of rotating one step at a time.");

        Assert.Equal(expected, numbers);
    }
}
