namespace DsaPractice.Strings.Tests;

public class Exercise03_IsPalindromeTests
{
    [Theory]
    [InlineData("", true)]
    [InlineData(" ", true)]
    [InlineData(".,!", true)]
    [InlineData("a", true)]
    [InlineData("racecar", true)]
    [InlineData("Racecar", true)]
    [InlineData("A man, a plan, a canal: Panama", true)]
    [InlineData("No 'x' in Nixon", true)]
    [InlineData("12321", true)]
    [InlineData("race a car", false)]
    [InlineData("0P", false)]
    [InlineData("ab", false)]
    public void ChecksPalindromes(string text, bool expected)
    {
        Assert.Equal(expected, PalindromeCheck.IsPalindrome(text));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => PalindromeCheck.IsPalindrome(null!));
    }
}
