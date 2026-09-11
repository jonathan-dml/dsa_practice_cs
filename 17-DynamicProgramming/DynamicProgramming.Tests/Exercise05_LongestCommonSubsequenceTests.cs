namespace DsaPractice.DynamicProgramming.Tests;

public class Exercise05_LongestCommonSubsequenceTests
{
    [Theory]
    [InlineData("abcde", "ace", 3)]
    [InlineData("abc", "abc", 3)]
    [InlineData("abc", "def", 0)]
    [InlineData("", "abc", 0)]
    [InlineData("", "", 0)]
    [InlineData("AGGTAB", "GXTXAYB", 4)]
    [InlineData("bl", "yby", 1)]
    [InlineData("aaaa", "aa", 2)]
    [InlineData("Abc", "abc", 2)]
    public void FindsLength(string first, string second, int expected)
    {
        Assert.Equal(expected, LongestCommonSubsequence.Length(first, second));
        Assert.Equal(expected, LongestCommonSubsequence.Length(second, first));
    }

    [Fact]
    public void MatchesBruteForceOnRandomInputs()
    {
        var random = new Random(164);
        for (int round = 0; round < 50; round++)
        {
            string a = new(Enumerable.Range(0, random.Next(0, 10)).Select(_ => (char)random.Next('a', 'd')).ToArray());
            string b = new(Enumerable.Range(0, random.Next(0, 10)).Select(_ => (char)random.Next('a', 'd')).ToArray());

            var subsequencesOfA = new HashSet<string>();
            for (int mask = 0; mask < 1 << a.Length; mask++)
                subsequencesOfA.Add(new string(a.Where((_, i) => (mask >> i & 1) == 1).ToArray()));
            int expected = 0;
            for (int mask = 0; mask < 1 << b.Length; mask++)
            {
                string s = new(b.Where((_, i) => (mask >> i & 1) == 1).ToArray());
                if (subsequencesOfA.Contains(s)) expected = Math.Max(expected, s.Length);
            }

            Assert.Equal(expected, LongestCommonSubsequence.Length(a, b));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => LongestCommonSubsequence.Length(null!, "a"));
        Assert.Throws<ArgumentNullException>(() => LongestCommonSubsequence.Length("a", null!));
    }

    [Fact]
    public void RunsInQuadraticTime()
    {
        string a = string.Concat(Enumerable.Repeat("abcde", 600));
        string b = string.Concat(Enumerable.Repeat("aebdc", 600));

        int actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => LongestCommonSubsequence.Length(a, b),
            "Fill a table over prefixes of both strings (two rows are enough).");

        Assert.InRange(actual, 1800, 3000);
    }
}
