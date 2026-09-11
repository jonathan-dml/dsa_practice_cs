namespace DsaPractice.LinkedLists.Tests;

public class Exercise04_HasCycleTests
{
    // Every call runs under a time limit, so a solution that loops forever on a cycle fails instead of hanging.
    private static bool HasCycle(ListNode? head) =>
        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => CycleDetection.HasCycle(head),
            "On a cyclic list, walking until null never ends. Use slow and fast pointers.");

    /// <summary>Builds a list and connects the tail to the node at <paramref name="cycleTo"/> (-1 = no cycle).</summary>
    private static ListNode? Build(int length, int cycleTo)
    {
        var head = ListNode.FromValues(Enumerable.Range(0, length).ToArray());
        if (cycleTo < 0 || head is null) return head;

        ListNode target = head, tail = head;
        for (int i = 0; tail.Next is not null; i++)
        {
            if (i + 1 == cycleTo) target = tail.Next;
            tail = tail.Next;
        }
        tail.Next = target;
        return head;
    }

    [Fact]
    public void EmptyListHasNoCycle()
    {
        Assert.False(HasCycle(null));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(10)]
    public void ListsWithoutCyclesReturnFalse(int length)
    {
        Assert.False(HasCycle(Build(length, -1)));
    }

    [Fact]
    public void SingleNodePointingToItselfIsACycle()
    {
        var node = new ListNode(1);
        node.Next = node;

        Assert.True(HasCycle(node));
    }

    [Theory]
    [InlineData(2, 0)]
    [InlineData(4, 1)]
    [InlineData(5, 4)]
    [InlineData(10, 0)]
    [InlineData(10, 7)]
    public void DetectsCycles(int length, int cycleTo)
    {
        Assert.True(HasCycle(Build(length, cycleTo)));
    }

    [Fact]
    public void HandlesLongLists()
    {
        Assert.True(HasCycle(Build(500_000, 250_000)));
        Assert.False(HasCycle(Build(500_000, -1)));
    }
}
