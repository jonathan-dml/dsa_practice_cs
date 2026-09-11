using System.Numerics;

namespace DsaPractice.Complexity.Tests;

public class Exercise05_FastModPowerTests
{
    [Theory]
    [InlineData(2, 10, 1000, 24)]
    [InlineData(3, 0, 7, 1)]
    [InlineData(5, 3, 1, 0)]
    [InlineData(0, 0, 13, 1)]
    [InlineData(0, 5, 13, 0)]
    [InlineData(7, 1, 5, 2)]
    [InlineData(10, 9, 6, 4)]
    public void ComputesPowers(long baseValue, long exponent, long modulus, long expected)
    {
        Assert.Equal(expected, FastModPower.Pow(baseValue, exponent, modulus));
    }

    [Fact]
    public void MatchesBigIntegerOnRandomInputs()
    {
        var random = new Random(11);
        for (int i = 0; i < 200; i++)
        {
            long baseValue = random.NextInt64(0, long.MaxValue);
            long exponent = random.NextInt64(0, 1_000_000_000_000);
            long modulus = random.NextInt64(1, int.MaxValue + 1L);

            long expected = (long)BigInteger.ModPow(baseValue, exponent, modulus);

            Assert.Equal(expected, FastModPower.Pow(baseValue, exponent, modulus));
        }
    }

    [Theory]
    [InlineData(-1, 2, 5)]
    [InlineData(2, -1, 5)]
    [InlineData(2, 3, 0)]
    [InlineData(2, 3, -5)]
    [InlineData(2, 3, int.MaxValue + 1L)]
    public void ThrowsForInvalidArguments(long baseValue, long exponent, long modulus)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => FastModPower.Pow(baseValue, exponent, modulus));
    }

    [Fact]
    public void RunsInLogarithmicTime()
    {
        long expected = (long)BigInteger.ModPow(123_456_789, long.MaxValue, 1_000_000_007);

        long actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1),
            () => FastModPower.Pow(123_456_789, long.MaxValue, 1_000_000_007),
            "Use exponentiation by squaring.");

        Assert.Equal(expected, actual);
    }
}
