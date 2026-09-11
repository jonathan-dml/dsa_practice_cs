# Lesson 14 — Heaps and Priority Queues

## Learning goals

- Store a complete binary tree in an array and navigate it with index arithmetic.
- Implement a binary heap with **sift up** and **sift down**.
- Build a heap in O(n) and use it to sort (heap sort).
- Use `PriorityQueue<TElement, TPriority>` for top-k problems, k-way merges, streaming medians and simulations.

## The concept

A **priority queue** always hands out the element with the highest priority (e.g. the smallest value) next.
The classic implementation is a **binary heap**: a complete binary tree where every parent is ≤ its children
(a *min-heap*). The smallest element is always at the root.

```
            1                     index:  0  1  2  3  4  5  6
          /   \                   array: [1, 3, 2, 7, 4, 5, 9]
         3     2
        / \   / \                 parent(i) = (i - 1) / 2
       7   4 5   9                left(i)   = 2i + 1
                                  right(i)  = 2i + 2
```

Because the tree is **complete** (filled level by level, left to right), it fits in an array with no gaps and no
child pointers.

### Add — sift up

Append the new element at the end, then swap it with its parent while it's smaller than the parent.

```
add 0:  [1, 3, 2, 7, 4, 5, 9, 0]
        0 < 7 → swap → [1, 3, 2, 0, 4, 5, 9, 7]
        0 < 3 → swap → [1, 0, 2, 3, 4, 5, 9, 7]
        0 < 1 → swap → [0, 1, 2, 3, 4, 5, 9, 7]
```

### Poll — sift down

Take the root, move the **last** element to the root, then swap it with its **smaller** child while it's larger than
that child.

### Build heap in O(n)

Calling `Add` n times costs O(n log n). Instead, sift down every non-leaf node from the last parent `(n / 2) - 1`
back to the root. Most nodes are near the bottom and move only a little, which adds up to O(n).

### Operations

| Operation | Time |
|---|---|
| `Peek` | O(1) |
| `Add` | O(log n) |
| `Poll` | O(log n) |
| Build from n elements | O(n) |
| Heap sort | O(n log n), in place, not stable |

### Priority queues in .NET

```csharp
var queue = new PriorityQueue<string, int>();      // min-heap by priority
queue.Enqueue("low", 5);
queue.Enqueue("urgent", 1);
string next = queue.Dequeue();                      // "urgent"

// Max-heap: reverse the comparer
var maxQueue = new PriorityQueue<int, int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));
```

### Classic patterns

| Pattern | Idea |
|---|---|
| **Top k** largest | keep a **min**-heap of size k; if a new value beats the root, replace the root |
| **k-way merge** | heap holds the current head of each list; poll the smallest, push its successor |
| **Running median** | max-heap for the lower half, min-heap for the upper half, keep sizes balanced |
| **Simulation** | repeatedly take the best/worst items (e.g. smash the two heaviest stones) |

### Common pitfalls

- Off-by-one errors in child indices (`2i + 1` / `2i + 2` for 0-based arrays).
- Forgetting to pick the **smaller** child when sifting down.
- `PriorityQueue` is not stable and doesn't support "update priority" — push a new entry instead.
- Averaging two large `int`s overflows: use `((long)a + b) / 2.0`.

## Exercises

### Exercise 01 — Binary min-heap
File: `Exercise01_MyMinHeap.cs`

```csharp
public MyMinHeap(IComparer<T>? comparer = null)
public int Count { get; }
public void Add(T item)
public T Peek()
public T Poll()
```

- Use an array or `List<T>` as storage and implement sift up / sift down yourself (no `PriorityQueue`, `SortedSet`, sorting).
- Use `comparer ?? Comparer<T>.Default`. With a reversed comparer the same class works as a max-heap.
- `Peek` and `Poll` throw `InvalidOperationException` when empty.
- **Target: O(log n)** for `Add` and `Poll`.

### Exercise 02 — Build heap and heap sort
File: `Exercise02_HeapOperations.cs`

```csharp
public static void BuildMinHeap(int[] values)
public static void HeapSort(int[] values)
```

- `BuildMinHeap` rearranges the array **in place** so that `values[i] <= values[2i + 1]` and `values[i] <= values[2i + 2]`
  (when those children exist). **Target: O(n)** using bottom-up sift down.
- `HeapSort` sorts ascending **in place** with O(1) extra space. Build a **max**-heap, then repeatedly swap the root with
  the last element of the heap and sift down within the shrinking heap.
- Don't use `Array.Sort`, LINQ or other collections. Throw `ArgumentNullException` for `null`.

