namespace DsaPractice.Recursion.Tests;

public class Exercise07_TowerOfHanoiTests
{
    [Fact]
    public void ZeroDisksNeedNoMoves()
    {
        Assert.Empty(TowerOfHanoi.Solve(0));
    }

    [Fact]
    public void OneDiskMovesDirectly()
    {
        Assert.Equal([('A', 'C')], TowerOfHanoi.Solve(1));
    }

    [Fact]
    public void TwoDisksUseTheSparePeg()
    {
        Assert.Equal([('A', 'B'), ('A', 'C'), ('B', 'C')], TowerOfHanoi.Solve(2));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(16)]
    public void ProducesAValidMinimalSolution(int disks)
    {
        var moves = TowerOfHanoi.Solve(disks);

        Assert.Equal((1 << disks) - 1, moves.Count);
        AssertSolves(disks, 'A', 'C', 'B', moves);
    }

    [Fact]
    public void RespectsCustomPegNames()
    {
        var moves = TowerOfHanoi.Solve(4, from: 'X', to: 'Z', via: 'Y');

        Assert.Equal(15, moves.Count);
        AssertSolves(4, 'X', 'Z', 'Y', moves);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(21)]
    public void ThrowsOutsideValidRange(int disks)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TowerOfHanoi.Solve(disks));
    }

    private static void AssertSolves(int disks, char from, char to, char via, IList<(char From, char To)> moves)
    {
        // Disk 1 is the smallest; the top of each stack is the top of the peg.
        var pegs = new Dictionary<char, Stack<int>>
        {
            [from] = new(Enumerable.Range(1, disks).Reverse()),
            [to] = new(),
            [via] = new(),
        };

        foreach (var (source, target) in moves)
        {
            Assert.True(pegs.ContainsKey(source) && pegs.ContainsKey(target), $"Unknown peg in move {source}->{target}.");
            Assert.True(pegs[source].Count > 0, $"Tried to move from empty peg {source}.");
            int disk = pegs[source].Pop();
            Assert.True(pegs[target].Count == 0 || pegs[target].Peek() > disk,
                $"Placed disk {disk} on top of a smaller disk on peg {target}.");
            pegs[target].Push(disk);
        }

        Assert.Equal(disks, pegs[to].Count);
    }
}
