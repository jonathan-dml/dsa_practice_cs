namespace DsaPractice.HashTables.Tests;

public class Exercise06_IsomorphicStringsTests
{
    [Theory]
    [InlineData("", "", true)]
    [InlineData("a", "b", true)]
    [InlineData("egg", "add", true)]
    [InlineData("paper", "title", true)]
    [InlineData("abc", "abc", true)]
    [InlineData("foo", "bar", false)]
    [InlineData("badc", "baba", false)]
    [InlineData("ab", "aa", false)]
    [InlineData("aa", "ab", false)]
    [InlineData("abc", "ab", false)]
    [InlineData("13", "42", true)]
    public void ChecksIsomorphism(string first, string second, bool expected)
    {
        Assert.Equal(expected, IsomorphicStrings.AreIsomorphic(first, second));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => IsomorphicStrings.AreIsomorphic(null!, "a"));
        Assert.Throws<ArgumentNullException>(() => IsomorphicStrings.AreIsomorphic("a", null!));
    }
}
