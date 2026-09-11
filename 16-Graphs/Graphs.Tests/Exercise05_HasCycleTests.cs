namespace DsaPractice.Graphs.Tests;

public class Exercise05_HasCycleTests
{
    public static TheoryData<int, int[][], bool> Cases => new()
    {
        { 0, [], false },
        { 3, [], false },
        { 3, [[0, 1], [1, 2], [2, 0]], true },
        { 3, [[0, 1], [1, 2]], false },
        { 4, [[0, 1], [0, 2], [1, 3], [2, 3]], false },
        { 4, [[3, 3]], true },
        { 2, [[0, 1], [1, 0]], true },
        { 6, [[0, 1], [1, 2], [3, 4], [4, 5], [5, 3]], true },
        { 5, [[4, 3], [3, 2], [2, 1], [1, 0]], false },
        { 5, [[0, 1], [1, 2], [2, 3], [3, 4], [0, 4], [1, 3]], false },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void DetectsCycles(int vertexCount, int[][] edges, bool expected)
    {
        Assert.Equal(expected, CycleDetection.HasCycleDirected(vertexCount, edges));
    }

    [Fact]
    public void MatchesKahnsAlgorithmOnRandomGraphs()
    {
        var random = new Random(145);
        for (int round = 0; round < 100; round++)
        {
            int n = random.Next(1, 15);
            int[][] edges = GraphTestData.RandomEdges(random, n, random.Next(0, 20), allowSelfLoops: true);

            var inDegree = new int[n];
            var adjacency = Enumerable.Range(0, n).Select(_ => new List<int>()).ToArray();
            foreach (var e in edges) { adjacency[e[0]].Add(e[1]); inDegree[e[1]]++; }
            var ready = new Queue<int>(Enumerable.Range(0, n).Where(v => inDegree[v] == 0));
            int processed = 0;
            while (ready.Count > 0)
            {
                int v = ready.Dequeue();
                processed++;
                foreach (int w in adjacency[v]) if (--inDegree[w] == 0) ready.Enqueue(w);
            }

            Assert.Equal(processed < n, CycleDetection.HasCycleDirected(n, edges));
        }
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => CycleDetection.HasCycleDirected(3, null!));
        Assert.Throws<ArgumentOutOfRangeException>(() => CycleDetection.HasCycleDirected(2, [[0, 2]]));
        Assert.Throws<ArgumentException>(() => CycleDetection.HasCycleDirected(2, [[0]]));
    }

    [Fact]
    public void HandlesLargeGraphs()
    {
        const int n = 200_000;
        int[][] chain = Enumerable.Range(0, n - 1).Select(i => new[] { i, i + 1 }).ToArray();
        int[][] chainWithBackEdge = [.. chain, [n - 1, n / 2]];

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            Assert.False(CycleDetection.HasCycleDirected(n, chain));
            Assert.True(CycleDetection.HasCycleDirected(n, chainWithBackEdge));
        }, "Each vertex should be explored once; use three states (unvisited, in progress, done).");
    }
}
