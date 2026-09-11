namespace DsaPractice.Strings.Tests;

public class Exercise06_LongestCommonPrefixTests
{
    [Theory]
    [InlineData(new string[] { }, "")]
    [InlineData(new[] { "alone" }, "alone")]
    [InlineData(new[] { "flower", "flow", "flight" }, "fl")]
    [InlineData(new[] { "dog", "racecar", "car" }, "")]
    [InlineData(new[] { "", "abc" }, "")]
    [InlineData(new[] { "abc", "abc", "abc" }, "abc")]
    [InlineData(new[] { "interview", "internet", "interval", "internal" }, "inter")]
    [InlineData(new[] { "ab", "a" }, "a")]
    [InlineData(new[] { "Abc", "abc" }, "")]
    public void FindsLongestCommonPrefix(string[] words, string expected)
    {
        Assert.Equal(expected, LongestCommonPrefix.Find(words));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => LongestCommonPrefix.Find(null!));
    }
}
