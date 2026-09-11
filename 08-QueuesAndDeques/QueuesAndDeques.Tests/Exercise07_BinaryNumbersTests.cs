namespace DsaPractice.QueuesAndDeques.Tests;

public class Exercise07_BinaryNumbersTests
{
    [Fact]
    public void ZeroCountGivesEmptyList()
    {
        Assert.Empty(BinaryNumbers.Generate(0));
    }

    [Theory]
    [InlineData(1, new[] { "1" })]
    [InlineData(2, new[] { "1", "10" })]
    [InlineData(5, new[] { "1", "10", "11", "100", "101" })]
    [InlineData(8, new[] { "1", "10", "11", "100", "101", "110", "111", "1000" })]
    public void GeneratesBinaryNumbers(int count, string[] expected)
    {
        Assert.Equal(expected, BinaryNumbers.Generate(count));
    }

    [Fact]
    public void MatchesConvertToString()
    {
        const int count = 5000;

        var result = BinaryNumbers.Generate(count);

        Assert.Equal(Enumerable.Range(1, count).Select(i => Convert.ToString(i, 2)), result);
    }

    [Fact]
    public void ThrowsForNegativeCount()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => BinaryNumbers.Generate(-1));
    }
}
