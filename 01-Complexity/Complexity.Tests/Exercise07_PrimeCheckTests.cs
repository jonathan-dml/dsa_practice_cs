namespace DsaPractice.Complexity.Tests;

public class Exercise07_PrimeCheckTests
{
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(97)]
    [InlineData(7919)]
    [InlineData(1_000_000_007)]
    public void RecognizesPrimes(long n)
    {
        Assert.True(PrimeCheck.IsPrime(n));
    }

    [Theory]
    [InlineData(long.MinValue)]
    [InlineData(-7)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(4)]
    [InlineData(9)]
    [InlineData(49)]
    [InlineData(91)]
    [InlineData(10_403)] // 101 * 103
    [InlineData(3_000_000_021)] // 3 * 1_000_000_007
    [InlineData(999_999_999_989L * 7)]
    public void RejectsNonPrimes(long n)
    {
        Assert.False(PrimeCheck.IsPrime(n));
    }

    [Fact]
    public void MatchesSieveForSmallNumbers()
    {
        const int limit = 10_000;
        var composite = new bool[limit + 1];
        for (int i = 2; i * i <= limit; i++)
            if (!composite[i])
                for (int j = i * i; j <= limit; j += i) composite[j] = true;

        for (int n = 0; n <= limit; n++)
        {
            Assert.Equal(n >= 2 && !composite[n], PrimeCheck.IsPrime(n));
        }
    }

    [Fact]
    public void RunsInSquareRootTime()
    {
        bool actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1),
            () => PrimeCheck.IsPrime(999_999_999_989),
            "Stop testing divisors once d * d > n.");

        Assert.True(actual);
    }
}
