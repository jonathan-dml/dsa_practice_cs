namespace DsaPractice.TreesAndBST.Tests;

public class Exercise02_TreeDepthTests
{
    [Fact]
    public void EmptyTreeHasDepthZero()
    {
        Assert.Equal(0, TreeDepth.Max(null));
        Assert.Equal(0, TreeDepth.Min(null));
    }

    [Theory]
    [InlineData("1", 1, 1)]
    [InlineData("3,9,20,null,null,15,7", 3, 2)]
    [InlineData("1,null,2", 2, 2)]
    [InlineData("1,2", 2, 2)]
    [InlineData("2,null,3,null,4,null,5,null,6", 5, 5)]
    [InlineData("1,2,3,4,5,null,null,6", 4, 2)]
    [InlineData("1,2,3,4,5,6,7", 3, 3)]
    public void ComputesDepths(string levelOrder, int expectedMax, int expectedMin)
    {
        var root = TreeTestHelpers.Tree(levelOrder);

        Assert.Equal(expectedMax, TreeDepth.Max(root));
        Assert.Equal(expectedMin, TreeDepth.Min(root));
    }

    [Fact]
    public void MaxMatchesHeightOnRandomTrees()
    {
        var random = new Random(92);
        for (int round = 0; round < 30; round++)
        {
            var root = TreeTestHelpers.RandomBst(random, random.Next(1, 300));

            Assert.Equal(TreeTestHelpers.Height(root), TreeDepth.Max(root));
            Assert.InRange(TreeDepth.Min(root), 1, TreeDepth.Max(root));
        }
    }
}
