namespace DsaPractice.Heaps.Tests;

public class Exercise01_MyMinHeapTests
{
    [Fact]
    public void NewHeapIsEmpty()
    {
        Assert.Equal(0, new MyMinHeap<int>().Count);
    }

    [Fact]
    public void PollReturnsItemsInAscendingOrder()
    {
        var heap = new MyMinHeap<int>();
        foreach (int value in new[] { 5, 3, 8, 1, 9, 2 }) heap.Add(value);

        var polled = new List<int>();
        while (heap.Count > 0) polled.Add(heap.Poll());

        Assert.Equal([1, 2, 3, 5, 8, 9], polled);
    }

    [Fact]
    public void PeekReturnsMinimumWithoutRemoving()
    {
        var heap = new MyMinHeap<int>();
        heap.Add(4);
        heap.Add(-2);
        heap.Add(7);

        Assert.Equal(-2, heap.Peek());
        Assert.Equal(-2, heap.Peek());
        Assert.Equal(3, heap.Count);
    }

    [Fact]
    public void HandlesDuplicates()
    {
        var heap = new MyMinHeap<int>();
        foreach (int value in new[] { 2, 2, 1, 1, 3, 1 }) heap.Add(value);

        Assert.Equal(1, heap.Poll());
        Assert.Equal(1, heap.Poll());
        Assert.Equal(1, heap.Poll());
        Assert.Equal(2, heap.Poll());
        Assert.Equal(2, heap.Poll());
        Assert.Equal(3, heap.Poll());
    }

    [Fact]
    public void EmptyHeapOperationsThrow()
    {
        var heap = new MyMinHeap<string>();

        Assert.Throws<InvalidOperationException>(() => heap.Peek());
        Assert.Throws<InvalidOperationException>(() => heap.Poll());
    }

    [Fact]
    public void ReversedComparerMakesAMaxHeap()
    {
        var heap = new MyMinHeap<int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));
        foreach (int value in new[] { 5, 3, 8, 1, 9, 2 }) heap.Add(value);

        Assert.Equal(9, heap.Poll());
        Assert.Equal(8, heap.Poll());
        Assert.Equal(5, heap.Peek());
    }

    [Fact]
    public void WorksWithCustomComparers()
    {
        var heap = new MyMinHeap<string>(StringComparer.Ordinal);
        foreach (var word in new[] { "pear", "apple", "Zebra", "fig" }) heap.Add(word);

        Assert.Equal("Zebra", heap.Poll());
        Assert.Equal("apple", heap.Poll());
        Assert.Equal("fig", heap.Poll());
        Assert.Equal("pear", heap.Poll());
    }

    [Fact]
    public void MatchesPriorityQueueOnRandomOperations()
    {
        var random = new Random(101);
        var heap = new MyMinHeap<int>();
        var expected = new PriorityQueue<int, int>();

        for (int i = 0; i < 20_000; i++)
        {
            if (expected.Count == 0 || random.Next(3) > 0)
            {
                int value = random.Next(-1000, 1000);
                heap.Add(value);
                expected.Enqueue(value, value);
            }
            else
            {
                Assert.Equal(expected.Dequeue(), heap.Poll());
            }

            Assert.Equal(expected.Count, heap.Count);
            if (expected.Count > 0) Assert.Equal(expected.Peek(), heap.Peek());
        }
    }

    [Fact]
    public void AddAndPollAreLogarithmic()
    {
        const int n = 500_000;
        var random = new Random(102);
        int[] values = Enumerable.Range(0, n).Select(_ => random.Next()).ToArray();
        int[] sorted = values.Order().ToArray();
        var heap = new MyMinHeap<int>();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            foreach (int v in values) heap.Add(v);
            for (int i = 0; i < n; i++)
            {
                if (heap.Poll() != sorted[i]) Assert.Fail($"Wrong value at position {i}.");
            }
        }, "Sift up after adding and sift down after polling; don't keep the whole array sorted.");
    }
}
