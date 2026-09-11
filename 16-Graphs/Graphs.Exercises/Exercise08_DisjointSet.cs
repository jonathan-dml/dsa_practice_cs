namespace DsaPractice.Graphs;

/// <summary>
/// Exercise 08 — Union-find with path compression and union by size. See README.md for details.
/// </summary>
public class DisjointSet
{
    // Suggested fields:
    // private readonly int[] _parent;
    // private readonly int[] _size;
    // private int _setCount;

    public DisjointSet(int size)
    {
        // TODO: validate size; every element starts in its own set.
    }

    public int SetCount => throw new NotImplementedException();

    public int Find(int element)
    {
        throw new NotImplementedException();
    }

    public bool Union(int first, int second)
    {
        throw new NotImplementedException();
    }

    public bool Connected(int first, int second)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Exercise 08 (part 2) — Count connected components of an undirected graph using <see cref="DisjointSet"/>.
/// </summary>
public static class ConnectedComponents
{
    public static int Count(int vertexCount, int[][] edges)
    {
        throw new NotImplementedException();
    }
}
