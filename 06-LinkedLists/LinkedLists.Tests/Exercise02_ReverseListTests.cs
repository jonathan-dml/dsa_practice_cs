namespace DsaPractice.LinkedLists.Tests;

public class Exercise02_ReverseListTests
{
    [Theory]
    [InlineData(new int[] { }, new int[] { })]
    [InlineData(new[] { 1 }, new[] { 1 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 5, 4, 3, 2, 1 })]
    [InlineData(new[] { 7, 7, 3 }, new[] { 3, 7, 7 })]
    public void ReversesLists(int[] values, int[] expected)
    {
        var reversed = ReverseList.Reverse(ListNode.FromValues(values));

        Assert.Equal(expected, ListNode.ToArray(reversed));
    }

    [Fact]
    public void ReusesTheOriginalNodes()
    {
        var head = ListNode.FromValues(1, 2, 3)!;
        var tail = head.Next!.Next!;

        var reversed = ReverseList.Reverse(head);

        Assert.Same(tail, reversed);
        Assert.Null(head.Next);
    }

    [Fact]
    public void ReversesLongListsInLinearTime()
    {
        const int n = 1_000_000;
        var head = ListNode.FromValues(Enumerable.Range(0, n).ToArray());

        var reversed = PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => ReverseList.Reverse(head));

        Assert.Equal(Enumerable.Range(0, n).Reverse(), ListNode.ToArray(reversed));
    }
}
