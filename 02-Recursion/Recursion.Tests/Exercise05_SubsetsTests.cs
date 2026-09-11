namespace DsaPractice.Recursion.Tests;

public class Exercise05_SubsetsTests
{
    // Makes the comparison independent of the order of subsets and of values inside each subset.
    private static List<string> Normalize(IEnumerable<IEnumerable<int>> subsets) =>
        subsets.Select(s => string.Join(",", s.Order())).Order(StringComparer.Ordinal).ToList();

    [Fact]
    public void EmptyInputHasOnlyTheEmptySubset()
    {
        var result = Subsets.Generate([]);

        var subset = Assert.Single(result);
        Assert.Empty(subset);
    }

    [Fact]
    public void GeneratesSubsetsOfTwoItems()
    {
        int[][] expected = [[], [1], [2], [1, 2]];

        Assert.Equal(Normalize(expected), Normalize(Subsets.Generate([1, 2])));
    }

    [Fact]
    public void GeneratesSubsetsOfThreeItems()
    {
        int[][] expected = [[], [1], [2], [3], [1, 2], [1, 3], [2, 3], [1, 2, 3]];

        Assert.Equal(Normalize(expected), Normalize(Subsets.Generate([1, 2, 3])));
    }

    [Fact]
    public void WorksWithNegativeValues()
    {
        int[][] expected = [[], [-5], [0], [-5, 0]];

        Assert.Equal(Normalize(expected), Normalize(Subsets.Generate([-5, 0])));
    }

    [Fact]
    public void GeneratesTwoToTheNDistinctSubsets()
    {
        int[] items = Enumerable.Range(1, 12).ToArray();

        var normalized = Normalize(Subsets.Generate(items));

        Assert.Equal(1 << items.Length, normalized.Count);
        Assert.Equal(normalized.Count, normalized.Distinct().Count());
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => Subsets.Generate(null!));
    }
}
