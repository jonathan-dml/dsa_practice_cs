namespace DsaPractice.Graphs;

/// <summary>
/// Exercise 01 — A directed or undirected graph stored as adjacency lists. See README.md for details.
/// </summary>
public class Graph
{
    // Suggested fields:
    // private readonly Dictionary<int, List<int>> _adjacency = new();
    // private readonly HashSet<(int From, int To)> _edges = new();
    // private readonly List<int> _vertices = new();

    public Graph(bool directed)
    {
        // TODO: remember whether the graph is directed.
    }

    public bool IsDirected => throw new NotImplementedException();

    public int VertexCount => throw new NotImplementedException();

    public int EdgeCount => throw new NotImplementedException();

    public IReadOnlyList<int> Vertices => throw new NotImplementedException();

    public bool AddVertex(int vertex)
    {
        throw new NotImplementedException();
    }

    public bool AddEdge(int from, int to)
    {
        throw new NotImplementedException();
    }

    public bool RemoveEdge(int from, int to)
    {
        throw new NotImplementedException();
    }

    public bool HasEdge(int from, int to)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<int> Neighbors(int vertex)
    {
        throw new NotImplementedException();
    }
}
