namespace DsaPractice.Stacks.Tests;

public class Exercise05_DailyTemperaturesTests
{
    [Theory]
    [InlineData(new int[] { }, new int[] { })]
    [InlineData(new[] { 50 }, new[] { 0 })]
    [InlineData(new[] { 73, 74, 75, 71, 69, 72, 76, 73 }, new[] { 1, 1, 4, 2, 1, 1, 0, 0 })]
    [InlineData(new[] { 30, 40, 50, 60 }, new[] { 1, 1, 1, 0 })]
    [InlineData(new[] { 30, 60, 90 }, new[] { 1, 1, 0 })]
    [InlineData(new[] { 90, 60, 30 }, new[] { 0, 0, 0 })]
    [InlineData(new[] { 50, 50, 51 }, new[] { 2, 1, 0 })]
    public void ComputesWaitingDays(int[] temperatures, int[] expected)
    {
        Assert.Equal(expected, DailyTemperatures.DaysUntilWarmer(temperatures));
    }

    [Fact]
    public void MatchesBruteForceOnRandomInputs()
    {
        var random = new Random(15);
        for (int round = 0; round < 30; round++)
        {
            int[] temps = Enumerable.Range(0, random.Next(0, 200)).Select(_ => random.Next(30, 60)).ToArray();
            int[] expected = new int[temps.Length];
            for (int i = 0; i < temps.Length; i++)
                for (int j = i + 1; j < temps.Length; j++)
                    if (temps[j] > temps[i]) { expected[i] = j - i; break; }

            Assert.Equal(expected, DailyTemperatures.DaysUntilWarmer(temps));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => DailyTemperatures.DaysUntilWarmer(null!));
    }

    [Fact]
    public void RunsInLinearTime()
    {
        const int n = 300_000;
        int[] temps = Enumerable.Range(0, n - 1).Select(i => n - i).Append(n + 1).ToArray();
        int[] expected = Enumerable.Range(0, n).Select(i => i == n - 1 ? 0 : n - 1 - i).ToArray();

        int[] actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => DailyTemperatures.DaysUntilWarmer(temps),
            "Keep a stack of indices still waiting for a warmer day.");

        Assert.Equal(expected, actual);
    }
}
