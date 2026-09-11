namespace DsaPractice.LinkedLists.Tests;

public class Exercise08_MyDoublyLinkedListTests
{
    [Fact]
    public void NewListIsEmpty()
    {
        var list = new MyDoublyLinkedList<int>();

        Assert.Equal(0, list.Count);
        Assert.Empty(list);
        Assert.Empty(list.Backwards());
    }

    [Fact]
    public void AddsAtBothEnds()
    {
        var list = new MyDoublyLinkedList<int>();

        list.AddLast(2);
        list.AddFirst(1);
        list.AddLast(3);

        Assert.Equal([1, 2, 3], list);
        Assert.Equal([3, 2, 1], list.Backwards());
        Assert.Equal(1, list.PeekFirst());
        Assert.Equal(3, list.PeekLast());
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void RemovesFromBothEnds()
    {
        var list = new MyDoublyLinkedList<string>();
        foreach (var s in new[] { "a", "b", "c", "d" }) list.AddLast(s);

        Assert.Equal("a", list.RemoveFirst());
        Assert.Equal("d", list.RemoveLast());

        Assert.Equal(["b", "c"], list);
        Assert.Equal(["c", "b"], list.Backwards());
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public void RemovingTheLastElementResetsBothEnds()
    {
        var list = new MyDoublyLinkedList<int>();
        list.AddFirst(1);

        Assert.Equal(1, list.RemoveLast());
        Assert.Empty(list);
        Assert.Empty(list.Backwards());

        list.AddLast(5);
        list.AddFirst(4);
        Assert.Equal([4, 5], list);
        Assert.Equal([5, 4], list.Backwards());
    }

    [Fact]
    public void EmptyListOperationsThrow()
    {
        var list = new MyDoublyLinkedList<int>();

        Assert.Throws<InvalidOperationException>(() => list.RemoveFirst());
        Assert.Throws<InvalidOperationException>(() => list.RemoveLast());
        Assert.Throws<InvalidOperationException>(() => list.PeekFirst());
        Assert.Throws<InvalidOperationException>(() => list.PeekLast());
    }

    [Fact]
    public void MatchesLinkedListOnRandomOperations()
    {
        var random = new Random(12);
        var list = new MyDoublyLinkedList<int>();
        var expected = new LinkedList<int>();

        for (int i = 0; i < 5_000; i++)
        {
            switch (random.Next(4))
            {
                case 0:
                    list.AddFirst(i);
                    expected.AddFirst(i);
                    break;
                case 1:
                    list.AddLast(i);
                    expected.AddLast(i);
                    break;
                case 2 when expected.Count > 0:
                    Assert.Equal(expected.First!.Value, list.RemoveFirst());
                    expected.RemoveFirst();
                    break;
                case 3 when expected.Count > 0:
                    Assert.Equal(expected.Last!.Value, list.RemoveLast());
                    expected.RemoveLast();
                    break;
            }

            Assert.Equal(expected.Count, list.Count);
        }

        Assert.Equal(expected, list);
        Assert.Equal(expected.Reverse(), list.Backwards());
    }

    [Fact]
    public void OperationsAtTheEndsAreConstantTime()
    {
        const int n = 300_000;
        var list = new MyDoublyLinkedList<int>();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < n; i++) list.AddLast(i);
            for (int i = n - 1; i >= 0; i--) Assert.Equal(i, list.RemoveLast());
        }, "RemoveLast should use the tail's Previous reference instead of walking the list.");

        Assert.Equal(0, list.Count);
    }
}
