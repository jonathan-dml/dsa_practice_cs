namespace DsaPractice.LinkedLists.Tests;

public class Exercise06_RemoveNthFromEndTests
{
    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 2, new[] { 1, 2, 3, 5 })]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 1, new[] { 1, 2, 3, 4 })]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 5, new[] { 2, 3, 4, 5 })]
    [InlineData(new[] { 1 }, 1, new int[] { })]
    [InlineData(new[] { 1, 2 }, 1, new[] { 1 })]
    [InlineData(new[] { 1, 2 }, 2, new[] { 2 })]
    public void RemovesTheNode(int[] values, int n, int[] expected)
    {
        var head = RemoveNthFromEnd.Remove(ListNode.FromValues(values), n);

        Assert.Equal(expected, ListNode.ToArray(head));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 }, 0)]
    [InlineData(new[] { 1, 2, 3 }, -1)]
    [InlineData(new[] { 1, 2, 3 }, 4)]
    [InlineData(new int[] { }, 1)]
    public void ThrowsForInvalidN(int[] values, int n)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RemoveNthFromEnd.Remove(ListNode.FromValues(values), n));
    }
}
