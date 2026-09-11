namespace DsaPractice.Heaps.Tests;

public class Exercise06_KClosestPointsTests
{
    private static (int X, int Y)[] Expected((int X, int Y)[] points, int k) =>
        points.OrderBy(p => (long)p.X * p.X + (long)p.Y * p.Y).ThenBy(p => p.X).ThenBy(p => p.Y).Take(k).ToArray();

    [Fact]
    public void FindsTheSingleClosestPoint()
    {
        Assert.Equal([(-2, 2)], KClosestPoints.Find([(1, 3), (-2, 2)], 1));
    }

    [Fact]
    public void OrdersByDistance()
    {
        Assert.Equal([(3, 3), (-2, 4)], KClosestPoints.Find([(3, 3), (5, -1), (-2, 4)], 2));
    }

    [Fact]
    public void BreaksTiesByXThenY()
    {
        (int, int)[] points = [(1, 0), (0, 1), (-1, 0), (0, -1)];

        Assert.Equal([(-1, 0), (0, -1), (0, 1), (1, 0)], KClosestPoints.Find(points, 4));
        Assert.Equal([(-1, 0), (0, -1)], KClosestPoints.Find(points, 2));
    }

    [Fact]
    public void UsesLongDistances()
    {
        (int, int)[] points = [(100_000, 100_000), (-100_000, 99_999), (0, 0)];

        Assert.Equal([(0, 0), (-100_000, 99_999), (100_000, 100_000)], KClosestPoints.Find(points, 3));
    }

    [Fact]
    public void ZeroKReturnsEmpty()
    {
        Assert.Empty(KClosestPoints.Find([(1, 1)], 0));
    }

    [Fact]
    public void MatchesSortingOnRandomInputs()
    {
        var random = new Random(110);
        for (int round = 0; round < 50; round++)
        {
            var points = Enumerable.Range(0, random.Next(0, 60)).Select(_ => (random.Next(-10, 11), random.Next(-10, 11))).ToArray();
            int k = random.Next(0, points.Length + 1);

            Assert.Equal(Expected(points, k), KClosestPoints.Find(points, k));
        }
    }

    [Fact]
    public void ThrowsForInvalidArguments()
    {
        Assert.Throws<ArgumentNullException>(() => KClosestPoints.Find(null!, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => KClosestPoints.Find([(1, 1)], -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => KClosestPoints.Find([(1, 1)], 2));
    }

    [Fact]
    public void HandlesLargeInputs()
    {
        var random = new Random(111);
        var points = Enumerable.Range(0, 500_000).Select(_ => (random.Next(-100_000, 100_001), random.Next(-100_000, 100_001))).ToArray();
        var expected = Expected(points, 100);

        var actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => KClosestPoints.Find(points, 100),
            "Keep a max-heap of the k best points seen so far.");

        Assert.Equal(expected, actual);
    }
}
