namespace DsaPractice.LinkedLists.Tests;

public class Exercise03_MiddleNodeTests
{
    [Fact]
    public void EmptyListHasNoMiddle()
    {
        Assert.Null(MiddleNode.Find(null));
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 2)]
    [InlineData(6, 3)]
    [InlineData(101, 50)]
    public void ReturnsTheMiddleNodeInstance(int length, int expectedIndex)
    {
        var head = ListNode.FromValues(Enumerable.Range(0, length).ToArray());
        var expected = head;
        for (int i = 0; i < expectedIndex; i++) expected = expected!.Next;

        var middle = MiddleNode.Find(head);

        Assert.Same(expected, middle);
    }

    [Fact]
    public void DoesNotModifyTheList()
    {
        var head = ListNode.FromValues(1, 2, 3, 4, 5);

        MiddleNode.Find(head);

        Assert.Equal([1, 2, 3, 4, 5], ListNode.ToArray(head));
    }
}
