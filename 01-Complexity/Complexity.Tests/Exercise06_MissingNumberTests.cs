namespace DsaPractice.Complexity.Tests;

public class Exercise06_MissingNumberTests
{
    [Theory]
    [InlineData(new int[] { }, 0)]
    [InlineData(new[] { 0 }, 1)]
    [InlineData(new[] { 1 }, 0)]
    [InlineData(new[] { 3, 0, 1 }, 2)]
    [InlineData(new[] { 0, 1 }, 2)]
    [InlineData(new[] { 9, 6, 4, 2, 3, 5, 7, 0, 1 }, 8)]
    public void FindsTheMissingNumber(int[] numbers, int expected)
    {
        Assert.Equal(expected, MissingNumber.Find(numbers));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => MissingNumber.Find(null!));
    }

    [Fact]
    public void HandlesLargeInputsWithoutOverflow()
    {
        const int n = 1_000_000;
        var random = new Random(5);
        int missing = random.Next(0, n + 1);
        int[] numbers = Enumerable.Range(0, n + 1).Where(x => x != missing).ToArray();
        random.Shuffle(numbers);

        int actual = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () => MissingNumber.Find(numbers));

        Assert.Equal(missing, actual);
    }
}
