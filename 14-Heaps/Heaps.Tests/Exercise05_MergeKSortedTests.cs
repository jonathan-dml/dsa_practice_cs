namespace DsaPractice.Heaps.Tests;

public class Exercise05_MergeKSortedTests
{
    public static TheoryData<int[][], int[]> Cases => new()
    {
        { [], [] },
        { [[]], [] },
        { [[], [1]], [1] },
        { [[1, 4, 5], [1, 3, 4], [2, 6]], [1, 1, 2, 3, 4, 4, 5, 6] },
        { [[-3, 0], [-5, 10], [7]], [-5, -3, 0, 7, 10] },
        { [[1, 2, 3]], [1, 2, 3] },
        { [[2, 2], [2], [2, 2, 2]], [2, 2, 2, 2, 2, 2] },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void MergesSortedArrays(int[][] arrays, int[] expected)
    {
        Assert.Equal(expected, MergeKSorted.Merge(arrays));
    }

    [Fact]
    public void MatchesSortOnRandomInputs()
    {
        var random = new Random(108);
        for (int round = 0; round < 30; round++)
        {
            int[][] arrays = Enumerable.Range(0, random.Next(0, 15))
                .Select(_ => Enumerable.Range(0, random.Next(0, 20)).Select(_ => random.Next(-100, 100)).Order().ToArray())
                .ToArray();

            Assert.Equal(arrays.SelectMany(a => a).Order(), MergeKSorted.Merge(arrays));
        }
    }

    [Fact]
    public void ThrowsForInvalidInput()
    {
        Assert.Throws<ArgumentNullException>(() => MergeKSorted.Merge(null!));
        Assert.Throws<ArgumentException>(() => MergeKSorted.Merge([[1], null!]));
    }

    [Fact]
    public void HandlesManyArrays()
    {
        var random = new Random(109);
        int[][] arrays = Enumerable.Range(0, 2000)
            .Select(_ => Enumerable.Range(0, 500).Select(_ => random.Next()).Order().ToArray())
            .ToArray();
        int[] expected = arrays.SelectMany(a => a).Order().ToArray();

        int[] actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => MergeKSorted.Merge(arrays),
            "Keep only the current head of each array in a priority queue.");

        Assert.Equal(expected, actual);
    }
}
