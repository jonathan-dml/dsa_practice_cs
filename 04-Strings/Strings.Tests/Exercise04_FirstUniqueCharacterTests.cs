namespace DsaPractice.Strings.Tests;

public class Exercise04_FirstUniqueCharacterTests
{
    [Theory]
    [InlineData("", -1)]
    [InlineData("z", 0)]
    [InlineData("leetcode", 0)]
    [InlineData("loveleetcode", 2)]
    [InlineData("aabb", -1)]
    [InlineData("aabbc", 4)]
    [InlineData("aA", 0)]
    [InlineData("  x ", 2)]
    public void FindsFirstUniqueCharacter(string text, int expected)
    {
        Assert.Equal(expected, FirstUniqueCharacter.FindIndex(text));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => FirstUniqueCharacter.FindIndex(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        // 50 000 distinct characters, repeated twice, followed by one unique character.
        string block = new(Enumerable.Range(0, 50_000).Select(i => (char)(0x0100 + i)).ToArray());
        string text = block + block + "!";

        int actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => FirstUniqueCharacter.FindIndex(text),
            "Count every character first, then find the first one with a count of 1.");

        Assert.Equal(text.Length - 1, actual);
    }
}
