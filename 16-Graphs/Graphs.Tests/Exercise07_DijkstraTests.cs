namespace DsaPractice.Graphs.Tests;

public class Exercise07_DijkstraTests
{
    private static long[] BellmanFord(int n, int[][] edges, int source)
    {
        var dist = Enumerable.Repeat(long.MaxValue, n).ToArray();
        dist[source] = 0;
        for (int i = 0; i < n - 1; i++)
            foreach (var e in edges)
                if (dist[e[0]] != long.MaxValue && dist[e[0]] + e[2] < dist[e[1]])
                    dist[e[1]] = dist[e[0]] + e[2];
        return dist.Select(d => d == long.MaxValue ? -1 : d).ToArray();
    }

    [Fact]
    public void FindsShortestDistances()
    {
        int[][] edges = [[0, 1, 4], [0, 2, 1], [2, 1, 2], [1, 3, 1], [2, 3, 5], [3, 4, 3]];

        Assert.Equal([0, 3, 1, 4, 7], Dijkstra.ShortestDistances(5, edges, 0));
    }

    [Fact]
    public void EdgesAreDirected()
    {
        int[][] edges = [[0, 1, 5], [1, 2, 5]];

        Assert.Equal([-1, -1, 0], Dijkstra.ShortestDistances(3, edges, 2));
        Assert.Equal([-1, 0, 5], Dijkstra.ShortestDistances(3, edges, 1));
    }

    [Fact]
    public void HandlesZeroWeightsParallelEdgesAndSelfLoops()
    {
        int[][] edges = [[0, 1, 10], [0, 1, 3], [1, 1, 0], [1, 2, 0], [2, 3, 7], [0, 3, 11]];

        Assert.Equal([0, 3, 3, 10], Dijkstra.ShortestDistances(4, edges, 0));
    }

    [Fact]
    public void UsesLongDistances()
    {
        int[][] edges = [[0, 1, int.MaxValue], [1, 2, int.MaxValue], [2, 3, int.MaxValue]];

        Assert.Equal([0, int.MaxValue, 2L * int.MaxValue, 3L * int.MaxValue], Dijkstra.ShortestDistances(4, edges, 0));
    }

    [Fact]
    public void MatchesBellmanFordOnRandomGraphs()
    {
        var random = new Random(148);
        for (int round = 0; round < 50; round++)
        {
            int n = random.Next(1, 30);
            int[][] edges = Enumerable.Range(0, random.Next(0, 100))
                .Select(_ => new[] { random.Next(n), random.Next(n), random.Next(0, 50) })
                .ToArray();
            int source = random.Next(n);

            Assert.Equal(BellmanFord(n, edges, source), Dijkstra.ShortestDistances(n, edges, source));
        }
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => Dijkstra.ShortestDistances(2, null!, 0));
        Assert.Throws<ArgumentException>(() => Dijkstra.ShortestDistances(2, [[0, 1, -1]], 0));
        Assert.Throws<ArgumentException>(() => Dijkstra.ShortestDistances(2, [[0, 1]], 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Dijkstra.ShortestDistances(2, [[0, 1, 1]], 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Dijkstra.ShortestDistances(2, [[0, 3, 1]], 0));
    }

    [Fact]
    public void RunsInLinearithmicTime()
    {
        const int n = 100_000;
        var random = new Random(149);
        var edgeList = new List<int[]>();
        for (int i = 0; i < n - 1; i++) edgeList.Add([i, i + 1, random.Next(1, 100)]);
        for (int i = 0; i < 400_000; i++) edgeList.Add([random.Next(n), random.Next(n), random.Next(1, 1_000_000)]);
        int[][] edges = edgeList.ToArray();

        long[] distances = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(3), () => Dijkstra.ShortestDistances(n, edges, 0),
            "Use a PriorityQueue keyed by tentative distance and skip outdated entries.");

        Assert.Equal(0, distances[0]);
        Assert.All(distances, d => Assert.True(d >= 0));
        foreach (var e in edges) Assert.True(distances[e[1]] <= distances[e[0]] + e[2]);
    }
}
