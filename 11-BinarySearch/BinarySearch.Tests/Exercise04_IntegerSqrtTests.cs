namespace DsaPractice.BinarySearch.Tests;

public class Exercise04_IntegerSqrtTests
{
    private static long Expected(long x)
    {
        long r = (long)Math.Sqrt(x);
        while ((Int128)r * r > x) r--;
        while ((Int128)(r + 1) * (r + 1) <= x) r++;
        return r;
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(8, 2)]
    [InlineData(15, 3)]
    [InlineData(16, 4)]
    [InlineData(2_147_395_599, 46_339)]
    [InlineData(2_147_395_600, 46_340)]
    [InlineData(1_000_000_000_000_000_000, 1_000_000_000)]
    [InlineData(long.MaxValue, 3_037_000_499L)]
    public void ComputesFloorOfSquareRoot(long x, long expected)
    {
        Assert.Equal(expected, IntegerSqrt.Floor(x));
    }

    [Fact]
    public void ThrowsForNegativeNumbers()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => IntegerSqrt.Floor(-1));
    }

    [Fact]
    public void MatchesReferenceOnRandomLargeValues()
    {
        var random = new Random(64);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < 20_000; i++)
            {
                long x = random.NextInt64(long.MaxValue);
                long expected = Expected(x);
                long actual = IntegerSqrt.Floor(x);
                if (actual != expected) Assert.Fail($"Floor({x}) should be {expected} but was {actual}.");
            }
        }, "Binary search r in [0, 3037000499] and compare r against x / r to avoid overflow.");
    }
}
