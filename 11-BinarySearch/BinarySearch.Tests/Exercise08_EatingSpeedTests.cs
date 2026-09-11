namespace DsaPractice.BinarySearch.Tests;

public class Exercise08_EatingSpeedTests
{
    private static bool CanFinish(int[] piles, int hours, long speed) =>
        piles.Sum(p => (p + speed - 1) / speed) <= hours;

    [Theory]
    [InlineData(new[] { 3, 6, 7, 11 }, 8, 4)]
    [InlineData(new[] { 30, 11, 23, 4, 20 }, 5, 30)]
    [InlineData(new[] { 30, 11, 23, 4, 20 }, 6, 23)]
    [InlineData(new[] { 1 }, 1, 1)]
    [InlineData(new[] { 1, 1, 1 }, 10, 1)]
    [InlineData(new[] { 1_000_000_000 }, 2, 500_000_000)]
    [InlineData(new[] { 312_884_470 }, 312_884_469, 2)]
    [InlineData(new[] { 1_000_000_000, 1_000_000_000 }, 3, 1_000_000_000)]
    public void FindsMinimumSpeed(int[] piles, int hours, int expected)
    {
        Assert.Equal(expected, EatingSpeed.Minimum(piles, hours));
    }

    [Fact]
    public void ResultIsTheSmallestFeasibleSpeedOnRandomInputs()
    {
        var random = new Random(66);
        for (int round = 0; round < 100; round++)
        {
            int[] piles = Enumerable.Range(0, random.Next(1, 20)).Select(_ => random.Next(1, 1000)).ToArray();
            int hours = random.Next(piles.Length, piles.Length * 5 + 1);

            int speed = EatingSpeed.Minimum(piles, hours);

            Assert.True(CanFinish(piles, hours, speed), $"Speed {speed} is too slow.");
            Assert.True(speed == 1 || !CanFinish(piles, hours, speed - 1), $"Speed {speed - 1} would also work.");
        }
    }

    [Fact]
    public void ThrowsForInvalidInput()
    {
        Assert.Throws<ArgumentNullException>(() => EatingSpeed.Minimum(null!, 1));
        Assert.Throws<ArgumentException>(() => EatingSpeed.Minimum([], 1));
        Assert.Throws<ArgumentException>(() => EatingSpeed.Minimum([1, 2, 3], 2));
    }

    [Fact]
    public void RunsInLogarithmicNumberOfChecks()
    {
        var random = new Random(67);
        int[] piles = Enumerable.Range(0, 10_000).Select(_ => random.Next(1, 1_000_000_001)).ToArray();
        const int hours = 1_000_000;

        int speed = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => EatingSpeed.Minimum(piles, hours),
            "Binary search the speed between 1 and the largest pile.");

        Assert.True(CanFinish(piles, hours, speed));
        Assert.False(CanFinish(piles, hours, speed - 1));
    }
}
