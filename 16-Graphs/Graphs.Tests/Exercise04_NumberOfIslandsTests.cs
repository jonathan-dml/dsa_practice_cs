using static DsaPractice.Graphs.Tests.GraphTestData;

namespace DsaPractice.Graphs.Tests;

public class Exercise04_NumberOfIslandsTests
{
    [Fact]
    public void CountsIslands()
    {
        var grid = Grid(
            "11000",
            "11000",
            "00100",
            "00011");

        Assert.Equal(3, NumberOfIslands.Count(grid));
    }

    [Fact]
    public void OneBigIsland()
    {
        var grid = Grid(
            "11110",
            "11010",
            "11000",
            "00000");

        Assert.Equal(1, NumberOfIslands.Count(grid));
    }

    [Fact]
    public void DiagonalCellsAreNotConnected()
    {
        var grid = Grid(
            "101",
            "010",
            "101");

        Assert.Equal(5, NumberOfIslands.Count(grid));
    }

    [Fact]
    public void IslandsCanWrapAroundWater()
    {
        var grid = Grid(
            "11111",
            "10001",
            "10101",
            "10001",
            "11111");

        Assert.Equal(2, NumberOfIslands.Count(grid));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void EdgeCases(int variant)
    {
        char[][] grid = variant == 0 ? [] : Grid("000", "000");

        Assert.Equal(0, NumberOfIslands.Count(grid));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => NumberOfIslands.Count(null!));
    }

    [Fact]
    public void MatchesUnionFindOnRandomGrids()
    {
        var random = new Random(144);
        for (int round = 0; round < 30; round++)
        {
            int rows = random.Next(1, 25), cols = random.Next(1, 25);
            char[][] grid = Enumerable.Range(0, rows)
                .Select(_ => Enumerable.Range(0, cols).Select(_ => random.Next(10) < 4 ? '1' : '0').ToArray())
                .ToArray();

            var parent = Enumerable.Range(0, rows * cols).ToArray();
            int Find(int x) => parent[x] == x ? x : parent[x] = Find(parent[x]);
            int land = 0, merges = 0;
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (grid[r][c] != '1') continue;
                    land++;
                    foreach (var (nr, nc) in new[] { (r - 1, c), (r, c - 1) })
                    {
                        if (nr < 0 || nc < 0 || grid[nr][nc] != '1') continue;
                        int a = Find(r * cols + c), b = Find(nr * cols + nc);
                        if (a != b) { parent[a] = b; merges++; }
                    }
                }

            Assert.Equal(land - merges, NumberOfIslands.Count(grid));
        }
    }

    [Fact]
    public void HandlesLargeGrids()
    {
        char[][] allLand = Enumerable.Range(0, 1000).Select(_ => Enumerable.Repeat('1', 1000).ToArray()).ToArray();
        char[][] checkerboard = Enumerable.Range(0, 1000)
            .Select(r => Enumerable.Range(0, 1000).Select(c => (r + c) % 2 == 0 ? '1' : '0').ToArray())
            .ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(3), () =>
        {
            Assert.Equal(1, NumberOfIslands.Count(allLand));
            Assert.Equal(500_000, NumberOfIslands.Count(checkerboard));
        }, "Visit each cell once, sinking land cells as you explore them.");
    }
}
