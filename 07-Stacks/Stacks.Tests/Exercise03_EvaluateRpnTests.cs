namespace DsaPractice.Stacks.Tests;

public class Exercise03_EvaluateRpnTests
{
    [Theory]
    [InlineData(new[] { "42" }, 42)]
    [InlineData(new[] { "-7" }, -7)]
    [InlineData(new[] { "2", "1", "+", "3", "*" }, 9)]
    [InlineData(new[] { "4", "13", "5", "/", "+" }, 6)]
    [InlineData(new[] { "10", "6", "9", "3", "+", "-11", "*", "/", "*", "17", "+", "5", "+" }, 22)]
    [InlineData(new[] { "5", "8", "-" }, -3)]
    [InlineData(new[] { "-7", "2", "/" }, -3)]
    [InlineData(new[] { "7", "-2", "/" }, -3)]
    [InlineData(new[] { "3", "4", "*", "5", "6", "*", "-" }, -18)]
    public void EvaluatesExpressions(string[] tokens, int expected)
    {
        Assert.Equal(expected, RpnCalculator.Evaluate(tokens));
    }

    public static TheoryData<string[]> MalformedExpressions => new()
    {
        Array.Empty<string>(),
        new[] { "+" },
        new[] { "1", "+" },
        new[] { "1", "2" },
        new[] { "1", "2", "3", "+" },
    };

    [Theory]
    [MemberData(nameof(MalformedExpressions))]
    public void ThrowsForMalformedExpressions(string[] tokens)
    {
        Assert.Throws<InvalidOperationException>(() => RpnCalculator.Evaluate(tokens));
    }

    [Fact]
    public void ThrowsForInvalidNumbers()
    {
        Assert.Throws<FormatException>(() => RpnCalculator.Evaluate(["1", "x", "+"]));
    }

    [Fact]
    public void ThrowsForDivisionByZero()
    {
        Assert.Throws<DivideByZeroException>(() => RpnCalculator.Evaluate(["1", "0", "/"]));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => RpnCalculator.Evaluate(null!));
    }
}
