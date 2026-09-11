namespace DsaPractice.Recursion.Tests;

public class Exercise02_FibonacciTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(10, 55)]
    [InlineData(20, 6765)]
    [InlineData(25, 75025)]
    public void NaiveComputesSmallValues(int n, long expected)
    {
        Assert.Equal(expected, Fibonacci.Naive(n));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(10, 55)]
    [InlineData(50, 12_586_269_025)]
    [InlineData(90, 2_880_067_194_370_816_120)]
    public void MemoizedComputesValues(int n, long expected)
    {
        Assert.Equal(expected, Fibonacci.Memoized(n));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(93)]
    public void BothThrowOutsideValidRange(int n)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Fibonacci.Naive(n));
        Assert.Throws<ArgumentOutOfRangeException>(() => Fibonacci.Memoized(n));
    }

    [Fact]
    public void MemoizedMatchesIterativeComputation()
    {
        long previous = 0, current = 1;
        for (int n = 0; n <= 92; n++)
        {
            Assert.Equal(previous, Fibonacci.Memoized(n));
            (previous, current) = (current, previous + current);
        }
    }

    [Fact]
    public void MemoizedRunsInLinearTime()
    {
        long actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => Fibonacci.Memoized(92),
            "Cache each F(k) the first time you compute it.");

        Assert.Equal(7_540_113_804_746_346_429, actual);
    }
}
