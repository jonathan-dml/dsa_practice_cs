namespace DsaPractice.Stacks.Tests;

public class Exercise02_ValidParenthesesTests
{
    [Theory]
    [InlineData("", true)]
    [InlineData("()", true)]
    [InlineData("()[]{}", true)]
    [InlineData("{[()]}", true)]
    [InlineData("(a + b) * [c - {d / e}]", true)]
    [InlineData("no brackets at all", true)]
    [InlineData("(]", false)]
    [InlineData("([)]", false)]
    [InlineData("(", false)]
    [InlineData(")", false)]
    [InlineData("((", false)]
    [InlineData("}{", false)]
    [InlineData("(()", false)]
    [InlineData("())", false)]
    public void ValidatesBrackets(string text, bool expected)
    {
        Assert.Equal(expected, BracketValidator.IsValid(text));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => BracketValidator.IsValid(null!));
    }

    [Fact]
    public void HandlesDeepNestingInLinearTime()
    {
        string nested = new string('(', 250_000) + new string('[', 250_000) + new string(']', 250_000) + new string(')', 250_000);
        string broken = nested[..^1] + "]";

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            Assert.True(BracketValidator.IsValid(nested));
            Assert.False(BracketValidator.IsValid(broken));
        });
    }
}
