namespace DsaPractice.Graphs.Tests;

internal static class GraphTestData
{
    /// <summary>A path 0 - 1 - 2 - ... - (n-1), with the edges listed in a shuffled order.</summary>
    public static int[][] ShuffledPath(int vertexCount, int seed)
    {
        int[][] edges = Enumerable.Range(0, vertexCount - 1).Select(i => new[] { i, i + 1 }).ToArray();
        new Random(seed).Shuffle(edges);
        return edges;
    }

    public static int[][] RandomEdges(Random random, int vertexCount, int edgeCount, bool allowSelfLoops = false)
    {
        var edges = new List<int[]>();
        if (vertexCount < 2 && !allowSelfLoops) return []; // no valid edge exists
        while (edges.Count < edgeCount)
        {
            int from = random.Next(vertexCount), to = random.Next(vertexCount);
            if (from == to && !allowSelfLoops) continue;
            edges.Add([from, to]);
        }
        return edges.ToArray();
    }

    public static char[][] Grid(params string[] rows) => rows.Select(r => r.ToCharArray()).ToArray();
}
