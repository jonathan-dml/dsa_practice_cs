namespace DsaPractice.TreesAndBST.Tests;

public class Exercise05_MyBinarySearchTreeTests
{
    private static MyBinarySearchTree TreeOf(params int[] values)
    {
        var tree = new MyBinarySearchTree();
        foreach (int value in values) tree.Insert(value);
        return tree;
    }

    [Fact]
    public void NewTreeIsEmpty()
    {
        var tree = new MyBinarySearchTree();

        Assert.Null(tree.Root);
        Assert.Equal(0, tree.Count);
        Assert.Empty(tree.InOrder());
        Assert.False(tree.Contains(1));
    }

    [Fact]
    public void InsertBuildsTheExpectedShape()
    {
        var tree = TreeOf(5, 3, 8, 1, 4, 7, 9);

        Assert.Equal([5, 3, 8, 1, 4, 7, 9], TreeNode.ToLevelOrder(tree.Root));
        Assert.Equal(7, tree.Count);
    }

    [Fact]
    public void InsertRejectsDuplicates()
    {
        var tree = TreeOf(2, 1);

        Assert.False(tree.Insert(2));
        Assert.True(tree.Insert(3));
        Assert.Equal(3, tree.Count);
    }

    [Fact]
    public void ContainsSearchesTheTree()
    {
        var tree = TreeOf(50, 30, 70, 20, 40, 60, 80);

        Assert.True(tree.Contains(60));
        Assert.True(tree.Contains(20));
        Assert.False(tree.Contains(65));
        Assert.False(tree.Contains(10));
    }

    [Fact]
    public void MinAndMax()
    {
        var tree = TreeOf(50, 30, 70, 20, 40, 60, 80, -5);

        Assert.Equal(-5, tree.Min());
        Assert.Equal(80, tree.Max());
    }

    [Fact]
    public void MinAndMaxThrowWhenEmpty()
    {
        var tree = new MyBinarySearchTree();

        Assert.Throws<InvalidOperationException>(() => tree.Min());
        Assert.Throws<InvalidOperationException>(() => tree.Max());
    }

    [Fact]
    public void InOrderYieldsSortedValues()
    {
        var tree = TreeOf(5, 3, 8, 1, 4, 7, 9);

        Assert.Equal([1, 3, 4, 5, 7, 8, 9], tree.InOrder());
    }

    [Fact]
    public void RemoveLeaf()
    {
        var tree = TreeOf(5, 3, 8, 1, 4, 7, 9);

        Assert.True(tree.Remove(1));

        Assert.Equal([5, 3, 8, null, 4, 7, 9], TreeNode.ToLevelOrder(tree.Root));
        Assert.Equal(6, tree.Count);
    }

    [Fact]
    public void RemoveNodeWithOneChild()
    {
        var tree = TreeOf(5, 3, 8, 4);

        Assert.True(tree.Remove(3));

        Assert.Equal([5, 4, 8], TreeNode.ToLevelOrder(tree.Root));
    }

    [Fact]
    public void RemoveNodeWithTwoChildrenUsesInOrderSuccessor()
    {
        var tree = TreeOf(5, 3, 8, 1, 4, 7, 9);

        Assert.True(tree.Remove(8));
        Assert.Equal([5, 3, 9, 1, 4, 7], TreeNode.ToLevelOrder(tree.Root));

        Assert.True(tree.Remove(5));
        Assert.Equal([7, 3, 9, 1, 4], TreeNode.ToLevelOrder(tree.Root));
    }

    [Fact]
    public void RemoveRootWithTwoChildren()
    {
        var tree = TreeOf(5, 3, 8, 1, 4, 7, 9);

        Assert.True(tree.Remove(5));

        Assert.Equal([7, 3, 8, 1, 4, null, 9], TreeNode.ToLevelOrder(tree.Root));
    }

    [Fact]
    public void RemoveMissingValueReturnsFalse()
    {
        var tree = TreeOf(5, 3, 8);

        Assert.False(tree.Remove(42));

        Assert.Equal([5, 3, 8], TreeNode.ToLevelOrder(tree.Root));
        Assert.Equal(3, tree.Count);
    }

    [Fact]
    public void RemovingTheOnlyNodeEmptiesTheTree()
    {
        var tree = TreeOf(1);

        Assert.True(tree.Remove(1));

        Assert.Null(tree.Root);
        Assert.Equal(0, tree.Count);
    }

    [Fact]
    public void MatchesSortedSetOnRandomOperations()
    {
        var random = new Random(95);
        var tree = new MyBinarySearchTree();
        var expected = new SortedSet<int>();

        for (int i = 0; i < 5000; i++)
        {
            int value = random.Next(300);
            switch (random.Next(3))
            {
                case 0:
                    Assert.Equal(expected.Add(value), tree.Insert(value));
                    break;
                case 1:
                    Assert.Equal(expected.Remove(value), tree.Remove(value));
                    break;
                default:
                    Assert.Equal(expected.Contains(value), tree.Contains(value));
                    break;
            }

            Assert.Equal(expected.Count, tree.Count);
        }

        Assert.Equal(expected, tree.InOrder());
        var inOrderFromNodes = new List<int>();
        TreeTestHelpers.CollectInOrder(tree.Root, inOrderFromNodes);
        Assert.Equal(expected, inOrderFromNodes);
    }

    [Fact]
    public void OperationsAreFastOnRandomInsertionOrder()
    {
        const int n = 200_000;
        int[] values = Enumerable.Range(0, n).ToArray();
        new Random(96).Shuffle(values);
        var tree = new MyBinarySearchTree();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            foreach (int v in values) tree.Insert(v);
            foreach (int v in values) if (!tree.Contains(v)) Assert.Fail($"Missing {v}.");
            for (int v = 0; v < n; v += 2) tree.Remove(v);
        }, "Each operation should walk a single root-to-leaf path.");

        Assert.Equal(n / 2, tree.Count);
        Assert.Equal(1, tree.Min());
        Assert.Equal(n - 1, tree.Max());
    }
}
