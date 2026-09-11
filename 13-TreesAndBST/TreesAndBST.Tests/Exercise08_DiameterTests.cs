namespace DsaPractice.TreesAndBST.Tests;

public class Exercise08_DiameterTests
{
    [Theory]
    [InlineData("", 0)]
    [InlineData("1", 0)]
    [InlineData("1,2", 1)]
    [InlineData("1,2,3", 2)]
    [InlineData("1,2,3,4,5", 3)]
    [InlineData("1,null,2,null,3,null,4", 3)]
    [InlineData("1,2,null,3,4,5,null,null,6,7,null,null,8", 6)]
    public void ComputesDiameter(string levelOrder, int expected)
    {
        Assert.Equal(expected, TreeDiameter.Compute(TreeTestHelpers.Tree(levelOrder)));
    }

    [Fact]
    public void MatchesBruteForceOnRandomTrees()
    {
        var random = new Random(99);
        for (int round = 0; round < 30; round++)
        {
            var root = TreeTestHelpers.RandomBst(random, random.Next(1, 200));

            Assert.Equal(BruteForce(root), TreeDiameter.Compute(root));
        }
    }

    private static int BruteForce(TreeNode? node) =>
        node is null
            ? 0
            : Math.Max(
                TreeTestHelpers.Height(node.Left) + TreeTestHelpers.Height(node.Right),
                Math.Max(BruteForce(node.Left), BruteForce(node.Right)));
}
