namespace DsaPractice.BinarySearch.Tests;

public class Exercise03_SearchInsertTests
{
    [Theory]
    [InlineData(new[] { 1, 3, 5, 6 }, 5, 2)]
    [InlineData(new[] { 1, 3, 5, 6 }, 2, 1)]
    [InlineData(new[] { 1, 3, 5, 6 }, 7, 4)]
    [InlineData(new[] { 1, 3, 5, 6 }, 0, 0)]
    [InlineData(new[] { 1, 3, 5, 6 }, 1, 0)]
    [InlineData(new[] { 1, 3, 5, 6 }, 6, 3)]
    [InlineData(new int[] { }, 42, 0)]
    [InlineData(new[] { 10 }, 5, 0)]
    [InlineData(new[] { 10 }, 15, 1)]
    public void FindsPosition(int[] sorted, int target, int expected)
    {
        Assert.Equal(expected, SearchInsert.Position(sorted, target));
    }

    [Fact]
    public void MatchesCountOfSmallerElementsOnRandomInputs()
    {
        var random = new Random(63);
        for (int round = 0; round < 50; round++)
        {
            int[] sorted = Enumerable.Range(0, random.Next(0, 100)).Select(_ => random.Next(-100, 100)).Distinct().Order().ToArray();
            for (int target = -105; target <= 105; target += 3)
            {
                Assert.Equal(sorted.Count(x => x < target), SearchInsert.Position(sorted, target));
            }
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => SearchInsert.Position(null!, 1));
    }

    [Fact]
    public void RunsInLogarithmicTime()
    {
        const int n = 1_000_000;
        int[] evens = Enumerable.Range(0, n).Select(i => i * 2).ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < n; i++)
            {
                if (SearchInsert.Position(evens, i * 2 + 1) != i + 1) Assert.Fail($"Wrong position for {i * 2 + 1}.");
            }
        });
    }
}
