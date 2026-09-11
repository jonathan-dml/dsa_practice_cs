namespace DsaPractice.QueuesAndDeques.Tests;

public class Exercise03_MyDequeTests
{
    [Fact]
    public void NewDequeIsEmpty()
    {
        Assert.Equal(0, new MyDeque<int>().Count);
    }

    [Fact]
    public void AddsAndRemovesAtBothEnds()
    {
        var deque = new MyDeque<int>();

        deque.AddLast(2);
        deque.AddFirst(1);
        deque.AddLast(3);
        deque.AddFirst(0);

        Assert.Equal(4, deque.Count);
        Assert.Equal(0, deque.PeekFirst());
        Assert.Equal(3, deque.PeekLast());
        Assert.Equal(0, deque.RemoveFirst());
        Assert.Equal(3, deque.RemoveLast());
        Assert.Equal(1, deque.RemoveFirst());
        Assert.Equal(2, deque.RemoveLast());
        Assert.Equal(0, deque.Count);
    }

    [Fact]
    public void CanBeUsedAsAStack()
    {
        var deque = new MyDeque<string>();
        foreach (var s in new[] { "a", "b", "c", "d", "e", "f" }) deque.AddFirst(s);

        Assert.Equal("f", deque.RemoveFirst());
        Assert.Equal("e", deque.RemoveFirst());
        Assert.Equal("a", deque.RemoveLast());
    }

    [Fact]
    public void EmptyDequeOperationsThrow()
    {
        var deque = new MyDeque<int>();

        Assert.Throws<InvalidOperationException>(() => deque.RemoveFirst());
        Assert.Throws<InvalidOperationException>(() => deque.RemoveLast());
        Assert.Throws<InvalidOperationException>(() => deque.PeekFirst());
        Assert.Throws<InvalidOperationException>(() => deque.PeekLast());
    }

    [Fact]
    public void MatchesLinkedListOnRandomOperations()
    {
        var random = new Random(33);
        var deque = new MyDeque<int>();
        var expected = new LinkedList<int>();

        for (int i = 0; i < 20_000; i++)
        {
            switch (random.Next(4))
            {
                case 0:
                    deque.AddFirst(i);
                    expected.AddFirst(i);
                    break;
                case 1:
                    deque.AddLast(i);
                    expected.AddLast(i);
                    break;
                case 2 when expected.Count > 0:
                    Assert.Equal(expected.First!.Value, deque.RemoveFirst());
                    expected.RemoveFirst();
                    break;
                case 3 when expected.Count > 0:
                    Assert.Equal(expected.Last!.Value, deque.RemoveLast());
                    expected.RemoveLast();
                    break;
            }

            Assert.Equal(expected.Count, deque.Count);
            if (expected.Count > 0)
            {
                Assert.Equal(expected.First!.Value, deque.PeekFirst());
                Assert.Equal(expected.Last!.Value, deque.PeekLast());
            }
        }
    }

    [Fact]
    public void AddFirstIsAmortizedConstantTime()
    {
        const int n = 500_000;
        var deque = new MyDeque<int>();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(1), () =>
        {
            for (int i = 0; i < n; i++) deque.AddFirst(i);
            for (int i = 0; i < n; i++) Assert.Equal(i, deque.RemoveLast());
        }, "Move the head index backwards with wrap-around instead of shifting elements.");
    }
}
