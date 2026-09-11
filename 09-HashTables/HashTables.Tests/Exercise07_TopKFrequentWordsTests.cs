namespace DsaPractice.HashTables.Tests;

public class Exercise07_TopKFrequentWordsTests
{
    [Theory]
    [InlineData(new[] { "i", "love", "leetcode", "i", "love", "coding" }, 2, new[] { "i", "love" })]
    [InlineData(new[] { "the", "day", "is", "sunny", "the", "the", "the", "sunny", "is", "is" }, 4, new[] { "the", "is", "sunny", "day" })]
    [InlineData(new[] { "b", "a", "c" }, 2, new[] { "a", "b" })]
    [InlineData(new[] { "a", "b", "a" }, 10, new[] { "a", "b" })]
    [InlineData(new[] { "a", "b" }, 0, new string[] { })]
    [InlineData(new string[] { }, 3, new string[] { })]
    [InlineData(new[] { "b", "B", "a", "A" }, 4, new[] { "A", "B", "a", "b" })]
    public void ReturnsMostFrequentWords(string[] words, int k, string[] expected)
    {
        Assert.Equal(expected, TopKFrequentWords.TopK(words, k));
    }

    [Fact]
    public void ThrowsForInvalidArguments()
    {
        Assert.Throws<ArgumentNullException>(() => TopKFrequentWords.TopK(null!, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TopKFrequentWords.TopK(["a"], -1));
    }

    [Fact]
    public void MatchesLinqOnRandomInputs()
    {
        var random = new Random(46);
        string[] vocabulary = Enumerable.Range(0, 300).Select(i => "w" + i).ToArray();
        string[] words = Enumerable.Range(0, 50_000).Select(_ => vocabulary[random.Next(vocabulary.Length)]).ToArray();

        var expected = words.GroupBy(w => w)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key, StringComparer.Ordinal)
            .Take(25)
            .Select(g => g.Key);

        Assert.Equal(expected, TopKFrequentWords.TopK(words, 25));
    }
}
