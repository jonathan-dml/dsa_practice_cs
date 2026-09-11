namespace DsaPractice.Sorting.Tests;

public class Exercise07_MergeIntervalsTests
{
    public static TheoryData<int[][], int[][]> Cases => new()
    {
        { [], [] },
        { [[1, 5]], [[1, 5]] },
        { [[1, 3], [2, 6], [8, 10], [15, 18]], [[1, 6], [8, 10], [15, 18]] },
        { [[1, 4], [4, 5]], [[1, 5]] },
        { [[1, 4], [2, 3]], [[1, 4]] },
        { [[8, 10], [1, 3]], [[1, 3], [8, 10]] },
        { [[5, 5], [5, 5]], [[5, 5]] },
        { [[1, 2], [3, 4], [5, 6]], [[1, 2], [3, 4], [5, 6]] },
        { [[6, 8], [1, 9], [2, 4], [4, 7]], [[1, 9]] },
        { [[-10, -5], [-6, 0], [1, 1]], [[-10, 0], [1, 1]] },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void MergesIntervals(int[][] intervals, int[][] expected)
    {
        Assert.Equal(expected, MergeIntervals.Merge(intervals));
    }

    public static TheoryData<int[][]> InvalidIntervals => new()
    {
        new[] { new[] { 3, 1 } },
        new[] { new[] { 1, 2, 3 } },
        new[] { new[] { 1 } },
    };

    [Theory]
    [MemberData(nameof(InvalidIntervals))]
    public void ThrowsForInvalidIntervals(int[][] intervals)
    {
        Assert.Throws<ArgumentException>(() => MergeIntervals.Merge(intervals));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => MergeIntervals.Merge(null!));
    }

    [Fact]
    public void MatchesCoverageOnRandomInputs()
    {
        var random = new Random(78);
        for (int round = 0; round < 50; round++)
        {
            int[][] intervals = Enumerable.Range(0, random.Next(0, 20)).Select(_ =>
            {
                int start = random.Next(0, 100);
                return new[] { start, start + random.Next(0, 10) };
            }).ToArray();

            int[][] merged = MergeIntervals.Merge(intervals);

            // Sorted, non-overlapping, non-touching...
            for (int i = 1; i < merged.Length; i++) Assert.True(merged[i - 1][1] < merged[i][0]);
            // ...and covering exactly the same integer points.
            for (int x = 0; x <= 110; x++)
            {
                bool inInput = intervals.Any(iv => iv[0] <= x && x <= iv[1]);
                bool inOutput = merged.Any(iv => iv[0] <= x && x <= iv[1]);
                Assert.Equal(inInput, inOutput);
            }
            // Endpoints come from the input intervals.
            Assert.All(merged, iv => Assert.Contains(intervals, orig => orig[0] == iv[0]));
        }
    }

    [Fact]
    public void RunsInLinearithmicTime()
    {
        var random = new Random(79);
        int[][] intervals = Enumerable.Range(0, 200_000).Select(_ =>
        {
            int start = random.Next(0, 100_000_000);
            return new[] { start, start + random.Next(0, 100) };
        }).ToArray();

        int[][] merged = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => MergeIntervals.Merge(intervals),
            "Sort by start once, then merge in a single pass.");

        for (int i = 1; i < merged.Length; i++) Assert.True(merged[i - 1][1] < merged[i][0]);
    }
}
