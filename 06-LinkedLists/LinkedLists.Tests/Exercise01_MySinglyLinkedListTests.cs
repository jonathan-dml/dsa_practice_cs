namespace DsaPractice.LinkedLists.Tests;

public class Exercise01_MySinglyLinkedListTests
{
    [Fact]
    public void NewListIsEmpty()
    {
        var list = new MySinglyLinkedList<int>();

        Assert.Equal(0, list.Count);
        Assert.Empty(list);
    }

    [Fact]
    public void AddLastAppendsInOrder()
    {
        var list = new MySinglyLinkedList<int>();

        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);

        Assert.Equal([1, 2, 3], list);
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void AddFirstPrependsInOrder()
    {
        var list = new MySinglyLinkedList<int>();

        list.AddFirst(1);
        list.AddFirst(2);
        list.AddFirst(3);

        Assert.Equal([3, 2, 1], list);
    }

    [Fact]
    public void MixedAddsKeepHeadAndTailCorrect()
    {
        var list = new MySinglyLinkedList<string>();

        list.AddFirst("b");
        list.AddLast("c");
        list.AddFirst("a");
        list.AddLast("d");

        Assert.Equal(["a", "b", "c", "d"], list);
        Assert.Equal("a", list.PeekFirst());
        Assert.Equal("d", list.PeekLast());
    }

    [Fact]
    public void RemoveFirstReturnsValuesInOrder()
    {
        var list = new MySinglyLinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);

        Assert.Equal(1, list.RemoveFirst());
        Assert.Equal(1, list.Count);
        Assert.Equal(2, list.RemoveFirst());
        Assert.Equal(0, list.Count);
        Assert.Empty(list);
    }

    [Fact]
    public void TailIsResetWhenTheListBecomesEmpty()
    {
        var list = new MySinglyLinkedList<int>();
        list.AddLast(1);
        list.RemoveFirst();

        list.AddLast(2);
        list.AddLast(3);

        Assert.Equal([2, 3], list);
        Assert.Equal(2, list.PeekFirst());
        Assert.Equal(3, list.PeekLast());
    }

    [Fact]
    public void EmptyListOperationsThrow()
    {
        var list = new MySinglyLinkedList<int>();

        Assert.Throws<InvalidOperationException>(() => list.RemoveFirst());
        Assert.Throws<InvalidOperationException>(() => list.PeekFirst());
        Assert.Throws<InvalidOperationException>(() => list.PeekLast());
    }

    [Fact]
    public void ContainsFindsValues()
    {
        var list = new MySinglyLinkedList<string?>();
        list.AddLast("x");
        list.AddLast(null);

        Assert.True(list.Contains("x"));
        Assert.True(list.Contains(null));
        Assert.False(list.Contains("y"));
    }

    [Fact]
    public void OperationsAtTheEndsAreConstantTime()
    {
        const int n = 200_000;
        var list = new MySinglyLinkedList<int>();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < n; i++)
            {
                list.AddLast(i);
                _ = list.PeekLast();
            }
        }, "Keep a tail reference so AddLast and PeekLast don't walk the whole list.");

        Assert.Equal(n, list.Count);
        Assert.Equal(Enumerable.Range(0, n), list);
    }
}
