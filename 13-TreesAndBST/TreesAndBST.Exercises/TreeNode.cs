namespace DsaPractice.TreesAndBST;

/// <summary>
/// PROVIDED — a node of a binary tree of integers, plus helpers to build trees from level-order arrays.
/// You don't need to change this file.
/// </summary>
public class TreeNode
{
    public TreeNode(int value, TreeNode? left = null, TreeNode? right = null)
    {
        Value = value;
        Left = left;
        Right = right;
    }

    public int Value { get; set; }

    public TreeNode? Left { get; set; }

    public TreeNode? Right { get; set; }

    /// <summary>
    /// Builds a tree from LeetCode-style level-order values, where null marks a missing child.
    /// Example: [3, 9, 20, null, null, 15, 7].
    /// </summary>
    public static TreeNode? FromLevelOrder(params int?[] values)
    {
        if (values.Length == 0 || values[0] is null)
        {
            return null;
        }

        var root = new TreeNode(values[0]!.Value);
        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        int index = 1;

        while (queue.Count > 0 && index < values.Length)
        {
            var node = queue.Dequeue();

            if (index < values.Length && values[index] is int left)
            {
                node.Left = new TreeNode(left);
                queue.Enqueue(node.Left);
            }
            index++;

            if (index < values.Length && values[index] is int right)
            {
                node.Right = new TreeNode(right);
                queue.Enqueue(node.Right);
            }
            index++;
        }

        return root;
    }

    /// <summary>Converts a tree to level-order values (with nulls for missing children), trimming trailing nulls.</summary>
    public static int?[] ToLevelOrder(TreeNode? root)
    {
        var values = new List<int?>();
        var queue = new Queue<TreeNode?>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node is null)
            {
                values.Add(null);
                continue;
            }

            values.Add(node.Value);
            queue.Enqueue(node.Left);
            queue.Enqueue(node.Right);
        }

        while (values.Count > 0 && values[^1] is null)
        {
            values.RemoveAt(values.Count - 1);
        }

        return values.ToArray();
    }

    public override string ToString() => $"TreeNode({Value})";
}
