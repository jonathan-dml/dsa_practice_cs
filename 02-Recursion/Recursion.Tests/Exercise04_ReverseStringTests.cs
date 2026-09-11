namespace DsaPractice.Recursion.Tests;

public class Exercise04_ReverseStringTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("a", "a")]
    [InlineData("ab", "ba")]
    [InlineData("hello", "olleh")]
    [InlineData("racecar", "racecar")]
    [InlineData("C# 12!", "!21 #C")]
    public void ReversesStrings(string input, string expected)
    {
        Assert.Equal(expected, StringReverser.Reverse(input));
    }

    [Fact]
    public void ReversesLongerStrings()
    {
        var random = new Random(1);
        char[] chars = Enumerable.Range(0, 1000).Select(_ => (char)random.Next('a', 'z' + 1)).ToArray();
        string input = new(chars);
        Array.Reverse(chars);

        Assert.Equal(new string(chars), StringReverser.Reverse(input));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => StringReverser.Reverse(null!));
    }
}
