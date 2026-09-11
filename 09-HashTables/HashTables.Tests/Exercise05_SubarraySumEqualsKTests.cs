namespace DsaPractice.HashTables.Tests;

public class Exercise05_SubarraySumEqualsKTests
{
    [Theory]
    [InlineData(new int[] { }, 0, 0)]
    [InlineData(new[] { 1, 1, 1 }, 2, 2)]
    [InlineData(new[] { 1, 2, 3 }, 3, 2)]
    [InlineData(new[] { 1, -1, 0 }, 0, 3)]
    [InlineData(new[] { 3, 4, 7, 2, -3, 1, 4, 2 }, 7, 4)]
    [InlineData(new[] { 5 }, 5, 1)]
    [InlineData(new[] { 5 }, 4, 0)]
    [InlineData(new[] { -1, -1, 1 }, 0, 1)]
    public void CountsSubarrays(int[] numbers, int k, long expected)
    {
        Assert.Equal(expected, SubarraySumEqualsK.Count(numbers, k));
    }

    [Fact]
    public void MatchesBruteForceOnRandomInputs()
    {
        var random = new Random(45);
        for (int round = 0; round < 50; round++)
        {
            int[] numbers = Enumerable.Range(0, random.Next(0, 80)).Select(_ => random.Next(-5, 6)).ToArray();
            int k = random.Next(-5, 6);
            long expected = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                long sum = 0;
                for (int j = i; j < numbers.Length; j++)
                {
                    sum += numbers[j];
                    if (sum == k) expected++;
                }
            }

            Assert.Equal(expected, SubarraySumEqualsK.Count(numbers, k));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => SubarraySumEqualsK.Count(null!, 0));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 200_000;
        int[] zeros = new int[n];

        long actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => SubarraySumEqualsK.Count(zeros, 0),
            "Count prefix sums in a dictionary.");

        Assert.Equal((long)n * (n + 1) / 2, actual);
    }
}
