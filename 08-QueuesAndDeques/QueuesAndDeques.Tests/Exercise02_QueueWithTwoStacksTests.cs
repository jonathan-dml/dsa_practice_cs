namespace DsaPractice.QueuesAndDeques.Tests;

public class Exercise02_QueueWithTwoStacksTests
{
    [Fact]
    public void BehavesLikeAQueue()
    {
        var queue = new QueueWithTwoStacks<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        Assert.Equal(1, queue.Peek());
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(1, queue.Count);
    }

    [Fact]
    public void KeepsOrderWhenEnqueueingBetweenDequeues()
    {
        var queue = new QueueWithTwoStacks<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        Assert.Equal(1, queue.Dequeue());

        queue.Enqueue(3);
        queue.Enqueue(4);

        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        queue.Enqueue(5);
        Assert.Equal(4, queue.Dequeue());
        Assert.Equal(5, queue.Dequeue());
        Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void EmptyQueueOperationsThrow()
    {
        var queue = new QueueWithTwoStacks<string>();

        Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        Assert.Throws<InvalidOperationException>(() => queue.Peek());
    }

    [Fact]
    public void MatchesQueueOnRandomOperations()
    {
        var random = new Random(32);
        var queue = new QueueWithTwoStacks<int>();
        var expected = new Queue<int>();

        for (int i = 0; i < 20_000; i++)
        {
            if (expected.Count == 0 || random.Next(2) == 0)
            {
                queue.Enqueue(i);
                expected.Enqueue(i);
            }
            else
            {
                Assert.Equal(expected.Peek(), queue.Peek());
                Assert.Equal(expected.Dequeue(), queue.Dequeue());
            }

            Assert.Equal(expected.Count, queue.Count);
        }
    }

    [Fact]
    public void OperationsAreAmortizedConstantTime()
    {
        const int size = 100_000;
        var queue = new QueueWithTwoStacks<int>();
        for (int i = 0; i < size; i++) queue.Enqueue(i);

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < 500_000; i++) queue.Enqueue(queue.Dequeue());
        }, "Only move items from the inbox to the outbox when the outbox is empty.");

        Assert.Equal(size, queue.Count);
        Assert.Equal(500_000 % size, queue.Peek());
    }
}
