namespace DsaPractice.Graphs.Tests;

public class Exercise08_DisjointSetTests
{
    [Fact]
    public void EveryElementStartsAlone()
    {
        var set = new DisjointSet(5);

        Assert.Equal(5, set.SetCount);
        for (int i = 0; i < 5; i++) Assert.Equal(i, set.Find(i));
        Assert.False(set.Connected(0, 1));
        Assert.True(set.Connected(3, 3));
    }

    [Fact]
    public void UnionMergesSets()
    {
        var set = new DisjointSet(6);

        Assert.True(set.Union(0, 1));
        Assert.True(set.Union(2, 3));
        Assert.True(set.Union(1, 3));
        Assert.False(set.Union(0, 2));

        Assert.Equal(3, set.SetCount);
        Assert.True(set.Connected(0, 3));
        Assert.Equal(set.Find(0), set.Find(2));
        Assert.False(set.Connected(0, 4));
        Assert.False(set.Connected(4, 5));
    }

    [Fact]
    public void EmptySetIsAllowed()
    {
        Assert.Equal(0, new DisjointSet(0).SetCount);
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DisjointSet(-1));
        var set = new DisjointSet(3);
        Assert.Throws<ArgumentOutOfRangeException>(() => set.Find(3));
        Assert.Throws<ArgumentOutOfRangeException>(() => set.Union(-1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => set.Connected(0, 5));
    }

    [Fact]
    public void MatchesLabelModelOnRandomOperations()
    {
        const int n = 60;
        var random = new Random(150);
        var set = new DisjointSet(n);
        int[] label = Enumerable.Range(0, n).ToArray();

        for (int i = 0; i < 2000; i++)
        {
            int a = random.Next(n), b = random.Next(n);
            if (random.Next(2) == 0)
            {
                bool merged = label[a] != label[b];
                Assert.Equal(merged, set.Union(a, b));
                if (merged)
                {
                    int old = label[a];
                    for (int k = 0; k < n; k++) if (label[k] == old) label[k] = label[b];
                }
            }
            else
            {
                Assert.Equal(label[a] == label[b], set.Connected(a, b));
            }

            Assert.Equal(label.Distinct().Count(), set.SetCount);
        }
    }

    public static TheoryData<int, int[][], int> ComponentCases => new()
    {
        { 0, [], 0 },
        { 5, [], 5 },
        { 5, [[0, 1], [1, 2], [3, 4]], 2 },
        { 3, [[0, 1], [1, 2], [2, 0]], 1 },
        { 4, [[0, 0], [1, 1]], 4 },
        { 6, [[0, 5], [1, 4], [2, 3], [5, 1]], 2 },
    };

    [Theory]
    [MemberData(nameof(ComponentCases))]
    public void CountsConnectedComponents(int vertexCount, int[][] edges, int expected)
    {
        Assert.Equal(expected, ConnectedComponents.Count(vertexCount, edges));
    }

    [Fact]
    public void ComponentCountValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => ConnectedComponents.Count(3, null!));
        Assert.Throws<ArgumentOutOfRangeException>(() => ConnectedComponents.Count(3, [[0, 3]]));
        Assert.Throws<ArgumentException>(() => ConnectedComponents.Count(3, [[0, 1, 2]]));
    }

    [Fact]
    public void OperationsAreNearlyConstantTime()
    {
        const int n = 1_000_000;
        var set = new DisjointSet(n);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            for (int i = 0; i < n - 1; i++) set.Union(i, i + 1);
            int root = set.Find(0);
            for (int i = 0; i < n; i++)
            {
                if (set.Find(i) != root) Assert.Fail($"Element {i} is not connected.");
                if (set.Find(0) != root) Assert.Fail("Root changed.");
            }
        }, "Use path compression in Find and union by size.");

        Assert.Equal(1, set.SetCount);
    }
}
