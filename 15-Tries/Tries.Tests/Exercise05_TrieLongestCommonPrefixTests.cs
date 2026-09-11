namespace DsaPractice.Tries.Tests;

public class Exercise05_TrieLongestCommonPrefixTests
{
    [Theory]
    [InlineData(new string[] { }, "")]
    [InlineData(new[] { "alone" }, "alone")]
    [InlineData(new[] { "flower", "flow", "flight" }, "fl")]
    [InlineData(new[] { "dog", "racecar", "car" }, "")]
    [InlineData(new[] { "abc", "ab" }, "ab")]
    [InlineData(new[] { "ab", "abc" }, "ab")]
    [InlineData(new[] { "same", "same", "same" }, "same")]
    [InlineData(new[] { "", "abc" }, "")]
    [InlineData(new[] { "interview", "internet", "interval", "internal" }, "inter")]
    [InlineData(new[] { "Case", "case" }, "")]
    public void FindsLongestCommonPrefix(string[] words, string expected)
    {
        Assert.Equal(expected, TrieLongestCommonPrefix.Find(words));
    }

    [Fact]
    public void MatchesDirectComparisonOnRandomWords()
    {
        var random = new Random(129);
        for (int round = 0; round < 100; round++)
        {
            string stem = WordGenerator.RandomWord(random, 0, 4, "ab");
            string[] words = Enumerable.Range(0, random.Next(1, 8))
                .Select(_ => stem + WordGenerator.RandomWord(random, 0, 3, "ab"))
                .ToArray();

            int length = 0;
            while (words.All(w => w.Length > length && w[length] == words[0][length])) length++;

            Assert.Equal(words[0][..length], TrieLongestCommonPrefix.Find(words));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => TrieLongestCommonPrefix.Find(null!));
    }
}
