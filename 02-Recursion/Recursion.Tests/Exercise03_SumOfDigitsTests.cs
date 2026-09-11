namespace DsaPractice.Recursion.Tests;

public class Exercise03_SumOfDigitsTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(7, 7)]
    [InlineData(10, 1)]
    [InlineData(123, 6)]
    [InlineData(-123, 6)]
    [InlineData(9999, 36)]
    [InlineData(1_000_000_000, 1)]
    [InlineData(long.MaxValue, 88)]
    [InlineData(-long.MaxValue, 88)]
    public void SumsDigits(long n, int expected)
    {
        Assert.Equal(expected, SumOfDigits.Compute(n));
    }
}
