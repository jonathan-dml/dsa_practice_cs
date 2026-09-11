namespace DsaPractice.Tries.Tests;

public class Exercise01_TrieTests
{
    [Fact]
    public void BasicOperations()
    {
        var trie = new Trie();

        trie.Insert("apple");

        Assert.True(trie.Search("apple"));
        Assert.False(trie.Search("app"));
        Assert.True(trie.StartsWith("app"));

        trie.Insert("app");
        Assert.True(trie.Search("app"));
    }

    [Fact]
    public void EmptyTrieContainsNothing()
    {
        var trie = new Trie();

        Assert.False(trie.Search("a"));
        Assert.False(trie.Search(""));
        Assert.False(trie.StartsWith(""));
        Assert.False(trie.StartsWith("a"));
    }

    [Fact]
    public void EmptyStringIsAValidWord()
    {
        var trie = new Trie();

        trie.Insert("");

        Assert.True(trie.Search(""));
        Assert.True(trie.StartsWith(""));
        Assert.False(trie.Search("a"));
    }

    [Fact]
    public void PrefixesAreNotWords()
    {
        var trie = new Trie();
        trie.Insert("hello");

        Assert.False(trie.Search("hell"));
        Assert.False(trie.Search("helloo"));
        Assert.True(trie.StartsWith("hell"));
        Assert.True(trie.StartsWith("hello"));
        Assert.False(trie.StartsWith("helloo"));
        Assert.True(trie.StartsWith(""));
    }

    [Fact]
    public void IsCaseSensitiveAndSupportsAnyCharacter()
    {
        var trie = new Trie();
        trie.Insert("Olá mundo!");

        Assert.True(trie.Search("Olá mundo!"));
        Assert.False(trie.Search("olá mundo!"));
        Assert.True(trie.StartsWith("Olá "));
    }

    [Fact]
    public void ThrowsForNull()
    {
        var trie = new Trie();

        Assert.Throws<ArgumentNullException>(() => trie.Insert(null!));
        Assert.Throws<ArgumentNullException>(() => trie.Search(null!));
        Assert.Throws<ArgumentNullException>(() => trie.StartsWith(null!));
    }

    [Fact]
    public void MatchesBruteForceOnRandomWords()
    {
        string[] words = WordGenerator.RandomWords(seed: 121, count: 60, minLength: 0, maxLength: 5, alphabet: "abc");
        var trie = new Trie();
        foreach (var word in words) trie.Insert(word);

        foreach (var candidate in WordGenerator.AllStrings("abcd", 4))
        {
            Assert.Equal(words.Contains(candidate), trie.Search(candidate));
            Assert.Equal(words.Any(w => w.StartsWith(candidate, StringComparison.Ordinal)), trie.StartsWith(candidate));
        }
    }

    [Fact]
    public void OperationsDependOnWordLengthOnly()
    {
        string[] words = WordGenerator.RandomWords(seed: 122, count: 100_000, minLength: 5, maxLength: 12);
        string[] prefixes = words.Select(w => w[..3]).ToArray();
        var trie = new Trie();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            foreach (var word in words) trie.Insert(word);
            foreach (var word in words) if (!trie.Search(word)) Assert.Fail($"Missing {word}.");
            foreach (var prefix in prefixes) if (!trie.StartsWith(prefix)) Assert.Fail($"Missing prefix {prefix}.");
            if (trie.StartsWith("#")) Assert.Fail("Unexpected prefix.");
        }, "Walk one node per character instead of scanning the stored words.");
    }
}
