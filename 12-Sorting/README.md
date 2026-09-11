# Lesson 12 — Sorting

## Learning goals

- Implement the classic elementary sorts (bubble, selection, insertion) and understand when they're good enough.
- Implement the divide-and-conquer sorts: **merge sort** and **quicksort**.
- Understand **stability**, **in-place** sorting, best/worst cases, and the O(n log n) lower bound for comparison sorts.
- Beat that bound with **counting sort** when keys are small integers.
- Use sorting as a tool (merging intervals) and partition-based selection (**quickselect**).

## The concept

### Elementary sorts — O(n²)

**Bubble sort** repeatedly walks the array swapping adjacent out-of-order pairs; the largest element "bubbles" to the
end each pass. If a pass makes no swap, the array is sorted → O(n) on sorted input.

**Selection sort** finds the minimum of the unsorted part and swaps it to the front. Always O(n²) comparisons, but at
most n − 1 swaps.

**Insertion sort** grows a sorted prefix, inserting each new element into its place by shifting larger elements right —
like sorting playing cards in your hand. O(n + inversions): very fast on nearly sorted data, which is why real-world
sorts use it for small subarrays.

```
insertion sort, inserting 3:
[1, 4, 7 | 3, 9]  →  [1, 4, _, 7, 9]  →  [1, _, 4, 7, 9]  →  [1, 3, 4, 7, 9]
```

### Merge sort — O(n log n), stable

Split the array in half, sort each half recursively, then **merge** the two sorted halves (Lesson 03, Exercise 06).

```
[38, 27, 43, 3, 9, 82, 10]
        split ↓
[38, 27, 43]      [3, 9, 82, 10]
   ...                 ...
[27, 38, 43]      [3, 9, 10, 82]
        merge ↓
[3, 9, 10, 27, 38, 43, 82]
```

There are log n levels and each level does O(n) work. It needs O(n) extra memory for merging. Taking from the **left**
half on ties (`<=`) keeps equal elements in their original order: merge sort is **stable**.

### Quicksort — O(n log n) average, O(n²) worst

Pick a **pivot**, **partition** the array so smaller elements come before it and larger ones after, then recurse on both
sides. It sorts in place and is very fast in practice.

- Choosing the first or last element as pivot on already sorted input makes every partition lopsided → O(n²) and very
  deep recursion. Use a **random** or middle pivot.
- Many equal keys can also degrade simple partition schemes. **Three-way partitioning** (`< pivot`, `== pivot`,
  `> pivot`) handles duplicates gracefully.
- Recursing into the **smaller** side first (and looping on the larger one) keeps the stack depth O(log n).

### Counting sort — O(n + k)

When values are integers in a small range `[0, k]`, count how many times each value appears, then write them back in
order. No comparisons at all.

### Comparison of algorithms

| Algorithm | Best | Average | Worst | Extra space | Stable |
|---|---|---|---|---|---|
| Bubble (early exit) | O(n) | O(n²) | O(n²) | O(1) | yes |
| Selection | O(n²) | O(n²) | O(n²) | O(1) | no |
| Insertion | O(n) | O(n²) | O(n²) | O(1) | yes |
| Merge | O(n log n) | O(n log n) | O(n log n) | O(n) | yes |
| Quick (random pivot) | O(n log n) | O(n log n) | O(n²) (unlikely) | O(log n) | no |
| Counting | O(n + k) | O(n + k) | O(n + k) | O(k) | yes* |

Any sort based only on comparisons needs Ω(n log n) comparisons in the worst case.
.NET's `Array.Sort` uses **introsort** (quicksort + heapsort + insertion sort) and is not stable; LINQ's `OrderBy` is stable.

### Stability

A stable sort keeps equal elements in their original relative order. It matters when sorting records by one key after
another (e.g. sort by name, then stable-sort by department → people grouped by department and alphabetical inside).

### Quickselect

To find the k-th largest element you don't need a full sort. Partition once: the pivot lands at its final position `p`.
If `p` is the position you want, you're done; otherwise continue only on the side that contains it.
Average O(n), worst O(n²) (again, randomize the pivot).

### Common pitfalls

- Off-by-one errors in partition bounds, leading to infinite recursion.
- Allocating new arrays at every level of merge sort (allocate one buffer and reuse it).
- Using `a - b` as a comparison: it overflows for large values. Use `a.CompareTo(b)`.

