namespace DsaPractice.TreesAndBST.Tests;

public class Exercise03_TreeMirrorTests
{
    [Theory]
    [InlineData("4,2,7,1,3,6,9", "4,7,2,9,6,3,1")]
    [InlineData("2,1,3", "2,3,1")]
    [InlineData("1,2", "1,null,2")]
    [InlineData("1", "1")]
    [InlineData("1,2,null,3,null,4", "1,null,2,null,3,null,4")]
    public void InvertsTrees(string input, string expected)
    {
        var root = TreeTestHelpers.Tree(input);

        var inverted = TreeMirror.Invert(root);

        Assert.Same(root, inverted);
        Assert.Equal(TreeTestHelpers.Values(expected), TreeNode.ToLevelOrder(inverted));
    }

    [Fact]
    public void InvertingNullReturnsNull()
    {
        Assert.Null(TreeMirror.Invert(null));
    }

    [Fact]
    public void InvertingTwiceRestoresTheTree()
    {
        var random = new Random(93);
        var root = TreeTestHelpers.RandomBst(random, 100);
        var original = TreeNode.ToLevelOrder(root);

        TreeMirror.Invert(TreeMirror.Invert(root));

        Assert.Equal(original, TreeNode.ToLevelOrder(root));
    }

    [Theory]
    [InlineData("", true)]
    [InlineData("1", true)]
    [InlineData("1,2,2", true)]
    [InlineData("1,2,2,3,4,4,3", true)]
    [InlineData("1,2,2,null,3,3", true)]
    [InlineData("1,2,2,null,3,null,3", false)]
    [InlineData("1,2,3", false)]
    [InlineData("1,2", false)]
    [InlineData("1,2,2,3,4,3,4", false)]
    public void ChecksSymmetry(string input, bool expected)
    {
        Assert.Equal(expected, TreeMirror.IsSymmetric(TreeTestHelpers.Tree(input)));
    }
}
