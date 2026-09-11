using System.Numerics;

namespace DsaPractice.DynamicProgramming.Tests;

public class Exercise08_UniquePathsTests
{
    public static TheoryData<int[][], long> Cases => new()
    {
        { [], 0 },
        { [[0]], 1 },
        { [[1]], 0 },
        { [[0, 0, 0], [0, 1, 0], [0, 0, 0]], 2 },
        { [[0, 1], [0, 0]], 1 },
        { [[0, 0], [1, 1], [0, 0]], 0 },
        { [[0, 0, 0, 0]], 1 },
        { [[0], [0], [0]], 1 },
        { [[0, 0, 0], [0, 0, 0]], 3 },
        { [[0, 0], [0, 1]], 0 },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void CountsPaths(int[][] grid, long expected)
    {
        Assert.Equal(expected, UniquePaths.WithObstacles(grid));
    }

    [Theory]
    [InlineData(3, 7)]
    [InlineData(10, 10)]
    [InlineData(17, 23)]
    [InlineData(30, 30)]
    public void EmptyGridsMatchTheBinomialCoefficient(int rows, int cols)
    {
        int[][] grid = Enumerable.Range(0, rows).Select(_ => new int[cols]).ToArray();
        BigInteger expected = 1;
        for (int i = 1; i <= rows - 1; i++) expected = expected * (cols - 1 + i) / i;

        long actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => UniquePaths.WithObstacles(grid),
            "Count paths per cell: paths(r, c) = paths(r - 1, c) + paths(r, c - 1).");

        Assert.Equal((long)expected, actual);
    }

    [Fact]
    public void MatchesBruteForceOnRandomSmallGrids()
    {
        var random = new Random(168);
        for (int round = 0; round < 50; round++)
        {
            int rows = random.Next(1, 7), cols = random.Next(1, 7);
            int[][] grid = Enumerable.Range(0, rows).Select(_ => Enumerable.Range(0, cols).Select(_ => random.Next(5) == 0 ? 1 : 0).ToArray()).ToArray();

            long Walk(int r, int c) =>
                r >= rows || c >= cols || grid[r][c] == 1 ? 0 : r == rows - 1 && c == cols - 1 ? 1 : Walk(r + 1, c) + Walk(r, c + 1);

            Assert.Equal(Walk(0, 0), UniquePaths.WithObstacles(grid));
        }
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => UniquePaths.WithObstacles(null!));
        Assert.Throws<ArgumentException>(() => UniquePaths.WithObstacles([[0, 0], [0]]));
        Assert.Throws<ArgumentException>(() => UniquePaths.WithObstacles([[0, 2]]));
    }
}