### Exercise 03 — K-th largest element in a stream
File: `Exercise03_KthLargestInStream.cs`

```csharp
public KthLargestInStream(int k, IEnumerable<int> initial)
public int Add(int value)
```

`Add` records a value and returns the k-th largest value seen so far.

```
k = 3, initial = [4, 5, 8, 2]
Add(3)  → 4
Add(5)  → 5
Add(10) → 5
Add(9)  → 8
Add(4)  → 8
```

- If fewer than `k` values have been seen after adding, the value is still recorded but `Add` throws
  `InvalidOperationException`.
- The constructor throws `ArgumentOutOfRangeException` when `k < 1` and `ArgumentNullException` for `null`.
- **Target: O(log k)** per `Add` — keep only the k largest values in a min-heap.

### Exercise 04 — Top k frequent elements
File: `Exercise04_TopKFrequent.cs`

```csharp
public static int[] Elements(int[] numbers, int k)
```

Return the `k` most frequent values ordered by frequency (highest first); ties are ordered by value (smallest first).

| Input | Output |
|---|---|
| `[1, 1, 1, 2, 2, 3]`, `k = 2` | `[1, 2]` |
| `[4, 4, 5, 5, 6]`, `k = 2` | `[4, 5]` |

- If `k` exceeds the number of distinct values, return all of them.
- Throw `ArgumentNullException` for `null` and `ArgumentOutOfRangeException` for a negative `k`.
- **Target: O(n log k).**

### Exercise 05 — Merge k sorted arrays
File: `Exercise05_MergeKSorted.cs`

```csharp
public static int[] Merge(int[][] arrays)
```

| Input | Output |
|---|---|
| `[[1, 4, 5], [1, 3, 4], [2, 6]]` | `[1, 1, 2, 3, 4, 4, 5, 6]` |
| `[[], [1]]` | `[1]` |

- Throw `ArgumentNullException` for `null` and `ArgumentException` if any inner array is `null`.
- **Target: O(n log k)** where `n` is the total number of elements.

<details><summary>Hint</summary>

Enqueue `(arrayIndex, elementIndex)` for the first element of each array with the element as priority. Each time you
dequeue, append the value and enqueue the next element of the same array.
</details>

### Exercise 06 — K closest points to the origin
File: `Exercise06_KClosestPoints.cs`

```csharp
public static (int X, int Y)[] Find((int X, int Y)[] points, int k)
```

Return the `k` points closest to `(0, 0)` (Euclidean distance), ordered by distance; ties are ordered by `X`, then `Y`.

| Input | Output |
|---|---|
| `[(1, 3), (-2, 2)]`, `k = 1` | `[(-2, 2)]` |
| `[(3, 3), (5, -1), (-2, 4)]`, `k = 2` | `[(3, 3), (-2, 4)]` |

- Compare squared distances as `long` — coordinates can be as large as `±100 000`.
- Throw `ArgumentNullException` for `null` and `ArgumentOutOfRangeException` when `k < 0` or `k > points.Length`.
- **Target: O(n log k).**

### Exercise 07 — Running median
File: `Exercise07_MedianFinder.cs`

```csharp
public int Count { get; }
public void Add(int value)
public double Median()
```

`Median` returns the middle value of everything added so far, or the average of the two middle values when the count
is even.

```
Add(1)  → Median() = 1
Add(2)  → Median() = 1.5
Add(3)  → Median() = 2
```

- `Median` throws `InvalidOperationException` when nothing was added.
- **Target: O(log n)** per `Add`, **O(1)** per `Median`.

<details><summary>Hint</summary>

Keep a max-heap `lower` and a min-heap `upper`, with every value in `lower` ≤ every value in `upper` and
`lower.Count` equal to `upper.Count` or one more. The median is `lower`'s top, or the average of both tops.
</details>

### Exercise 08 — Last stone weight
File: `Exercise08_LastStoneWeight.cs`

```csharp
public static int Smash(int[] stones)
```

Repeatedly take the two heaviest stones `x <= y` and smash them: if `x == y` both are destroyed, otherwise a stone of
weight `y - x` remains. Return the weight of the last stone, or `0` if none are left.

| Input | Output |
|---|---|
| `[2, 7, 4, 1, 8, 1]` | `1` |
| `[3, 3]` | `0` |
| `[]` | `0` |

- Throw `ArgumentNullException` for `null`. **Target: O(n log n).**

## Running the tests

```bash
dotnet test 14-Heaps/Heaps.Tests
dotnet test 14-Heaps/Heaps.Tests --filter "FullyQualifiedName~Exercise07"
```
