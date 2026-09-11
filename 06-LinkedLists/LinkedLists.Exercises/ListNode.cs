namespace DsaPractice.LinkedLists;

/// <summary>
/// PROVIDED — a node of a singly linked list of integers, used by Exercises 02–07.
/// You don't need to change this file.
/// </summary>
public class ListNode
{
    public ListNode(int value, ListNode? next = null)
    {
        Value = value;
        Next = next;
    }

    public int Value { get; set; }

    public ListNode? Next { get; set; }

    /// <summary>Builds a list from the given values and returns its head (null when empty).</summary>
    public static ListNode? FromValues(params int[] values)
    {
        ListNode? head = null;
        for (int i = values.Length - 1; i >= 0; i--)
        {
            head = new ListNode(values[i], head);
        }

        return head;
    }

    /// <summary>
    /// Copies the values of a list into an array. Throws if the list has more than
    /// <paramref name="maxNodes"/> nodes, which almost always means it contains a cycle.
    /// </summary>
    public static int[] ToArray(ListNode? head, int maxNodes = 2_000_000)
    {
        var values = new List<int>();
        for (var node = head; node is not null; node = node.Next)
        {
            if (values.Count == maxNodes)
            {
                throw new InvalidOperationException("The list is too long; it probably contains a cycle.");
            }

            values.Add(node.Value);
        }

        return values.ToArray();
    }

    public override string ToString() => $"ListNode({Value})";
}
