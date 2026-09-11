namespace DsaPractice.Tries.Tests;

public class Exercise03_AutocompleteTests
{
    private static readonly string[] Words = ["car", "card", "care", "cart", "cat", "dog"];

    [Theory]
    [InlineData("car", 3, new[] { "car", "card", "care" })]
    [InlineData("ca", 10, new[] { "car", "card", "care", "cart", "cat" })]
    [InlineData("cart", 5, new[] { "cart" })]
    [InlineData("d", 5, new[] { "dog" })]
    [InlineData("z", 5, new string[] { })]
    [InlineData("cars", 5, new string[] { })]
    [InlineData("", 2, new[] { "car", "card" })]
    [InlineData("ca", 0, new string[] { })]
    public void SuggestsWordsInOrdinalOrder(string prefix, int limit, string[] expected)
    {
        var autocomplete = new Autocomplete(Words);

        Assert.Equal(expected, autocomplete.Suggest(prefix, limit));
    }

    [Fact]
    public void IgnoresDuplicatesAndUsesOrdinalOrder()
    {
        var autocomplete = new Autocomplete(["beta", "Beta", "alpha", "beta", "Alpha", ""]);

        Assert.Equal(["", "Alpha", "Beta", "alpha", "beta"], autocomplete.Suggest("", 10));
        Assert.Equal(["beta"], autocomplete.Suggest("b", 10));
    }

    [Fact]
    public void ThrowsForInvalidArguments()
    {
        Assert.Throws<ArgumentNullException>(() => new Autocomplete(null!));
        var autocomplete = new Autocomplete(Words);
        Assert.Throws<ArgumentNullException>(() => autocomplete.Suggest(null!, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => autocomplete.Suggest("c", -1));
    }

    [Fact]
    public void MatchesLinqOnRandomWords()
    {
        string[] words = WordGenerator.RandomWords(seed: 124, count: 500, minLength: 1, maxLength: 6, alphabet: "abcAB");
        var autocomplete = new Autocomplete(words);
        var random = new Random(125);

        foreach (var prefix in WordGenerator.AllStrings("abAB", 2))
        {
            int limit = random.Next(0, 30);
            var expected = words.Distinct()
                .Where(w => w.StartsWith(prefix, StringComparison.Ordinal))
                .Order(StringComparer.Ordinal)
                .Take(limit);

            Assert.Equal(expected, autocomplete.Suggest(prefix, limit));
        }
    }

    [Fact]
    public void SuggestionsAreFastWithManyWords()
    {
        string[] words = WordGenerator.RandomWords(seed: 126, count: 100_000, minLength: 4, maxLength: 10);
        string[] prefixes = WordGenerator.RandomWords(seed: 127, count: 10_000, minLength: 2, maxLength: 3);
        var autocomplete = new Autocomplete(words);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            foreach (var prefix in prefixes)
            {
                var suggestions = autocomplete.Suggest(prefix, 10);
                if (suggestions.Any(s => !s.StartsWith(prefix, StringComparison.Ordinal))) Assert.Fail("Wrong suggestion.");
            }
        }, "Walk to the prefix node and stop the DFS once you have enough words.");
    }
}
