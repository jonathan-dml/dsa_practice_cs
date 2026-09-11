namespace DsaPractice.TreesAndBST.Tests;

public class Exercise01_TraversalsTests
{
    private static readonly TreeNode? Sample = TreeNode.FromLevelOrder(3, 9, 20, null, null, 15, 7);
    private static readonly TreeNode? RightLeaning = TreeNode.FromLevelOrder(1, null, 2, 3);
    private static readonly TreeNode? Bst = TreeNode.FromLevelOrder(8, 3, 10, 1, 6, null, 14, null, null, 4, 7, 13);

    [Fact]
    public void EmptyTreeGivesEmptyResults()
    {
        Assert.Empty(TreeTraversals.PreOrder(null));
        Assert.Empty(TreeTraversals.InOrder(null));
        Assert.Empty(TreeTraversals.PostOrder(null));
        Assert.Empty(TreeTraversals.LevelOrder(null));
    }

    [Fact]
    public void PreOrderVisitsNodeBeforeChildren()
    {
        Assert.Equal([3, 9, 20, 15, 7], TreeTraversals.PreOrder(Sample));
        Assert.Equal([1, 2, 3], TreeTraversals.PreOrder(RightLeaning));
        Assert.Equal([8, 3, 1, 6, 4, 7, 10, 14, 13], TreeTraversals.PreOrder(Bst));
    }

    [Fact]
    public void InOrderVisitsLeftNodeRight()
    {
        Assert.Equal([9, 3, 15, 20, 7], TreeTraversals.InOrder(Sample));
        Assert.Equal([1, 3, 2], TreeTraversals.InOrder(RightLeaning));
        Assert.Equal([1, 3, 4, 6, 7, 8, 10, 13, 14], TreeTraversals.InOrder(Bst));
    }

    [Fact]
    public void PostOrderVisitsChildrenBeforeNode()
    {
        Assert.Equal([9, 15, 7, 20, 3], TreeTraversals.PostOrder(Sample));
        Assert.Equal([3, 2, 1], TreeTraversals.PostOrder(RightLeaning));
        Assert.Equal([1, 4, 7, 6, 3, 13, 14, 10, 8], TreeTraversals.PostOrder(Bst));
    }

    [Fact]
    public void LevelOrderGroupsNodesByDepth()
    {
        AssertLevels([[3], [9, 20], [15, 7]], TreeTraversals.LevelOrder(Sample));
        AssertLevels([[1], [2], [3]], TreeTraversals.LevelOrder(RightLeaning));
        AssertLevels([[8], [3, 10], [1, 6, 14], [4, 7, 13]], TreeTraversals.LevelOrder(Bst));
    }

    [Fact]
    public void SingleNodeTree()
    {
        var root = new TreeNode(42);

        Assert.Equal([42], TreeTraversals.PreOrder(root));
        Assert.Equal([42], TreeTraversals.InOrder(root));
        Assert.Equal([42], TreeTraversals.PostOrder(root));
        AssertLevels([[42]], TreeTraversals.LevelOrder(root));
    }

    [Fact]
    public void InOrderOfRandomBstIsSorted()
    {
        var random = new Random(91);
        for (int round = 0; round < 20; round++)
        {
            var root = TreeTestHelpers.RandomBst(random, 200);
            var expected = new List<int>();
            TreeTestHelpers.CollectInOrder(root, expected);

            Assert.Equal(expected, TreeTraversals.InOrder(root));
            Assert.Equal(TreeTestHelpers.CountNodes(root), TreeTraversals.PreOrder(root).Count);
            Assert.Equal(TreeTestHelpers.Height(root), TreeTraversals.LevelOrder(root).Count);
        }
    }

    private static void AssertLevels(int[][] expected, IList<IList<int>> actual)
    {
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }
    }
}
