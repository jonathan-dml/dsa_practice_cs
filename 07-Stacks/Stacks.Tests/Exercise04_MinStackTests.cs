namespace DsaPractice.Stacks.Tests;

public class Exercise04_MinStackTests
{
    [Fact]
    public void TracksMinimumAsValuesArePushedAndPopped()
    {
        var stack = new MinStack();

        stack.Push(-2);
        stack.Push(0);
        stack.Push(-3);

        Assert.Equal(-3, stack.GetMin());
        Assert.Equal(-3, stack.Pop());
        Assert.Equal(0, stack.Top());
        Assert.Equal(-2, stack.GetMin());
        Assert.Equal(2, stack.Count);
    }

    [Fact]
    public void HandlesDuplicateMinimums()
    {
        var stack = new MinStack();
        stack.Push(2);
        stack.Push(1);
        stack.Push(1);

        stack.Pop();
        Assert.Equal(1, stack.GetMin());

        stack.Pop();
        Assert.Equal(2, stack.GetMin());
    }

    [Fact]
    public void HandlesExtremeValues()
    {
        var stack = new MinStack();
        stack.Push(int.MaxValue);
        stack.Push(int.MinValue);

        Assert.Equal(int.MinValue, stack.GetMin());
        stack.Pop();
        Assert.Equal(int.MaxValue, stack.GetMin());
    }

    [Fact]
    public void EmptyStackOperationsThrow()
    {
        var stack = new MinStack();

        Assert.Throws<InvalidOperationException>(() => stack.Pop());
        Assert.Throws<InvalidOperationException>(() => stack.Top());
        Assert.Throws<InvalidOperationException>(() => stack.GetMin());
    }

    [Fact]
    public void MatchesBruteForceOnRandomOperations()
    {
        var random = new Random(21);
        var stack = new MinStack();
        var expected = new List<int>();

        for (int i = 0; i < 5000; i++)
        {
            if (expected.Count == 0 || random.Next(3) > 0)
            {
                int value = random.Next(-1000, 1000);
                stack.Push(value);
                expected.Add(value);
            }
            else
            {
                Assert.Equal(expected[^1], stack.Pop());
                expected.RemoveAt(expected.Count - 1);
            }

            if (expected.Count > 0)
            {
                Assert.Equal(expected.Min(), stack.GetMin());
                Assert.Equal(expected[^1], stack.Top());
            }
            Assert.Equal(expected.Count, stack.Count);
        }
    }

    [Fact]
    public void GetMinIsConstantTime()
    {
        const int n = 300_000;
        var stack = new MinStack();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = n; i > 0; i--)
            {
                stack.Push(i);
                Assert.Equal(i, stack.GetMin());
            }
            for (int i = 1; i <= n; i++)
            {
                Assert.Equal(i, stack.GetMin());
                stack.Pop();
            }
        }, "Store the minimum together with each pushed value.");
    }
}
