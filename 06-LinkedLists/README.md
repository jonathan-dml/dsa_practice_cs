# Lesson 06 — Linked Lists

## Learning goals

- Represent a sequence as **nodes connected by references** instead of contiguous memory.
- Implement singly and doubly linked lists with O(1) insertion/removal at the ends.
- Manipulate `Next` pointers safely: reversing, merging, removing.
- Use the **slow/fast pointer** (runner) technique and the **dummy head** trick.

## The concept

A linked list stores each element in a separate **node** object that also holds a reference to the next node.
The list itself only needs to remember the first node (**head**) — and often the last one (**tail**).

```
Singly linked list

 head                                   tail
  │                                      │
  ▼                                      ▼
┌────┬───┐    ┌────┬───┐    ┌────┬───┐    ┌────┬──────┐
│ 10 │ ●─┼──▶ │ 20 │ ●─┼──▶ │ 30 │ ●─┼──▶ │ 40 │ null │
└────┴───┘    └────┴───┘    └────┴───┘    └────┴──────┘
```

A **doubly** linked list also stores a `Previous` reference, so it can be walked backwards and a node can be
removed without searching for its predecessor.

```
null ◀─┬────┬─▶ ◀─┬────┬─▶ ◀─┬────┬─▶ null
       │ 10 │     │ 20 │     │ 30 │
       └────┘     └────┘     └────┘
```

### Arrays vs linked lists

| Operation | Array / `List<T>` | Singly linked (with tail) | Doubly linked |
|---|---|---|---|
| Access by index | **O(1)** | O(n) | O(n) |
| Add / remove at front | O(n) | **O(1)** | **O(1)** |
| Add at back | amortized O(1) | **O(1)** | **O(1)** |
| Remove at back | O(1) | O(n) (need the previous node) | **O(1)** |
| Insert / remove given a node | O(n) | O(1) after it | **O(1)** |
| Memory per element | value only | value + 1 reference | value + 2 references |
| Cache friendliness | excellent | poor | poor |

.NET's `LinkedList<T>` is a doubly linked list.

### Techniques

**Reversing** — walk the list once, flipping each `Next` reference. Keep three variables:

```
prev = null, current = head
while current != null:
    next = current.Next      // remember the rest of the list
    current.Next = prev      // flip the arrow
    prev = current
    current = next
return prev                   // new head
```

