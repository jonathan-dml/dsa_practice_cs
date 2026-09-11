namespace DsaPractice.Strings.Tests;

public class Exercise02_IsAnagramTests
{
    [Theory]
    [InlineData("", "", true)]
    [InlineData("a", "a", true)]
    [InlineData("anagram", "nagaram", true)]
    [InlineData("listen", "silent", true)]
    [InlineData("dormitory", "dirtyroom", true)]
    [InlineData("rat", "car", false)]
    [InlineData("a", "A", false)]
    [InlineData("ab", "a", false)]
    [InlineData("aab", "abb", false)]
    [InlineData("a b!", "!b a", true)]
    public void DetectsAnagrams(string first, string second, bool expected)
    {
        Assert.Equal(expected, Anagram.IsAnagram(first, second));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => Anagram.IsAnagram(null!, "a"));
        Assert.Throws<ArgumentNullException>(() => Anagram.IsAnagram("a", null!));
    }
}
