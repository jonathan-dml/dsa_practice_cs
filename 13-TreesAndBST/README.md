# Lesson 13 — Binary Trees and Binary Search Trees

## Learning goals

- Use tree vocabulary: root, child, leaf, subtree, depth, height.
- Traverse a tree depth-first (pre-, in-, post-order) and breadth-first (level order).
- Write recursive functions that combine answers from the left and right subtrees.
- Understand the **binary search tree** property and implement search, insertion and deletion.

## The concept

A **binary tree** is made of nodes; each node has a value and at most two children, `Left` and `Right`.

```
              8          ← root (depth 0)
            /   \
           3     10      ← depth 1
          / \      \
         1   6      14   ← depth 2
            / \    /
           4   7  13     ← leaves: 1, 4, 7, 13 (no children)
```

- The **height** of a tree is the number of nodes (or edges, depending on convention) on its longest root-to-leaf path.
  In this lesson, `MaxDepth` counts **nodes** (the tree above has max depth 4) and `Diameter` counts **edges**.
- Every node is the root of its own **subtree**, which is why recursion fits trees so naturally:

```csharp
int CountNodes(TreeNode? node) => node is null ? 0 : 1 + CountNodes(node.Left) + CountNodes(node.Right);
```

### Traversals

| Traversal | Order | Tree above | Typical use |
|---|---|---|---|
| Pre-order | node, left, right | 8 3 1 6 4 7 10 14 13 | copy / serialize a tree |
| In-order | left, node, right | 1 3 4 6 7 8 10 13 14 | BST → sorted order |
| Post-order | left, right, node | 1 4 7 6 3 13 14 10 8 | delete / evaluate children first |
| Level order | depth by depth (BFS with a queue) | [8] [3 10] [1 6 14] [4 7 13] | shortest paths, printing levels |

Depth-first traversals are easiest to write recursively; they can also use an explicit `Stack<TreeNode>`.
Level order uses a `Queue<TreeNode>`: process `queue.Count` nodes at a time to separate the levels.

### Binary search tree (BST)

A BST keeps this invariant for **every** node: all values in the left subtree are smaller, all values in the right
subtree are larger. The tree above is a BST.

Searching compares with the current node and goes left or right — like binary search — so it costs O(height).

| Operation | Balanced tree | Degenerate tree (a "linked list") |
|---|---|---|
| Search / insert / delete | O(log n) | O(n) |
| Min / max | O(log n) | O(n) |
| In-order traversal | O(n) | O(n) |

Inserting already sorted values produces a degenerate tree. **Self-balancing** trees (AVL, red-black) fix this by
rotating nodes; .NET's `SortedSet<T>` and `SortedDictionary<TKey, TValue>` are red-black trees.

### Deleting from a BST

1. **Leaf** — just remove it.
2. **One child** — replace the node by its child.
3. **Two children** — copy the **in-order successor** (the minimum of the right subtree) into the node, then delete the
   successor from the right subtree (it has at most one child).

```
delete 3:        8                      8
               /   \                  /   \
              3     10     →         4     10
             / \      \             / \      \
            1   6      14          1   6      14
               /                        \
              4                          (4 moved up)
```

### Validating a BST

Checking only `node.Left.Value < node.Value < node.Right.Value` is **not enough**: a value deep in the right subtree
must still be larger than the root. Pass down the allowed range `(min, max)` (use `long` or nullable bounds to handle
`int.MinValue` / `int.MaxValue`), or check that the in-order traversal is strictly increasing.

### Common pitfalls

- Forgetting the `null` base case.
- A node with one child is **not** a leaf (matters for minimum depth).
- Very deep (degenerate) trees can overflow the call stack with recursion.

## The provided `TreeNode` class

`TreeNode.cs` is **already implemented**. Trees in tests are written in LeetCode's level-order format, where `null`
marks a missing child:

```csharp
TreeNode? root = TreeNode.FromLevelOrder(3, 9, 20, null, null, 15, 7);
//     3
//    / \
//   9  20
//     /  \
//    15   7
int?[] values = TreeNode.ToLevelOrder(root);   // [3, 9, 20, null, null, 15, 7]
```

## Exercises

### Exercise 01 — Traversals
File: `Exercise01_Traversals.cs`

```csharp
public static IList<int> PreOrder(TreeNode? root)
public static IList<int> InOrder(TreeNode? root)
public static IList<int> PostOrder(TreeNode? root)
public static IList<IList<int>> LevelOrder(TreeNode? root)
```

| Tree | Pre | In | Post | Level |
|---|---|---|---|---|
| `[3, 9, 20, null, null, 15, 7]` | `3 9 20 15 7` | `9 3 15 20 7` | `9 15 7 20 3` | `[[3], [9, 20], [15, 7]]` |

An empty tree (`null`) produces empty lists.

