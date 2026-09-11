namespace DsaPractice.Arrays.Tests;

public class Exercise07_ProductExceptSelfTests
{
    [Theory]
    [InlineData(new int[] { }, new long[] { })]
    [InlineData(new[] { 5 }, new long[] { 1 })]
    [InlineData(new[] { 2, 3 }, new long[] { 3, 2 })]
    [InlineData(new[] { 1, 2, 3, 4 }, new long[] { 24, 12, 8, 6 })]
    [InlineData(new[] { -1, 1, 0, -3, 3 }, new long[] { 0, 0, 9, 0, 0 })]
    [InlineData(new[] { 0, 0 }, new long[] { 0, 0 })]
    [InlineData(new[] { 0, 4, 5 }, new long[] { 20, 0, 0 })]
    public void ComputesProducts(int[] numbers, long[] expected)
    {
        Assert.Equal(expected, ProductExceptSelf.Compute(numbers));
    }

    [Fact]
    public void UsesLongForLargeProducts()
    {
        int[] numbers = [100_000, 100_000, 100_000];

        Assert.Equal([10_000_000_000, 10_000_000_000, 10_000_000_000], ProductExceptSelf.Compute(numbers));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => ProductExceptSelf.Compute(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 200_000;
        var random = new Random(4);
        int[] numbers = Enumerable.Range(0, n).Select(_ => random.Next(2) == 0 ? -1 : 1).ToArray();
        long total = numbers.Aggregate(1L, (acc, x) => acc * x);

        long[] result = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => ProductExceptSelf.Compute(numbers),
            "Combine a left-to-right pass with a right-to-left pass.");

        // For values of ±1, dividing by x is the same as multiplying by x.
        Assert.Equal(numbers.Select(x => total * x), result);
    }
}
