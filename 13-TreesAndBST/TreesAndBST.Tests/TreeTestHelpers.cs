namespace DsaPractice.TreesAndBST.Tests;

internal static class TreeTestHelpers
{
    /// <summary>Parses "3,9,20,null,null,15,7" into level-order values (attributes can't hold int?[] arrays).</summary>
    public static int?[] Values(string levelOrder) =>
        string.IsNullOrWhiteSpace(levelOrder)
            ? []
            : levelOrder.Split(',')
                .Select(s => s.Trim())
                .Select(s => s == "null" ? (int?)null : int.Parse(s, System.Globalization.CultureInfo.InvariantCulture))
                .ToArray();

    /// <summary>Builds a tree from a level-order string such as "3,9,20,null,null,15,7".</summary>
    public static TreeNode? Tree(string levelOrder) => TreeNode.FromLevelOrder(Values(levelOrder));

    /// <summary>Builds a (not necessarily balanced) BST by inserting the values in order. Duplicates are ignored.</summary>
    public static TreeNode? BuildBst(IEnumerable<int> values)
    {
        TreeNode? root = null;
        foreach (int value in values)
        {
            if (root is null)
            {
                root = new TreeNode(value);
                continue;
            }

            var node = root;
            while (true)
            {
                if (value == node.Value) break;
                if (value < node.Value)
                {
                    if (node.Left is null) { node.Left = new TreeNode(value); break; }
                    node = node.Left;
                }
                else
                {
                    if (node.Right is null) { node.Right = new TreeNode(value); break; }
                    node = node.Right;
                }
            }
        }

        return root;
    }

    public static TreeNode? RandomBst(Random random, int count, int maxValue = 1000) =>
        BuildBst(Enumerable.Range(0, count).Select(_ => random.Next(maxValue)));

    public static int CountNodes(TreeNode? node) => node is null ? 0 : 1 + CountNodes(node.Left) + CountNodes(node.Right);

    public static int Height(TreeNode? node) => node is null ? 0 : 1 + Math.Max(Height(node.Left), Height(node.Right));

    public static void CollectInOrder(TreeNode? node, List<int> values)
    {
        if (node is null) return;
        CollectInOrder(node.Left, values);
        values.Add(node.Value);
        CollectInOrder(node.Right, values);
    }
}
