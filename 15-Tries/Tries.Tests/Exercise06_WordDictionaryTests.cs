namespace DsaPractice.Tries.Tests;

public class Exercise06_WordDictionaryTests
{
    [Theory]
    [InlineData("pad", false)]
    [InlineData("bad", true)]
    [InlineData(".ad", true)]
    [InlineData("b..", true)]
    [InlineData("...", true)]
    [InlineData("..d", true)]
    [InlineData("....", false)]
    [InlineData("..", false)]
    [InlineData(".", false)]
    [InlineData("", false)]
    [InlineData("ba", false)]
    [InlineData("m.a", false)]
    public void SearchesWithWildcards(string pattern, bool expected)
    {
        var dictionary = new WordDictionary();
        dictionary.AddWord("bad");
        dictionary.AddWord("dad");
        dictionary.AddWord("mad");

        Assert.Equal(expected, dictionary.Search(pattern));
    }

    [Fact]
    public void EmptyWordCanBeAdded()
    {
        var dictionary = new WordDictionary();

        Assert.False(dictionary.Search(""));
        dictionary.AddWord("");
        Assert.True(dictionary.Search(""));
    }

    [Fact]
    public void DotMatchesAnyCharacterIncludingDots()
    {
        var dictionary = new WordDictionary();
        dictionary.AddWord("a.c");

        Assert.True(dictionary.Search("a.c"));
        Assert.True(dictionary.Search("..."));
        Assert.False(dictionary.Search("abc"));
    }

    [Fact]
    public void ThrowsForNull()
    {
        var dictionary = new WordDictionary();

        Assert.Throws<ArgumentNullException>(() => dictionary.AddWord(null!));
        Assert.Throws<ArgumentNullException>(() => dictionary.Search(null!));
    }

    [Fact]
    public void MatchesBruteForceOnRandomPatterns()
    {
        string[] words = WordGenerator.RandomWords(seed: 130, count: 40, minLength: 0, maxLength: 4, alphabet: "abc");
        var dictionary = new WordDictionary();
        foreach (var word in words) dictionary.AddWord(word);

        foreach (var pattern in WordGenerator.AllStrings("ab.", 4))
        {
            bool expected = words.Any(w => w.Length == pattern.Length && w.Zip(pattern).All(p => p.Second == '.' || p.First == p.Second));

            Assert.Equal(expected, dictionary.Search(pattern));
        }
    }

    [Fact]
    public void SearchIsFastWithManyWords()
    {
        var random = new Random(131);
        string[] words = WordGenerator.RandomWords(seed: 132, count: 200_000, minLength: 8, maxLength: 8);
        var dictionary = new WordDictionary();
        foreach (var word in words) dictionary.AddWord(word);

        string[] found = Enumerable.Range(0, 5000).Select(_ =>
        {
            char[] chars = words[random.Next(words.Length)].ToCharArray();
            chars[random.Next(8)] = '.';
            chars[random.Next(8)] = '.';
            return new string(chars);
        }).ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            foreach (var pattern in found) if (!dictionary.Search(pattern)) Assert.Fail($"Pattern {pattern} should match.");
            for (int i = 0; i < 5000; i++) if (dictionary.Search("ab.#....")) Assert.Fail("Nothing contains '#'.");
        }, "Only branch on '.'; follow a single child for regular characters.");
    }
}
