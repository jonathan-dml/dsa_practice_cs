namespace DsaPractice.Heaps.Tests;

public class Exercise08_LastStoneWeightTests
{
    [Theory]
    [InlineData(new int[] { }, 0)]
    [InlineData(new[] { 1 }, 1)]
    [InlineData(new[] { 3, 3 }, 0)]
    [InlineData(new[] { 2, 7, 4, 1, 8, 1 }, 1)]
    [InlineData(new[] { 10, 4, 2 }, 4)]
    [InlineData(new[] { 1, 1, 1 }, 1)]
    [InlineData(new[] { 9, 3, 2, 10 }, 0)]
    public void SmashesStones(int[] stones, int expected)
    {
        Assert.Equal(expected, LastStoneWeight.Smash(stones));
    }

    [Fact]
    public void MatchesSimulationOnRandomInputs()
    {
        var random = new Random(113);
        for (int round = 0; round < 50; round++)
        {
            int[] stones = Enumerable.Range(0, random.Next(0, 30)).Select(_ => random.Next(1, 50)).ToArray();

            var list = stones.ToList();
            while (list.Count > 1)
            {
                list.Sort();
                int y = list[^1], x = list[^2];
                list.RemoveRange(list.Count - 2, 2);
                if (y != x) list.Add(y - x);
            }

            Assert.Equal(list.Count == 0 ? 0 : list[0], LastStoneWeight.Smash(stones));
        }
    }

    [Fact]
    public void DoesNotModifyTheInput()
    {
        int[] stones = [2, 7, 4, 1, 8, 1];

        LastStoneWeight.Smash(stones);

        Assert.Equal([2, 7, 4, 1, 8, 1], stones);
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => LastStoneWeight.Smash(null!));
    }

    [Fact]
    public void RunsInLinearithmicTime()
    {
        var random = new Random(114);
        int[] stones = Enumerable.Range(0, 200_000).Select(_ => random.Next(1, 1_000_000)).ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => LastStoneWeight.Smash(stones),
            "Use a max-heap instead of re-sorting after every smash.");
    }
}
