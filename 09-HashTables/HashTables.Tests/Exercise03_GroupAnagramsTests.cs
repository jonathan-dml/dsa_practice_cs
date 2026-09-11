namespace DsaPractice.HashTables.Tests;

public class Exercise03_GroupAnagramsTests
{
    // Makes the comparison independent of group order and of word order inside each group.
    private static List<string> Normalize(IEnumerable<IEnumerable<string>> groups) =>
        groups.Select(g => string.Join(",", g.Order(StringComparer.Ordinal))).Order(StringComparer.Ordinal).ToList();

    [Fact]
    public void GroupsAnagrams()
    {
        string[] words = ["eat", "tea", "tan", "ate", "nat", "bat"];
        string[][] expected = [["eat", "tea", "ate"], ["tan", "nat"], ["bat"]];

        Assert.Equal(Normalize(expected), Normalize(AnagramGrouper.Group(words)));
    }

    [Fact]
    public void EmptyInputGivesNoGroups()
    {
        Assert.Empty(AnagramGrouper.Group([]));
    }

    [Fact]
    public void EmptyStringFormsItsOwnGroup()
    {
        string[][] expected = [["", ""], ["a"]];

        Assert.Equal(Normalize(expected), Normalize(AnagramGrouper.Group(["", "a", ""])));
    }

    [Fact]
    public void KeepsDuplicatesAndIsCaseSensitive()
    {
        string[][] expected = [["abc", "cba", "abc"], ["Abc"]];

        Assert.Equal(Normalize(expected), Normalize(AnagramGrouper.Group(["abc", "cba", "Abc", "abc"])));
    }

    [Fact]
    public void WordsWithSameLettersButDifferentCountsAreDifferent()
    {
        string[][] expected = [["aab"], ["abb"]];

        Assert.Equal(Normalize(expected), Normalize(AnagramGrouper.Group(["aab", "abb"])));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => AnagramGrouper.Group(null!));
    }

    [Fact]
    public void HandlesManyWordsEfficiently()
    {
        var random = new Random(43);
        string[] words = Enumerable.Range(0, 100_000)
            .Select(_ => new string(Enumerable.Range(0, 8).Select(_ => (char)random.Next('a', 'z' + 1)).ToArray()))
            .ToArray();
        var expected = words.GroupBy(w => new string(w.Order().ToArray())).Select(g => g.AsEnumerable());

        var actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(3), () => AnagramGrouper.Group(words),
            "Use a dictionary keyed by each word's sorted letters.");

        Assert.Equal(Normalize(expected), Normalize(actual));
    }
}
