namespace DsaPractice.TreesAndBST.Tests;

public class Exercise07_KthSmallestTests
{
    [Theory]
    [InlineData("3,1,4,null,2", 1, 1)]
    [InlineData("3,1,4,null,2", 2, 2)]
    [InlineData("3,1,4,null,2", 4, 4)]
    [InlineData("5,3,6,2,4,null,null,1", 3, 3)]
    [InlineData("5,3,6,2,4,null,null,1", 6, 6)]
    [InlineData("7", 1, 7)]
    public void FindsKthSmallest(string levelOrder, int k, int expected)
    {
        Assert.Equal(expected, KthSmallest.InBst(TreeTestHelpers.Tree(levelOrder), k));
    }

    [Fact]
    public void MatchesSortedValuesOnRandomTrees()
    {
        var random = new Random(98);
        for (int round = 0; round < 20; round++)
        {
            var root = TreeTestHelpers.RandomBst(random, 150);
            var sorted = new List<int>();
            TreeTestHelpers.CollectInOrder(root, sorted);

            for (int k = 1; k <= sorted.Count; k++)
            {
                Assert.Equal(sorted[k - 1], KthSmallest.InBst(root, k));
            }
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(5)]
    public void ThrowsForInvalidK(int k)
    {
        var root = TreeTestHelpers.Tree("3,1,4,null,2");

        Assert.Throws<ArgumentOutOfRangeException>(() => KthSmallest.InBst(root, k));
    }

    [Fact]
    public void EmptyTreeThrows()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => KthSmallest.InBst(null, 1));
    }
}
