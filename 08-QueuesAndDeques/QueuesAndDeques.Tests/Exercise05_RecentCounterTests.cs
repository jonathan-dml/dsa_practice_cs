namespace DsaPractice.QueuesAndDeques.Tests;

public class Exercise05_RecentCounterTests
{
    [Fact]
    public void CountsPingsInTheLastThreeSeconds()
    {
        var counter = new RecentCounter();

        Assert.Equal(1, counter.Ping(1));
        Assert.Equal(2, counter.Ping(100));
        Assert.Equal(3, counter.Ping(3001));
        Assert.Equal(3, counter.Ping(3002));
    }

    [Fact]
    public void WindowIsInclusive()
    {
        var counter = new RecentCounter();

        counter.Ping(1000);

        Assert.Equal(2, counter.Ping(4000)); // window [1000, 4000] still includes 1000
        Assert.Equal(2, counter.Ping(4001)); // window [1001, 4001] no longer does
    }

    [Fact]
    public void OldPingsExpireAfterLongGaps()
    {
        var counter = new RecentCounter();
        counter.Ping(1);
        counter.Ping(2);
        counter.Ping(3);

        Assert.Equal(1, counter.Ping(10_000));
        Assert.Equal(2, counter.Ping(13_000));
        Assert.Equal(2, counter.Ping(16_000));
    }

    [Theory]
    [InlineData(100)]
    [InlineData(99)]
    public void ThrowsWhenTimestampsDoNotIncrease(int next)
    {
        var counter = new RecentCounter();
        counter.Ping(100);

        Assert.Throws<ArgumentException>(() => counter.Ping(next));
    }

    [Fact]
    public void RunsInAmortizedConstantTime()
    {
        const int n = 1_000_000;
        var counter = new RecentCounter();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int t = 0; t < n; t++)
            {
                int count = counter.Ping(t);
                if (count != Math.Min(t + 1, 3001)) Assert.Fail($"Ping({t}) returned {count}.");
            }
        }, "Remove expired timestamps from the front of a queue instead of counting all pings.");
    }
}
