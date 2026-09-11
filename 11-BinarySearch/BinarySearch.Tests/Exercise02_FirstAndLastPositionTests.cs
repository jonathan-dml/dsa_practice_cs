namespace DsaPractice.BinarySearch.Tests;

public class Exercise02_FirstAndLastPositionTests
{
    [Theory]
    [InlineData(new[] { 5, 7, 7, 8, 8, 10 }, 8, 3, 4)]
    [InlineData(new[] { 5, 7, 7, 8, 8, 10 }, 7, 1, 2)]
    [InlineData(new[] { 5, 7, 7, 8, 8, 10 }, 5, 0, 0)]
    [InlineData(new[] { 5, 7, 7, 8, 8, 10 }, 10, 5, 5)]
    [InlineData(new[] { 5, 7, 7, 8, 8, 10 }, 6, -1, -1)]
    [InlineData(new[] { 5, 7, 7, 8, 8, 10 }, 11, -1, -1)]
    [InlineData(new int[] { }, 0, -1, -1)]
    [InlineData(new[] { 1 }, 1, 0, 0)]
    [InlineData(new[] { 2, 2, 2, 2 }, 2, 0, 3)]
    public void FindsBoundaries(int[] sorted, int target, int first, int last)
    {
        Assert.Equal((first, last), FirstAndLastPosition.Find(sorted, target));
    }

    [Fact]
    public void MatchesIndexOfAndLastIndexOfOnRandomInputs()
    {
        var random = new Random(62);
        for (int round = 0; round < 50; round++)
        {
            int[] sorted = Enumerable.Range(0, random.Next(0, 100)).Select(_ => random.Next(0, 15)).Order().ToArray();
            for (int target = -1; target <= 16; target++)
            {
                var expected = (Array.IndexOf(sorted, target), Array.LastIndexOf(sorted, target));

                Assert.Equal(expected, FirstAndLastPosition.Find(sorted, target));
            }
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => FirstAndLastPosition.Find(null!, 1));
    }

    [Fact]
    public void RunsInLogarithmicTimeWithManyDuplicates()
    {
        int[] sorted = [.. Enumerable.Repeat(1, 500_000), .. Enumerable.Repeat(2, 1_000_000), .. Enumerable.Repeat(3, 500_000)];

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < 2000; i++)
            {
                if (FirstAndLastPosition.Find(sorted, 2) != (500_000, 1_499_999)) Assert.Fail("Wrong boundaries for 2.");
                if (FirstAndLastPosition.Find(sorted, 3) != (1_500_000, 1_999_999)) Assert.Fail("Wrong boundaries for 3.");
            }
        }, "Run two boundary searches instead of walking outward from a match.");
    }
}
