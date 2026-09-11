namespace DsaPractice.Stacks.Tests;

public class Exercise01_MyStackTests
{
    [Fact]
    public void NewStackIsEmpty()
    {
        var stack = new MyStack<int>();

        Assert.Equal(0, stack.Count);
        Assert.True(stack.IsEmpty);
    }

    [Fact]
    public void PopReturnsItemsInReverseOrder()
    {
        var stack = new MyStack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        Assert.Equal(3, stack.Pop());
        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
        Assert.True(stack.IsEmpty);
    }

    [Fact]
    public void PeekDoesNotRemove()
    {
        var stack = new MyStack<string>();
        stack.Push("a");
        stack.Push("b");

        Assert.Equal("b", stack.Peek());
        Assert.Equal("b", stack.Peek());
        Assert.Equal(2, stack.Count);
        Assert.False(stack.IsEmpty);
    }

    [Fact]
    public void EmptyStackOperationsThrow()
    {
        var stack = new MyStack<int>();

        Assert.Throws<InvalidOperationException>(() => stack.Pop());
        Assert.Throws<InvalidOperationException>(() => stack.Peek());

        stack.Push(1);
        stack.Pop();
        Assert.Throws<InvalidOperationException>(() => stack.Pop());
    }

    [Fact]
    public void GrowsBeyondInitialCapacity()
    {
        var stack = new MyStack<int>();

        for (int i = 0; i < 1000; i++) stack.Push(i);

        Assert.Equal(1000, stack.Count);
        for (int i = 999; i >= 0; i--) Assert.Equal(i, stack.Pop());
    }

    [Fact]
    public void InterleavedPushAndPop()
    {
        var stack = new MyStack<int>();

        stack.Push(1);
        stack.Push(2);
        Assert.Equal(2, stack.Pop());
        stack.Push(3);
        stack.Push(4);
        Assert.Equal(4, stack.Pop());
        Assert.Equal(3, stack.Pop());
        Assert.Equal(1, stack.Peek());
        Assert.Equal(1, stack.Count);
    }

    [Fact]
    public void StoresNull()
    {
        var stack = new MyStack<string?>();

        stack.Push(null);

        Assert.Null(stack.Peek());
        Assert.Equal(1, stack.Count);
    }

    [Fact]
    public void OperationsAreAmortizedConstantTime()
    {
        const int n = 2_000_000;
        var stack = new MyStack<int>();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < n; i++) stack.Push(i);
            long sum = 0;
            while (!stack.IsEmpty) sum += stack.Pop();
            Assert.Equal((long)n * (n - 1) / 2, sum);
        }, "Double the array when it's full; never shift elements.");
    }
}
