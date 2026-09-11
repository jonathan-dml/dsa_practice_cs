namespace DsaPractice.Heaps.Tests;

public class Exercise04_TopKFrequentTests
{
    [Theory]
    [InlineData(new[] { 1, 1, 1, 2, 2, 3 }, 2, new[] { 1, 2 })]
    [InlineData(new[] { 1 }, 1, new[] { 1 })]
    [InlineData(new[] { 4, 4, 5, 5, 6 }, 2, new[] { 4, 5 })]
    [InlineData(new[] { 3, 3, -1, -1, 2 }, 3, new[] { -1, 3, 2 })]
    [InlineData(new[] { 1, 2 }, 5, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, 0, new int[] { })]
    [InlineData(new int[] { }, 2, new int[] { })]
    [InlineData(new[] { 7, 7, 7, 8, 8, 9, 9, 9, 9 }, 3, new[] { 9, 7, 8 })]
    public void ReturnsMostFrequentElements(int[] numbers, int k, int[] expected)
    {
        Assert.Equal(expected, TopKFrequent.Elements(numbers, k));
    }

    [Fact]
    public void MatchesLinqOnRandomInputs()
    {
        var random = new Random(107);
        for (int round = 0; round < 30; round++)
        {
            int[] numbers = Enumerable.Range(0, random.Next(0, 500)).Select(_ => random.Next(-30, 30)).ToArray();
            int k = random.Next(0, 20);
            var expected = numbers.GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key)
                .Take(k)
                .Select(g => g.Key);

            Assert.Equal(expected, TopKFrequent.Elements(numbers, k));
        }
    }

    [Fact]
    public void ThrowsForInvalidArguments()
    {
        Assert.Throws<ArgumentNullException>(() => TopKFrequent.Elements(null!, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TopKFrequent.Elements([1], -1));
    }
}
