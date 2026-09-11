namespace DsaPractice.Recursion.Tests;

public class Exercise06_PermutationsTests
{
    private static List<string> Sorted(IEnumerable<string> values) => values.Order(StringComparer.Ordinal).ToList();

    [Theory]
    [InlineData("", new[] { "" })]
    [InlineData("a", new[] { "a" })]
    [InlineData("ab", new[] { "ab", "ba" })]
    [InlineData("abc", new[] { "abc", "acb", "bac", "bca", "cab", "cba" })]
    [InlineData("aab", new[] { "aab", "aba", "baa" })]
    [InlineData("aaaa", new[] { "aaaa" })]
    public void GeneratesUniquePermutations(string input, string[] expected)
    {
        Assert.Equal(Sorted(expected), Sorted(Permutations.Generate(input)));
    }

    [Theory]
    [InlineData("abcdef", 720)]
    [InlineData("aabbcc", 90)]
    [InlineData("mississippi", 34_650)]
    public void GeneratesTheRightNumberOfDistinctPermutations(string input, int expectedCount)
    {
        var result = Permutations.Generate(input);

        Assert.Equal(expectedCount, result.Count);
        Assert.Equal(expectedCount, result.Distinct().Count());
        string letters = string.Concat(input.Order());
        Assert.All(result, p => Assert.Equal(letters, string.Concat(p.Order())));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => Permutations.Generate(null!));
    }
}
