namespace DsaPractice.Tries.Tests;

public class Exercise07_ReplaceWordsTests
{
    [Theory]
    [InlineData(new[] { "cat", "bat", "rat" }, "the cattle was rattled by the battery", "the cat was rat by the bat")]
    [InlineData(new[] { "a", "b", "c" }, "aadsfasf absbs bbab cadsfafs", "a a b c")]
    [InlineData(new[] { "a", "aa", "aaa" }, "aaaa aaa aa a", "a a a a")]
    [InlineData(new[] { "catt", "cat", "bat", "rat" }, "the cattle was rattled by the battery", "the cat was rat by the bat")]
    [InlineData(new[] { "ac", "ab" }, "it is abnormal that this solution is accepted", "it is ab that this solution is ac")]
    [InlineData(new string[] { }, "nothing changes here", "nothing changes here")]
    [InlineData(new[] { "x" }, "", "")]
    [InlineData(new[] { "hello" }, "hell hello helloworld", "hell hello hello")]
    public void ReplacesWordsWithShortestRoot(string[] roots, string sentence, string expected)
    {
        Assert.Equal(expected, ReplaceWords.Replace(roots, sentence));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => ReplaceWords.Replace(null!, "a"));
        Assert.Throws<ArgumentNullException>(() => ReplaceWords.Replace(["a"], null!));
    }

    [Fact]
    public void RunsInTimeProportionalToTheInputLength()
    {
        string[] roots = WordGenerator.RandomWords(seed: 133, count: 10_000, minLength: 3, maxLength: 6);
        string[] words = WordGenerator.RandomWords(seed: 134, count: 200_000, minLength: 5, maxLength: 10);
        string sentence = string.Join(' ', words);

        var rootSet = roots.ToHashSet();
        string expected = string.Join(' ', words.Select(w =>
            Enumerable.Range(1, w.Length).Select(len => w[..len]).FirstOrDefault(rootSet.Contains) ?? w));

        string actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => ReplaceWords.Replace(roots, sentence),
            "Put the roots in a trie; for each word stop at the first node that ends a root.");

        Assert.Equal(expected, actual);
    }
}