## Exercises

All sorts operate **in place** on the given array unless stated otherwise. Don't use `Array.Sort`, `List.Sort` or LINQ
ordering inside your implementations.

### Exercise 01 — Bubble sort
File: `Exercise01_BubbleSort.cs`

```csharp
public static void Sort(int[] numbers)
```

- Stop as soon as a full pass makes no swaps. **Target: O(n) on already sorted input** (tested with 1 million elements).
- Throw `ArgumentNullException` for `null`.

### Exercise 02 — Selection sort
File: `Exercise02_SelectionSort.cs`

```csharp
public static void Sort(int[] numbers)
```

- Throw `ArgumentNullException` for `null`.

### Exercise 03 — Insertion sort
File: `Exercise03_InsertionSort.cs`

```csharp
public static void Sort(int[] numbers)
```

- **Target: O(n) on nearly sorted input** — shift elements instead of swapping them all the way and stop as soon as the
  element is in place.
- Throw `ArgumentNullException` for `null`.

### Exercise 04 — Merge sort (generic and stable)
File: `Exercise04_MergeSort.cs`

```csharp
public static void Sort<T>(T[] items, Comparison<T> comparison)
```

- Sort using the provided comparison (`comparison(a, b) < 0` means `a` comes before `b`).
- Must be **stable**. **Target: O(n log n)**.
- Throw `ArgumentNullException` if `items` or `comparison` is `null`.

<details><summary>Hint</summary>

Allocate a single `T[] buffer` of the same length. `SortRange(lo, hi)` sorts `[lo, hi)`: split at `mid`, sort both halves,
merge them into the buffer, and copy back. When merging, take from the left half when `comparison(left, right) <= 0`.
</details>

### Exercise 05 — Quicksort
File: `Exercise05_QuickSort.cs`

```csharp
public static void Sort(int[] numbers)
```

- **Target: O(n log n) on average**, including already sorted input and inputs with many duplicates
  (each tested with 1 million elements).
- Throw `ArgumentNullException` for `null`.

<details><summary>Hint</summary>

Use a random pivot and three-way partitioning (Dutch national flag from Lesson 10). Recurse into the smaller of the two
outer regions and loop on the larger one.
</details>

### Exercise 06 — Counting sort
File: `Exercise06_CountingSort.cs`

```csharp
public static void Sort(int[] values)
```

- All values must be **non-negative**; throw `ArgumentException` otherwise, `ArgumentNullException` for `null`.
- **Target: O(n + max)** — no comparisons between elements.

### Exercise 07 — Merge intervals
File: `Exercise07_MergeIntervals.cs`

```csharp
public static int[][] Merge(int[][] intervals)
```

Each interval is `[start, end]` with `start <= end`. Merge all overlapping (or touching) intervals and return them sorted
by start.

| Input | Output |
|---|---|
| `[[1, 3], [2, 6], [8, 10], [15, 18]]` | `[[1, 6], [8, 10], [15, 18]]` |
| `[[1, 4], [4, 5]]` | `[[1, 5]]` |
| `[[8, 10], [1, 3]]` | `[[1, 3], [8, 10]]` |

- Throw `ArgumentNullException` for `null`, and `ArgumentException` if an interval doesn't have exactly two values or
  has `start > end`.
- You may use `Array.Sort`/LINQ here — the point is to *use* sorting. **Target: O(n log n).**

### Exercise 08 — K-th largest element (quickselect)
File: `Exercise08_QuickSelect.cs`

```csharp
public static int KthLargest(int[] numbers, int k)
```

| Input | Output |
|---|---|
| `[3, 2, 1, 5, 6, 4]`, `k = 2` | `5` |
| `[3, 2, 3, 1, 2, 4, 5, 5, 6]`, `k = 4` | `4` |

- Don't modify the input array (work on a copy). Throw `ArgumentNullException` for `null` and
  `ArgumentOutOfRangeException` when `k < 1` or `k > numbers.Length`.
- **Target: O(n) on average** with quickselect.

## Running the tests

```bash
dotnet test 12-Sorting/Sorting.Tests
dotnet test 12-Sorting/Sorting.Tests --filter "FullyQualifiedName~Exercise05"
```
