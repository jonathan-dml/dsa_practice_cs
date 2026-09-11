namespace DsaPractice.Strings.Tests;

public class Exercise05_CompressTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("a", "a")]
    [InlineData("abc", "abc")]
    [InlineData("aabb", "aabb")]
    [InlineData("aaa", "a3")]
    [InlineData("aabbb", "a2b3")]
    [InlineData("aabcccccaaa", "a2b1c5a3")]
    [InlineData("aaaaaaaaaaaa", "a12")]
    [InlineData("AAaa", "AAaa")]
    [InlineData("AAAaaa", "A3a3")]
    public void CompressesRuns(string text, string expected)
    {
        Assert.Equal(expected, StringCompressor.Compress(text));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => StringCompressor.Compress(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        string text = string.Concat(Enumerable.Repeat("aaaab", 200_000));
        string expected = string.Concat(Enumerable.Repeat("a4b1", 200_000));

        string actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => StringCompressor.Compress(text),
            "Use a StringBuilder instead of concatenating strings.");

        Assert.Equal(expected, actual);
    }
}
