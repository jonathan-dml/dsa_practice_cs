namespace DsaPractice.TreesAndBST.Tests;

public class Exercise06_LowestCommonAncestorTests
{
    private static readonly TreeNode Sample = TreeNode.FromLevelOrder(6, 2, 8, 0, 4, 7, 9, null, null, 3, 5)!;

    [Theory]
    [InlineData(2, 8, 6)]
    [InlineData(8, 2, 6)]
    [InlineData(2, 4, 2)]
    [InlineData(3, 5, 4)]
    [InlineData(0, 5, 2)]
    [InlineData(7, 9, 8)]
    [InlineData(6, 6, 6)]
    [InlineData(3, 3, 3)]
    [InlineData(0, 9, 6)]
    public void FindsLowestCommonAncestor(int first, int second, int expected)
    {
        Assert.Equal(expected, LowestCommonAncestor.InBst(Sample, first, second));
    }

    [Fact]
    public void MatchesPathIntersectionOnRandomTrees()
    {
        var random = new Random(97);
        for (int round = 0; round < 30; round++)
        {
            int[] values = Enumerable.Range(0, 100).Select(_ => random.Next(1000)).Distinct().ToArray();
            var root = TreeTestHelpers.BuildBst(values)!;

            for (int q = 0; q < 20; q++)
            {
                int a = values[random.Next(values.Length)];
                int b = values[random.Next(values.Length)];

                Assert.Equal(ExpectedLca(root, a, b), LowestCommonAncestor.InBst(root, a, b));
            }
        }
    }

    [Fact]
    public void ThrowsForNullRoot()
    {
        Assert.Throws<ArgumentNullException>(() => LowestCommonAncestor.InBst(null!, 1, 2));
    }

    private static int ExpectedLca(TreeNode root, int a, int b)
    {
        List<int> PathTo(int value)
        {
            var path = new List<int>();
            for (var node = root; node is not null; node = value < node.Value ? node.Left : node.Right)
            {
                path.Add(node.Value);
                if (node.Value == value) break;
            }
            return path;
        }

        var pathA = PathTo(a);
        var pathB = PathTo(b);
        int lca = root.Value;
        for (int i = 0; i < Math.Min(pathA.Count, pathB.Count) && pathA[i] == pathB[i]; i++) lca = pathA[i];
        return lca;
    }
}
