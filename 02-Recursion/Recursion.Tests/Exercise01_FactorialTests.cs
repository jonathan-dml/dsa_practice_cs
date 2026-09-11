namespace DsaPractice.Recursion.Tests;

public class Exercise01_FactorialTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(5, 120)]
    [InlineData(10, 3_628_800)]
    [InlineData(20, 2_432_902_008_176_640_000)]
    public void ComputesFactorial(int n, long expected)
    {
        Assert.Equal(expected, Factorial.Compute(n));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(21)]
    [InlineData(int.MinValue)]
    public void ThrowsOutsideValidRange(int n)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Factorial.Compute(n));
    }
}
