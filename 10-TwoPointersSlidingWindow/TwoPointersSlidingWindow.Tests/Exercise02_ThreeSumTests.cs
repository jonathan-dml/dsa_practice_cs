namespace DsaPractice.TwoPointersSlidingWindow.Tests;

public class Exercise02_ThreeSumTests
{
    private static List<(int, int, int)> Sorted(IEnumerable<(int A, int B, int C)> triplets) => triplets.Order().ToList();

    private static List<(int, int, int)> BruteForce(int[] numbers)
    {
        var set = new HashSet<(int, int, int)>();
        for (int i = 0; i < numbers.Length; i++)
            for (int j = i + 1; j < numbers.Length; j++)
                for (int k = j + 1; k < numbers.Length; k++)
                    if (numbers[i] + numbers[j] + numbers[k] == 0)
                    {
                        int[] t = [numbers[i], numbers[j], numbers[k]];
                        Array.Sort(t);
                        set.Add((t[0], t[1], t[2]));
                    }
        return set.Order().ToList();
    }

    [Fact]
    public void FindsUniqueTriplets()
    {
        var result = ThreeSum.FindTriplets([-1, 0, 1, 2, -1, -4]);

        Assert.Equal([(-1, -1, 2), (-1, 0, 1)], Sorted(result));
    }

    [Theory]
    [InlineData(new int[] { })]
    [InlineData(new[] { 0 })]
    [InlineData(new[] { 0, 0 })]
    [InlineData(new[] { 0, 1, 1 })]
    [InlineData(new[] { 1, 2, -2, -1 })]
    public void ReturnsEmptyWhenNoTripletExists(int[] numbers)
    {
        Assert.Empty(ThreeSum.FindTriplets(numbers));
    }

    [Fact]
    public void DoesNotRepeatTriplets()
    {
        Assert.Equal([(0, 0, 0)], Sorted(ThreeSum.FindTriplets([0, 0, 0, 0, 0])));
        Assert.Equal([(-2, 1, 1)], Sorted(ThreeSum.FindTriplets([-2, 1, 1, 1, -2])));
    }

    [Fact]
    public void DoesNotModifyTheInput()
    {
        int[] numbers = [3, -1, -2, 0];

        ThreeSum.FindTriplets(numbers);

        Assert.Equal([3, -1, -2, 0], numbers);
    }

    [Fact]
    public void MatchesBruteForceOnRandomInputs()
    {
        var random = new Random(51);
        for (int round = 0; round < 40; round++)
        {
            int[] numbers = Enumerable.Range(0, random.Next(0, 40)).Select(_ => random.Next(-10, 11)).ToArray();

            Assert.Equal(BruteForce(numbers), Sorted(ThreeSum.FindTriplets(numbers)));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => ThreeSum.FindTriplets(null!));
    }

    [Fact]
    public void RunsInQuadraticTime()
    {
        var random = new Random(52);
        int[] numbers = Enumerable.Range(0, 3000).Select(_ => random.Next(-100_000, 100_001)).ToArray();

        var result = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => ThreeSum.FindTriplets(numbers),
            "Sort a copy, fix one element and use two pointers for the other two.");

        Assert.All(result, t =>
        {
            Assert.Equal(0, t.A + t.B + t.C);
            Assert.True(t.A <= t.B && t.B <= t.C);
        });
        Assert.Equal(result.Count, result.Distinct().Count());
    }
}
