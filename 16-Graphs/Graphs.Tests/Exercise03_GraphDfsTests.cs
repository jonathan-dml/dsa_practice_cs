namespace DsaPractice.Graphs.Tests;

public class Exercise03_GraphDfsTests
{
    private static readonly int[][] Sample = [[0, 1], [0, 2], [1, 3], [2, 4], [3, 5], [4, 5]];

    [Fact]
    public void VisitsInDepthFirstPreOrder()
    {
        Assert.Equal([0, 1, 3, 5, 4, 2], GraphDfs.Order(6, Sample, 0));
        Assert.Equal([2, 0, 1, 3, 5, 4], GraphDfs.Order(6, Sample, 2));
    }

    [Fact]
    public void FollowsEdgeListOrder()
    {
        int[][] edges = [[0, 3], [0, 1], [3, 2], [1, 4]];

        Assert.Equal([0, 3, 2, 1, 4], GraphDfs.Order(5, edges, 0));
    }

    [Fact]
    public void RespectsDirection()
    {
        int[][] edges = [[0, 1], [2, 1], [1, 3]];

        Assert.Equal([2, 1, 3], GraphDfs.Order(4, edges, 2, directed: true));
        Assert.Equal([3], GraphDfs.Order(4, edges, 3, directed: true));
    }

    [Fact]
    public void MatchesReferenceRecursiveDfsOnRandomGraphs()
    {
        var random = new Random(142);
        for (int round = 0; round < 30; round++)
        {
            int n = random.Next(1, 40);
            int[][] edges = GraphTestData.RandomEdges(random, n, random.Next(0, 80));
            bool directed = random.Next(2) == 0;
            int start = random.Next(n);

            var adjacency = Enumerable.Range(0, n).Select(_ => new List<int>()).ToArray();
            foreach (var e in edges)
            {
                adjacency[e[0]].Add(e[1]);
                if (!directed) adjacency[e[1]].Add(e[0]);
            }
            var expected = new List<int>();
            var visited = new bool[n];
            void Visit(int v)
            {
                visited[v] = true;
                expected.Add(v);
                foreach (int w in adjacency[v]) if (!visited[w]) Visit(w);
            }
            Visit(start);

            Assert.Equal(expected, GraphDfs.Order(n, edges, start, directed));
        }
    }

    [Theory]
    [InlineData(0, 5, false, true)]
    [InlineData(5, 0, false, true)]
    [InlineData(5, 0, true, false)]
    [InlineData(0, 0, true, true)]
    public void ChecksPaths(int source, int target, bool directed, bool expected)
    {
        Assert.Equal(expected, GraphDfs.HasPath(6, Sample, source, target, directed));
    }

    [Fact]
    public void NoPathBetweenComponents()
    {
        Assert.False(GraphDfs.HasPath(4, [[0, 1], [2, 3]], 0, 3));
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => GraphDfs.Order(3, null!, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => GraphDfs.Order(-1, [], 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => GraphDfs.HasPath(3, [[0, 1]], 0, 7));
    }

    [Fact]
    public void HandlesVeryDeepGraphs()
    {
        const int n = 200_000;
        int[][] edges = GraphTestData.ShuffledPath(n, seed: 143);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            Assert.Equal(Enumerable.Range(0, n), GraphDfs.Order(n, edges, 0));
            Assert.True(GraphDfs.HasPath(n, edges, n - 1, 0));
        }, "Build adjacency lists once; each vertex and edge should be processed once.");
    }
}
