namespace DsaPractice.TreesAndBST.Tests;

public class Exercise04_ValidateBstTests
{
    [Theory]
    [InlineData("", true)]
    [InlineData("1", true)]
    [InlineData("2,1,3", true)]
    [InlineData("8,3,10,1,6,null,14,null,null,4,7,13", true)]
    [InlineData("0,-1", true)]
    [InlineData("5,1,4,null,null,3,6", false)]
    [InlineData("5,4,6,null,null,3,7", false)]
    [InlineData("2,2,2", false)]
    [InlineData("1,1", false)]
    [InlineData("10,5,15,null,12", false)]
    public void ValidatesBstProperty(string input, bool expected)
    {
        Assert.Equal(expected, BstValidator.IsValid(TreeTestHelpers.Tree(input)));
    }

    [Theory]
    [InlineData("-2147483648", true)]
    [InlineData("2147483647", true)]
    [InlineData("-2147483648,null,2147483647", true)]
    [InlineData("2147483647,-2147483648", true)]
    [InlineData("-2147483648,-2147483648", false)]
    [InlineData("2147483647,null,2147483647", false)]
    public void HandlesExtremeValues(string input, bool expected)
    {
        Assert.Equal(expected, BstValidator.IsValid(TreeTestHelpers.Tree(input)));
    }

    [Fact]
    public void RandomBstsAreValidUntilAValueIsCorrupted()
    {
        var random = new Random(94);
        for (int round = 0; round < 30; round++)
        {
            var root = TreeTestHelpers.BuildBst(Enumerable.Range(0, 200).Select(_ => random.Next(10_000)))!;
            Assert.True(BstValidator.IsValid(root));

            // Put a value larger than the root deep inside the left subtree.
            var node = root;
            while (node.Left is not null) node = node.Left;
            if (ReferenceEquals(node, root)) continue;
            node.Value = root.Value + 1;

            Assert.False(BstValidator.IsValid(root));
        }
    }
}
