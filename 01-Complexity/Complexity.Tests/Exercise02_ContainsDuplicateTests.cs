namespace DsaPractice.Complexity.Tests;

public class Exercise02_ContainsDuplicateTests
{
    [Theory]
    [InlineData(new int[] { }, false)]
    [InlineData(new[] { 7 }, false)]
    [InlineData(new[] { 1, 2, 3, 4 }, false)]
    [InlineData(new[] { 1, 2, 3, 1 }, true)]
    [InlineData(new[] { 5, 5 }, true)]
    [InlineData(new[] { -1, 0, 1, -1 }, true)]
    [InlineData(new[] { int.MinValue, int.MaxValue, 0 }, false)]
    public void DetectsDuplicates(int[] numbers, bool expected)
    {
        Assert.Equal(expected, ContainsDuplicate.HasDuplicate(numbers));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => ContainsDuplicate.HasDuplicate(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 1_000_000;
        int[] distinct = Enumerable.Range(0, n).ToArray();
        new Random(7).Shuffle(distinct);
        int[] duplicateAtEnd = [.. distinct, distinct[0]];

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            Assert.False(ContainsDuplicate.HasDuplicate(distinct));
            Assert.True(ContainsDuplicate.HasDuplicate(duplicateAtEnd));
        }, "A HashSet<int> gives O(1) average lookups.");
    }
}
