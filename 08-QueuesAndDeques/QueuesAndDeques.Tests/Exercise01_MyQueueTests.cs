namespace DsaPractice.QueuesAndDeques.Tests;

public class Exercise01_MyQueueTests
{
    [Fact]
    public void NewQueueIsEmpty()
    {
        var queue = new MyQueue<int>();

        Assert.Equal(0, queue.Count);
        Assert.True(queue.IsEmpty);
    }

    [Fact]
    public void DequeueReturnsItemsInArrivalOrder()
    {
        var queue = new MyQueue<string>();
        queue.Enqueue("a");
        queue.Enqueue("b");
        queue.Enqueue("c");

        Assert.Equal("a", queue.Dequeue());
        Assert.Equal("b", queue.Dequeue());
        Assert.Equal("c", queue.Dequeue());
        Assert.True(queue.IsEmpty);
    }

    [Fact]
    public void PeekDoesNotRemove()
    {
        var queue = new MyQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);

        Assert.Equal(1, queue.Peek());
        Assert.Equal(1, queue.Peek());
        Assert.Equal(2, queue.Count);
    }

    [Fact]
    public void EmptyQueueOperationsThrow()
    {
        var queue = new MyQueue<int>();

        Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        Assert.Throws<InvalidOperationException>(() => queue.Peek());

        queue.Enqueue(1);
        queue.Dequeue();
        Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
    }

    [Fact]
    public void WrapsAroundAndGrowsInOrder()
    {
        var queue = new MyQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());

        for (int i = 4; i <= 10; i++) queue.Enqueue(i);

        for (int i = 3; i <= 10; i++) Assert.Equal(i, queue.Dequeue());
        Assert.True(queue.IsEmpty);
    }

    [Fact]
    public void MatchesQueueOnRandomOperations()
    {
        var random = new Random(31);
        var queue = new MyQueue<int>();
        var expected = new Queue<int>();

        for (int i = 0; i < 20_000; i++)
        {
            if (expected.Count == 0 || random.Next(5) < 3)
            {
                queue.Enqueue(i);
                expected.Enqueue(i);
            }
            else
            {
                Assert.Equal(expected.Dequeue(), queue.Dequeue());
            }

            Assert.Equal(expected.Count, queue.Count);
            if (expected.Count > 0) Assert.Equal(expected.Peek(), queue.Peek());
        }
    }

    [Fact]
    public void DequeueDoesNotShiftElements()
    {
        const int size = 100_000;
        var queue = new MyQueue<int>();
        for (int i = 0; i < size; i++) queue.Enqueue(i);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < 1_000_000; i++) queue.Enqueue(queue.Dequeue());
        }, "Advance a head index (with wrap-around) instead of moving elements.");

        Assert.Equal(size, queue.Count);
        Assert.Equal(1_000_000 % size, queue.Peek());
    }
}
