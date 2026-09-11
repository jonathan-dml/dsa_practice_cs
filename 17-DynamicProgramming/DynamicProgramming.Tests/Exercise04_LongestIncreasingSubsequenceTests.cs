namespace DsaPractice.DynamicProgramming.Tests;

public class Exercise04_LongestIncreasingSubsequenceTests
{
    [Theory]
    [InlineData(new int[] { }, 0)]
    [InlineData(new[] { 5 }, 1)]
    [InlineData(new[] { 10, 9, 2, 5, 3, 7, 101, 18 }, 4)]
    [InlineData(new[] { 0, 1, 0, 3, 2, 3 }, 4)]
    [InlineData(new[] { 7, 7, 7, 7 }, 1)]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 5)]
    [InlineData(new[] { 5, 4, 3, 2, 1 }, 1)]
    [InlineData(new[] { 4, 10, 4, 3, 8, 9 }, 3)]
    [InlineData(new[] { int.MinValue, 0, int.MaxValue }, 3)]
    public void FindsLength(int[] numbers, int expected)
    {
        Assert.Equal(expected, LongestIncreasingSubsequence.Length(numbers));
    }

    [Fact]
    public void MatchesQuadraticDpOnRandomInputs()
    {
        var random = new Random(163);
        for (int round = 0; round < 50; round++)
        {
            int[] numbers = Enumerable.Range(0, random.Next(0, 200)).Select(_ => random.Next(-50, 50)).ToArray();
            int[] dp = new int[numbers.Length];
            for (int i = 0; i < numbers.Length; i++)
            {
                dp[i] = 1;
                for (int j = 0; j < i; j++) if (numbers[j] < numbers[i]) dp[i] = Math.Max(dp[i], dp[j] + 1);
            }

            Assert.Equal(dp.DefaultIfEmpty(0).Max(), LongestIncreasingSubsequence.Length(numbers));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => LongestIncreasingSubsequence.Length(null!));
    }

    [Fact]
    public void RunsInLinearithmicTime()
    {
        const int n = 200_000;
        int[] increasingWithNoise = Enumerable.Range(0, n).Select(i => i % 2 == 0 ? i : i - 3).ToArray();

        int actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => LongestIncreasingSubsequence.Length(increasingWithNoise),
            "Maintain the smallest tail for each subsequence length and binary search it.");

        // Values come in pairs (2k, 2k - 2); at most one value per pair fits in an increasing subsequence.
        Assert.Equal(n / 2, actual);
    }
}
