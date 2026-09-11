namespace DsaPractice.Graphs.Tests;

public class Exercise01_GraphTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void NewGraphIsEmpty(bool directed)
    {
        var graph = new Graph(directed);

        Assert.Equal(directed, graph.IsDirected);
        Assert.Equal(0, graph.VertexCount);
        Assert.Equal(0, graph.EdgeCount);
        Assert.Empty(graph.Vertices);
    }

    [Fact]
    public void AddVertexRejectsDuplicates()
    {
        var graph = new Graph(directed: false);

        Assert.True(graph.AddVertex(7));
        Assert.False(graph.AddVertex(7));
        Assert.Equal(1, graph.VertexCount);
        Assert.Empty(graph.Neighbors(7));
    }

    [Fact]
    public void DirectedEdgesGoOneWay()
    {
        var graph = new Graph(directed: true);

        Assert.True(graph.AddEdge(1, 2));

        Assert.True(graph.HasEdge(1, 2));
        Assert.False(graph.HasEdge(2, 1));
        Assert.Equal([2], graph.Neighbors(1));
        Assert.Empty(graph.Neighbors(2));
        Assert.Equal(2, graph.VertexCount);
        Assert.Equal(1, graph.EdgeCount);
    }

    [Fact]
    public void UndirectedEdgesGoBothWaysButCountOnce()
    {
        var graph = new Graph(directed: false);

        Assert.True(graph.AddEdge(1, 2));

        Assert.True(graph.HasEdge(1, 2));
        Assert.True(graph.HasEdge(2, 1));
        Assert.Equal([2], graph.Neighbors(1));
        Assert.Equal([1], graph.Neighbors(2));
        Assert.Equal(1, graph.EdgeCount);
    }

    [Fact]
    public void DuplicateEdgesAreRejected()
    {
        var directed = new Graph(directed: true);
        Assert.True(directed.AddEdge(1, 2));
        Assert.False(directed.AddEdge(1, 2));
        Assert.True(directed.AddEdge(2, 1));
        Assert.Equal(2, directed.EdgeCount);

        var undirected = new Graph(directed: false);
        Assert.True(undirected.AddEdge(1, 2));
        Assert.False(undirected.AddEdge(2, 1));
        Assert.Equal(1, undirected.EdgeCount);
    }

    [Fact]
    public void KeepsInsertionOrder()
    {
        var graph = new Graph(directed: true);

        graph.AddVertex(5);
        graph.AddEdge(0, 3);
        graph.AddEdge(0, 1);
        graph.AddEdge(0, 2);

        Assert.Equal([5, 0, 3, 1, 2], graph.Vertices);
        Assert.Equal([3, 1, 2], graph.Neighbors(0));
    }

    [Fact]
    public void RemoveEdgeRemovesBothDirectionsInUndirectedGraphs()
    {
        var graph = new Graph(directed: false);
        graph.AddEdge(1, 2);
        graph.AddEdge(1, 3);

        Assert.True(graph.RemoveEdge(2, 1));

        Assert.False(graph.HasEdge(1, 2));
        Assert.False(graph.HasEdge(2, 1));
        Assert.Equal([3], graph.Neighbors(1));
        Assert.Empty(graph.Neighbors(2));
        Assert.Equal(1, graph.EdgeCount);
        Assert.Equal(3, graph.VertexCount);
        Assert.False(graph.RemoveEdge(1, 2));
    }

    [Fact]
    public void UnknownVertices()
    {
        var graph = new Graph(directed: true);
        graph.AddEdge(1, 2);

        Assert.Throws<KeyNotFoundException>(() => graph.Neighbors(42));
        Assert.False(graph.HasEdge(42, 1));
        Assert.False(graph.RemoveEdge(42, 1));
    }

    [Fact]
    public void SelfLoopsAppearOnce()
    {
        var graph = new Graph(directed: false);

        Assert.True(graph.AddEdge(4, 4));

        Assert.Equal([4], graph.Neighbors(4));
        Assert.True(graph.HasEdge(4, 4));
        Assert.Equal(1, graph.EdgeCount);
        Assert.True(graph.RemoveEdge(4, 4));
        Assert.Empty(graph.Neighbors(4));
    }

    [Fact]
    public void EdgeOperationsAreConstantTimeForHighDegreeVertices()
    {
        const int n = 200_000;
        var graph = new Graph(directed: false);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            for (int i = 1; i <= n; i++) graph.AddEdge(0, i);
            for (int i = 1; i <= n; i++)
            {
                if (graph.AddEdge(i, 0)) Assert.Fail("Duplicate edge accepted.");
                if (!graph.HasEdge(0, i)) Assert.Fail($"Missing edge 0-{i}.");
            }
        }, "Keep a hash set of edges so lookups don't scan neighbour lists.");

        Assert.Equal(n, graph.EdgeCount);
        Assert.Equal(n, graph.Neighbors(0).Count);
    }
}
