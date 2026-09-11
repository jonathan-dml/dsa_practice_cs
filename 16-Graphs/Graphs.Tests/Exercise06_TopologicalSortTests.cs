namespace DsaPractice.Graphs.Tests;

public class Exercise06_TopologicalSortTests
{
    private static void AssertValidOrder(int courseCount, int[][] prerequisites, int[]? order)
    {
        Assert.NotNull(order);
        Assert.Equal(Enumerable.Range(0, courseCount), order.Order());
        var position = new int[courseCount];
        for (int i = 0; i < order.Length; i++) position[order[i]] = i;
        foreach (var p in prerequisites)
        {
            Assert.True(position[p[1]] < position[p[0]], $"Course {p[1]} must come before course {p[0]}.");
        }
    }

    public static TheoryData<int, int[][]> Possible => new()
    {
        { 0, [] },
        { 1, [] },
        { 2, [[1, 0]] },
        { 4, [[1, 0], [2, 0], [3, 1], [3, 2]] },
        { 5, [] },
        { 6, [[5, 4], [4, 3], [3, 2], [2, 1], [1, 0]] },
        { 3, [[2, 0], [2, 1], [2, 0]] },
    };

    [Theory]
    [MemberData(nameof(Possible))]
    public void ReturnsAValidOrder(int courseCount, int[][] prerequisites)
    {
        AssertValidOrder(courseCount, prerequisites, TopologicalSort.CourseOrder(courseCount, prerequisites));
    }

    public static TheoryData<int, int[][]> Impossible => new()
    {
        { 2, [[1, 0], [0, 1]] },
        { 1, [[0, 0]] },
        { 4, [[1, 0], [2, 1], [3, 2], [1, 3]] },
    };

    [Theory]
    [MemberData(nameof(Impossible))]
    public void ReturnsNullWhenThereIsACycle(int courseCount, int[][] prerequisites)
    {
        Assert.Null(TopologicalSort.CourseOrder(courseCount, prerequisites));
    }

    [Fact]
    public void WorksOnRandomDags()
    {
        var random = new Random(146);
        for (int round = 0; round < 50; round++)
        {
            int n = random.Next(1, 30);
            int[] hidden = Enumerable.Range(0, n).ToArray();
            random.Shuffle(hidden);
            int[][] prerequisites = Enumerable.Range(0, random.Next(0, 60))
                .Select(_ =>
                {
                    int i = random.Next(n), j = random.Next(n);
                    return (i, j);
                })
                .Where(p => p.i != p.j)
                .Select(p => new[] { hidden[Math.Max(p.i, p.j)], hidden[Math.Min(p.i, p.j)] })
                .ToArray();

            AssertValidOrder(n, prerequisites, TopologicalSort.CourseOrder(n, prerequisites));
        }
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => TopologicalSort.CourseOrder(2, null!));
        Assert.Throws<ArgumentOutOfRangeException>(() => TopologicalSort.CourseOrder(2, [[0, 2]]));
    }

    [Fact]
    public void HandlesLargeInputs()
    {
        const int n = 200_000;
        int[][] prerequisites = Enumerable.Range(1, n - 1).Select(i => new[] { i, i - 1 }).ToArray();
        new Random(147).Shuffle(prerequisites);

        int[]? order = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => TopologicalSort.CourseOrder(n, prerequisites),
            "Use Kahn's algorithm with in-degree counts and a queue.");

        Assert.Equal(Enumerable.Range(0, n), order!);
    }
}
