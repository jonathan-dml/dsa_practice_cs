namespace DsaPractice.DynamicProgramming.Tests;

public class Exercise07_EditDistanceTests
{
    [Theory]
    [InlineData("horse", "ros", 3)]
    [InlineData("intention", "execution", 5)]
    [InlineData("kitten", "sitting", 3)]
    [InlineData("", "abc", 3)]
    [InlineData("abc", "", 3)]
    [InlineData("", "", 0)]
    [InlineData("same", "same", 0)]
    [InlineData("abc", "acb", 2)]
    [InlineData("a", "A", 1)]
    [InlineData("flaw", "lawn", 2)]
    public void ComputesDistance(string source, string target, int expected)
    {
        Assert.Equal(expected, EditDistance.Compute(source, target));
    }

    [Fact]
    public void IsSymmetricAndSatisfiesTriangleInequality()
    {
        var random = new Random(167);
        string Word() => new(Enumerable.Range(0, random.Next(0, 12)).Select(_ => (char)random.Next('a', 'e')).ToArray());

        for (int round = 0; round < 100; round++)
        {
            string a = Word(), b = Word(), c = Word();
            int ab = EditDistance.Compute(a, b);

            Assert.Equal(ab, EditDistance.Compute(b, a));
            Assert.True(ab <= EditDistance.Compute(a, c) + EditDistance.Compute(c, b));
            Assert.InRange(ab, Math.Abs(a.Length - b.Length), Math.Max(a.Length, b.Length));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => EditDistance.Compute(null!, "a"));
        Assert.Throws<ArgumentNullException>(() => EditDistance.Compute("a", null!));
    }

    [Fact]
    public void RunsInQuadraticTime()
    {
        string a = string.Concat(Enumerable.Repeat("abcd", 500));
        string b = string.Concat(Enumerable.Repeat("bcda", 500));

        int actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => EditDistance.Compute(a, b),
            "Fill a table over prefixes; each cell depends on three neighbours.");

        Assert.Equal(2, actual);
    }
}