### Exercise 02 — Maximum and minimum depth
File: `Exercise02_TreeDepth.cs`

```csharp
public static int Max(TreeNode? root)
public static int Min(TreeNode? root)
```

- `Max` is the number of nodes on the **longest** root-to-leaf path; `Min` on the **shortest** one. Both are 0 for `null`.

| Tree | Max | Min |
|---|---|---|
| `[3, 9, 20, null, null, 15, 7]` | `3` | `2` |
| `[2, null, 3, null, 4, null, 5]` | `4` | `4` |

### Exercise 03 — Invert and symmetry
File: `Exercise03_TreeMirror.cs`

```csharp
public static TreeNode? Invert(TreeNode? root)
public static bool IsSymmetric(TreeNode? root)
```

- `Invert` swaps the left and right children of every node **in place** and returns the same root.
- `IsSymmetric` tells whether the tree is a mirror image of itself around its center.

| Input | Output |
|---|---|
| `Invert([4, 2, 7, 1, 3, 6, 9])` | `[4, 7, 2, 9, 6, 3, 1]` |
| `IsSymmetric([1, 2, 2, 3, 4, 4, 3])` | `true` |
| `IsSymmetric([1, 2, 2, null, 3, null, 3])` | `false` |

### Exercise 04 — Validate a BST
File: `Exercise04_ValidateBst.cs`

```csharp
public static bool IsValid(TreeNode? root)
```

Values in the left subtree must be **strictly** smaller and values in the right subtree **strictly** larger, for every node.

| Input | Output |
|---|---|
| `[2, 1, 3]` | `true` |
| `[5, 1, 4, null, null, 3, 6]` | `false` |
| `[5, 4, 6, null, null, 3, 7]` | `false` (3 is in the right subtree of 5) |
| `[2, 2, 2]` | `false` |

### Exercise 05 — Binary search tree
File: `Exercise05_MyBinarySearchTree.cs`

```csharp
public TreeNode? Root { get; }
public int Count { get; }
public bool Insert(int value)       // false if the value already exists
public bool Contains(int value)
public bool Remove(int value)       // false if the value doesn't exist
public int Min()
public int Max()
public IEnumerable<int> InOrder()
```

- Build the tree out of `TreeNode` instances so the tests can inspect its shape through `Root`.
- Insert new values as leaves, following the BST rule.
- `Remove` a node with two children by replacing its value with its **in-order successor** (minimum of the right subtree).
- `Min` and `Max` throw `InvalidOperationException` on an empty tree.

| Operations | `ToLevelOrder(Root)` |
|---|---|
| Insert 5, 3, 8, 1, 4, 7, 9 | `[5, 3, 8, 1, 4, 7, 9]` |
| …then `Remove(5)` | `[7, 3, 8, 1, 4, null, 9]` |

### Exercise 06 — Lowest common ancestor in a BST
File: `Exercise06_LowestCommonAncestor.cs`

```csharp
public static int InBst(TreeNode root, int first, int second)
```

Return the value of the deepest node that has both values in its subtree (a node counts as its own descendant).
Both values are guaranteed to exist in the tree.

```
          6
        /   \
       2     8
      / \   / \
     0   4 7   9
        / \
       3   5
```

| Values | Output |
|---|---|
| `2`, `8` | `6` |
| `2`, `4` | `2` |
| `3`, `5` | `4` |

- Throw `ArgumentNullException` for a `null` root.
- **Target: O(height)** — use the BST property instead of searching both subtrees.

### Exercise 07 — K-th smallest value in a BST
File: `Exercise07_KthSmallest.cs`

```csharp
public static int InBst(TreeNode? root, int k)
```

| Tree | k | Output |
|---|---|---|
| `[3, 1, 4, null, 2]` | `1` | `1` |
| `[5, 3, 6, 2, 4, null, null, 1]` | `3` | `3` |

- Throw `ArgumentOutOfRangeException` when `k < 1` or `k` exceeds the number of nodes.
- **Target: O(height + k)** — stop the in-order traversal once you reach the k-th node.

### Exercise 08 — Diameter of a binary tree
File: `Exercise08_Diameter.cs`

```csharp
public static int Compute(TreeNode? root)
```

The diameter is the number of **edges** on the longest path between any two nodes. The path doesn't have to pass
through the root.

| Tree | Output |
|---|---|
| `[1, 2, 3, 4, 5]` | `3` (4 → 2 → 1 → 3) |
| `[1, 2]` | `1` |
| `[1]` or `null` | `0` |

- **Target: O(n)** — compute heights and the best diameter in the same post-order pass.

## Running the tests

```bash
dotnet test 13-TreesAndBST/TreesAndBST.Tests
dotnet test 13-TreesAndBST/TreesAndBST.Tests --filter "FullyQualifiedName~Exercise05"
```
