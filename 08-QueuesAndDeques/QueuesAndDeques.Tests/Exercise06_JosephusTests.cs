namespace DsaPractice.QueuesAndDeques.Tests;

public class Exercise06_JosephusTests
{
    [Theory]
    [InlineData(1, 1, new[] { 1 })]
    [InlineData(1, 5, new[] { 1 })]
    [InlineData(5, 1, new[] { 1, 2, 3, 4, 5 })]
    [InlineData(5, 2, new[] { 2, 4, 1, 5, 3 })]
    [InlineData(7, 3, new[] { 3, 6, 2, 7, 5, 1, 4 })]
    [InlineData(4, 6, new[] { 2, 1, 4, 3 })]
    public void ProducesEliminationOrder(int people, int step, int[] expected)
    {
        Assert.Equal(expected, Josephus.EliminationOrder(people, step));
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(5, 2, 3)]
    [InlineData(7, 3, 4)]
    [InlineData(41, 3, 31)]
    public void FindsTheSurvivor(int people, int step, int expected)
    {
        Assert.Equal(expected, Josephus.LastSurvivor(people, step));
    }

    [Fact]
    public void MatchesTheRecurrence()
    {
        for (int step = 1; step <= 10; step++)
        {
            int survivor = 0; // zero-based survivor for a circle of 1
            for (int people = 1; people <= 200; people++)
            {
                if (people > 1) survivor = (survivor + step) % people;
                Assert.Equal(survivor + 1, Josephus.LastSurvivor(people, step));
            }
        }
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(5, 0)]
    [InlineData(-1, -1)]
    public void ThrowsForInvalidArguments(int people, int step)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Josephus.EliminationOrder(people, step));
        Assert.Throws<ArgumentOutOfRangeException>(() => Josephus.LastSurvivor(people, step));
    }
}
