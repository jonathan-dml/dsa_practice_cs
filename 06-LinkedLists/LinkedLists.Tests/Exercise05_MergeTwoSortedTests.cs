namespace DsaPractice.LinkedLists.Tests;

public class Exercise05_MergeTwoSortedTests
{
    [Theory]
    [InlineData(new int[] { }, new int[] { }, new int[] { })]
    [InlineData(new int[] { }, new[] { 0 }, new[] { 0 })]
    [InlineData(new[] { 5 }, new int[] { }, new[] { 5 })]
    [InlineData(new[] { 1, 2, 4 }, new[] { 1, 3, 4 }, new[] { 1, 1, 2, 3, 4, 4 })]
    [InlineData(new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 1, 2, 3, 4, 5, 6 })]
    [InlineData(new[] { 4, 5, 6 }, new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4, 5, 6 })]
    [InlineData(new[] { -3, 0, 10 }, new[] { -5, 0, 7, 20 }, new[] { -5, -3, 0, 0, 7, 10, 20 })]
    public void MergesSortedLists(int[] first, int[] second, int[] expected)
    {
        var merged = SortedListMerger.Merge(ListNode.FromValues(first), ListNode.FromValues(second));

        Assert.Equal(expected, ListNode.ToArray(merged));
    }

    [Fact]
    public void ReusesTheExistingNodes()
    {
        var first = ListNode.FromValues(1, 3)!;
        var second = ListNode.FromValues(2)!;
        var nodes = new HashSet<ListNode>(ReferenceEqualityComparer.Instance) { first, first.Next!, second };

        var merged = SortedListMerger.Merge(first, second);

        for (var node = merged; node is not null; node = node.Next)
        {
            Assert.Contains(node, nodes);
        }
    }

    [Fact]
    public void MatchesSortOnRandomInputs()
    {
        var random = new Random(6);
        for (int round = 0; round < 50; round++)
        {
            int[] a = Enumerable.Range(0, random.Next(0, 30)).Select(_ => random.Next(-20, 20)).Order().ToArray();
            int[] b = Enumerable.Range(0, random.Next(0, 30)).Select(_ => random.Next(-20, 20)).Order().ToArray();

            var merged = SortedListMerger.Merge(ListNode.FromValues(a), ListNode.FromValues(b));

            Assert.Equal(a.Concat(b).Order(), ListNode.ToArray(merged));
        }
    }
}
