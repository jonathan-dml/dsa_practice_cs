namespace DsaPractice.Stacks.Tests;

public class Exercise07_DecodeStringTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("abc", "abc")]
    [InlineData("3[a]", "aaa")]
    [InlineData("3[a]2[bc]", "aaabcbc")]
    [InlineData("3[a2[c]]", "accaccacc")]
    [InlineData("2[abc]3[cd]ef", "abcabccdcdcdef")]
    [InlineData("10[x]", "xxxxxxxxxx")]
    [InlineData("ab2[c]de", "abccde")]
    [InlineData("1[a1[b1[c]]]", "abc")]
    [InlineData("3[z]2[2[y]pq4[2[jk]e1[f]]]ef", "zzzyypqjkjkefjkjkefjkjkefjkjkefyypqjkjkefjkjkefjkjkefjkjkefef")]
    public void DecodesStrings(string encoded, string expected)
    {
        Assert.Equal(expected, StringDecoder.Decode(encoded));
    }

    [Fact]
    public void HandlesLargeRepeatCounts()
    {
        string decoded = StringDecoder.Decode("100[ab]");

        Assert.Equal(string.Concat(Enumerable.Repeat("ab", 100)), decoded);
    }

    [Fact]
    public void HandlesNestedLargeCounts()
    {
        string decoded = StringDecoder.Decode("12[3[x]y]");

        Assert.Equal(string.Concat(Enumerable.Repeat("xxxy", 12)), decoded);
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => StringDecoder.Decode(null!));
    }
}