**Slow and fast pointers** — move `slow` one step and `fast` two steps. When `fast` reaches the end, `slow` is in the
middle. If the list has a cycle, `fast` eventually *laps* `slow` and they meet (Floyd's algorithm).

**Dummy head** — create a fake node before the head so that removing or inserting at the front is not a special
case: `var dummy = new ListNode(0, head); ... return dummy.Next;`

### Common pitfalls

- Losing the rest of the list by overwriting `Next` before saving it.
- Forgetting to update `tail` (e.g. after removing the last remaining element).
- Null reference exceptions on `node.Next.Next` — check `fast != null && fast.Next != null`.
- Infinite loops on cyclic lists.

## The provided `ListNode` class

Exercises 02–07 work directly with nodes. `ListNode.cs` is **already implemented**:

```csharp
public class ListNode
{
    public int Value { get; set; }
    public ListNode? Next { get; set; }

    public static ListNode? FromValues(params int[] values);   // [1, 2, 3] → 1 → 2 → 3
    public static int[] ToArray(ListNode? head);               // 1 → 2 → 3 → [1, 2, 3]
}
```

## Exercises

### Exercise 01 — Singly linked list
File: `Exercise01_MySinglyLinkedList.cs`

```csharp
public int Count { get; }
public void AddFirst(T value)
public void AddLast(T value)
public T RemoveFirst()
public T PeekFirst()
public T PeekLast()
public bool Contains(T value)
public IEnumerator<T> GetEnumerator()
```

- Keep both a head and a tail reference so `AddFirst`, `AddLast`, `RemoveFirst`, `PeekFirst` and `PeekLast` are **O(1)**.
- `RemoveFirst`, `PeekFirst` and `PeekLast` throw `InvalidOperationException` on an empty list.
- `Contains` uses `EqualityComparer<T>.Default`.
- Don't use `List<T>` or `LinkedList<T>` internally — create your own private `Node` class.

### Exercise 02 — Reverse a linked list
File: `Exercise02_ReverseList.cs`

```csharp
public static ListNode? Reverse(ListNode? head)
```

| Input | Output |
|---|---|
| `1 → 2 → 3 → 4 → 5` | `5 → 4 → 3 → 2 → 1` |
| `null` | `null` |

- Reuse the existing nodes (don't allocate new ones).
- **Target: O(n) time, O(1) space** (iterative).

### Exercise 03 — Middle node
File: `Exercise03_MiddleNode.cs`

```csharp
public static ListNode? Find(ListNode? head)
```

Return the middle node. With an even number of nodes, return the **second** of the two middle nodes.

| Input | Output |
|---|---|
| `1 → 2 → 3 → 4 → 5` | node `3` |
| `1 → 2 → 3 → 4 → 5 → 6` | node `4` |
| `null` | `null` |

- **Target: one pass**, using slow/fast pointers.

### Exercise 04 — Detect a cycle
File: `Exercise04_HasCycle.cs`

```csharp
public static bool HasCycle(ListNode? head)
```

Return `true` if following `Next` references ever revisits a node.

```
1 → 2 → 3 → 4
    ▲       │
    └───────┘      → true
```

- **Target: O(n) time, O(1) space** (Floyd's tortoise and hare). A `HashSet<ListNode>` works but uses O(n) space.

### Exercise 05 — Merge two sorted lists
File: `Exercise05_MergeTwoSorted.cs`

```csharp
public static ListNode? Merge(ListNode? first, ListNode? second)
```

Both lists are sorted ascending. Splice their nodes together into one sorted list and return its head.

| Input | Output |
|---|---|
| `1 → 2 → 4`, `1 → 3 → 4` | `1 → 1 → 2 → 3 → 4 → 4` |
| `null`, `0` | `0` |

<details><summary>Hint</summary>

Start with a dummy node and a `tail` pointer. Repeatedly attach the smaller of the two current nodes to `tail.Next`.
When one list runs out, attach the rest of the other.
</details>

### Exercise 06 — Remove the n-th node from the end
File: `Exercise06_RemoveNthFromEnd.cs`

```csharp
public static ListNode? Remove(ListNode? head, int n)
```

| Input | Output |
|---|---|
| `1 → 2 → 3 → 4 → 5`, `n = 2` | `1 → 2 → 3 → 5` |
| `1`, `n = 1` | `null` |
| `1 → 2`, `n = 2` | `2` |

- Throw `ArgumentOutOfRangeException` when `n < 1` or `n` is greater than the list length.
- **Target: one pass.**

<details><summary>Hint</summary>

Use a dummy head. Move a `fast` pointer `n` steps ahead, then move `fast` and `slow` together until `fast.Next` is null.
`slow.Next` is the node to remove.
</details>

### Exercise 07 — Palindrome linked list
File: `Exercise07_IsPalindromeList.cs`

```csharp
public static bool IsPalindrome(ListNode? head)
```

| Input | Output |
|---|---|
| `1 → 2 → 2 → 1` | `true` |
| `1 → 2 → 3 → 2 → 1` | `true` |
| `1 → 2` | `false` |

- The list must be **unchanged** when the method returns.
- **Target: O(n) time.** Bonus: O(1) extra space.

<details><summary>Hint</summary>

Find the middle (Exercise 03), reverse the second half (Exercise 02), compare both halves, then reverse the second half
back to restore the list.
</details>

### Exercise 08 — Doubly linked list
File: `Exercise08_MyDoublyLinkedList.cs`

```csharp
public int Count { get; }
public void AddFirst(T value)
public void AddLast(T value)
public T RemoveFirst()
public T RemoveLast()
public T PeekFirst()
public T PeekLast()
public IEnumerator<T> GetEnumerator()   // front to back
public IEnumerable<T> Backwards()        // back to front
```

- All add/remove/peek operations are **O(1)**; removing or peeking on an empty list throws `InvalidOperationException`.
- Keep `Previous` and `Next` references consistent — the tests walk the list in both directions.

## Running the tests

```bash
dotnet test 06-LinkedLists/LinkedLists.Tests
dotnet test 06-LinkedLists/LinkedLists.Tests --filter "FullyQualifiedName~Exercise04"
```
