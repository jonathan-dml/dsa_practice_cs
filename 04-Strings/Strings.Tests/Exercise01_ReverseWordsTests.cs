namespace DsaPractice.Strings.Tests;

public class Exercise01_ReverseWordsTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("   ", "")]
    [InlineData("hello", "hello")]
    [InlineData("the sky is blue", "blue is sky the")]
    [InlineData("  hello world  ", "world hello")]
    [InlineData("a good   example", "example good a")]
    [InlineData("C# is fun!", "fun! is C#")]
    public void ReversesWords(string sentence, string expected)
    {
        Assert.Equal(expected, WordReverser.ReverseWords(sentence));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => WordReverser.ReverseWords(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        string[] words = Enumerable.Range(0, 200_000).Select(i => "word" + i).ToArray();
        string sentence = string.Join("  ", words);
        string expected = string.Join(" ", words.Reverse());

        string actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => WordReverser.ReverseWords(sentence),
            "Avoid string concatenation in a loop; use string.Join or StringBuilder.");

        Assert.Equal(expected, actual);
    }
}
