namespace DsaPractice.TwoPointersSlidingWindow.Tests;

public class Exercise05_LongestUniqueSubstringTests
{
    [Theory]
    [InlineData("", 0)]
    [InlineData(" ", 1)]
    [InlineData("a", 1)]
    [InlineData("abcabcbb", 3)]
    [InlineData("bbbbb", 1)]
    [InlineData("pwwkew", 3)]
    [InlineData("dvdf", 3)]
    [InlineData("abba", 2)]
    [InlineData("tmmzuxt", 5)]
    [InlineData("abcdef", 6)]
    [InlineData("aA", 2)]
    public void FindsLongestUniqueSubstring(string text, int expected)
    {
        Assert.Equal(expected, LongestUniqueSubstring.Length(text));
    }

    [Fact]
    public void MatchesBruteForceOnRandomInputs()
    {
        var random = new Random(55);
        for (int round = 0; round < 50; round++)
        {
            string text = new(Enumerable.Range(0, random.Next(0, 60)).Select(_ => (char)random.Next('a', 'h')).ToArray());
            int expected = 0;
            for (int i = 0; i < text.Length; i++)
            {
                var seen = new HashSet<char>();
                for (int j = i; j < text.Length && seen.Add(text[j]); j++) expected = Math.Max(expected, j - i + 1);
            }

            Assert.Equal(expected, LongestUniqueSubstring.Length(text));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => LongestUniqueSubstring.Length(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int distinct = 60_000;
        string text = new(Enumerable.Range(0, 1_000_000).Select(i => (char)(0x0100 + i % distinct)).ToArray());

        int actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => LongestUniqueSubstring.Length(text),
            "Slide the window: remember the last index of each character.");

        Assert.Equal(distinct, actual);
    }
}
