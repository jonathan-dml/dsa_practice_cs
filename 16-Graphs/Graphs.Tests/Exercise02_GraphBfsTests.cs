namespace DsaPractice.Graphs.Tests;

public class Exercise02_GraphBfsTests
{
    private static readonly int[][] Sample = [[0, 1], [0, 2], [1, 3], [2, 4], [3, 5], [4, 5]];

    [Fact]
    public void VisitsVerticesLevelByLevel()
    {
        Assert.Equal([0, 1, 2, 3, 4, 5], GraphBfs.Order(6, Sample, 0));
        Assert.Equal([5, 3, 4, 1, 2, 0], GraphBfs.Order(6, Sample, 5));
    }

    [Fact]
    public void FollowsEdgeListOrder()
    {
        int[][] edges = [[0, 3], [0, 1], [0, 2], [1, 4]];

        Assert.Equal([0, 3, 1, 2, 4], GraphBfs.Order(5, edges, 0));
    }

    [Fact]
    public void OnlyVisitsReachableVertices()
    {
        int[][] edges = [[0, 1], [2, 3]];

        Assert.Equal([0, 1], GraphBfs.Order(4, edges, 0));
        Assert.Equal([4], GraphBfs.Order(5, edges, 4));
    }

    [Fact]
    public void RespectsDirection()
    {
        int[][] edges = [[0, 1], [2, 1], [1, 3]];

        Assert.Equal([1, 3], GraphBfs.Order(4, edges, 1, directed: true));
        Assert.Equal([1, 0, 2, 3], GraphBfs.Order(4, edges, 1, directed: false));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 1)]
    [InlineData(0, 3, 2)]
    [InlineData(0, 5, 3)]
    [InlineData(2, 3, 3)]
    public void FindsShortestPathLengths(int source, int target, int expected)
    {
        Assert.Equal(expected, GraphBfs.ShortestPathLength(6, Sample, source, target));
    }

    [Fact]
    public void ShortestPathIsMinusOneWhenUnreachable()
    {
        int[][] edges = [[0, 1], [1, 2]];

        Assert.Equal(-1, GraphBfs.ShortestPathLength(4, edges, 0, 3));
        Assert.Equal(-1, GraphBfs.ShortestPathLength(3, edges, 2, 0, directed: true));
        Assert.Equal(2, GraphBfs.ShortestPathLength(3, edges, 2, 0, directed: false));
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => GraphBfs.Order(3, null!, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => GraphBfs.Order(3, [[0, 1]], 3));
        Assert.Throws<ArgumentOutOfRangeException>(() => GraphBfs.Order(3, [[0, 5]], 0));
        Assert.Throws<ArgumentException>(() => GraphBfs.Order(3, [[0, 1, 2]], 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => GraphBfs.ShortestPathLength(3, [[0, 1]], 0, -1));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 200_000;
        int[][] edges = GraphTestData.ShuffledPath(n, seed: 141);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            Assert.Equal(Enumerable.Range(0, n), GraphBfs.Order(n, edges, 0));
            Assert.Equal(n - 1, GraphBfs.ShortestPathLength(n, edges, 0, n - 1));
        }, "Build adjacency lists once and use a bool[] for visited vertices.");
    }
}
