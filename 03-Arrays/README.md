# Lesson 03 — Arrays

## Learning goals

- Understand how arrays are laid out in memory and why indexing is O(1).
- Know the cost of inserting and removing elements in the middle of an array.
- Write **in-place** algorithms that use O(1) extra memory.
- Work with multi-dimensional (`int[,]`) and jagged (`int[][]`) arrays.

## The concept

An array is a **fixed-size block of contiguous memory** holding elements of the same type.
Because every element has the same size, the address of element `i` is computed directly:

```
address(i) = address(0) + i * sizeof(element)

index:     0      1      2      3      4
        ┌──────┬──────┬──────┬──────┬──────┐
int[] → │  7   │  3   │  9   │  1   │  4   │
        └──────┴──────┴──────┴──────┴──────┘
         0x100  0x104  0x108  0x10C  0x110
```

That is why reading or writing `numbers[i]` is O(1), regardless of the array's size. It is also very cache
friendly: iterating an array is usually much faster than iterating a linked structure.

The downside: the size is fixed at creation (`new int[5]`), and inserting or deleting in the middle requires
**shifting** every element after that position.

```
Insert 5 at index 1:
[7, 3, 9, 1, _]  →  shift right  →  [7, _, 3, 9, 1]  →  [7, 5, 3, 9, 1]
```

### Operations

| Operation | Time |
|---|---|
| Read / write by index | O(1) |
| Search for a value (unsorted) | O(n) |
| Search for a value (sorted, binary search) | O(log n) |
| Insert / delete at the end (if there is room) | O(1) |
| Insert / delete at the beginning or middle | O(n) |

### Arrays in C#

```csharp
int[] a = new int[5];              // all zeros
int[] b = [1, 2, 3];               // collection expression
int[,] grid = new int[3, 4];       // rectangular: grid[row, col], grid.GetLength(0) rows
int[][] jagged = [[1], [2, 3]];    // array of arrays, rows may differ in length
Span<int> middle = b.AsSpan(1, 2); // a view over part of an array, no copy
```

Arrays are **reference types**: passing an array to a method lets that method modify the caller's elements.
That's how the in-place exercises below work.

### Key techniques

- **Two indices** moving toward each other (reverse, palindromes) or at different speeds (read/write pointers
  when removing elements in place).
- **Reverse tricks**: rotating by `k` = reverse all, reverse the first `k`, reverse the rest.
- **Prefix/suffix passes**: precompute information from the left and from the right.

### Common pitfalls

- Off-by-one errors: the last valid index is `Length - 1`.
- Creating a new array when the exercise asks you to modify the input **in place**.
- `int[,]` uses `GetLength(dimension)`; `Length` is the *total* number of cells.

## Exercises

### Exercise 01 — Minimum and maximum
File: `Exercise01_MinMax.cs`

```csharp
public static (int Min, int Max) Find(int[] numbers)
```

Find the smallest and largest value with a single pass (no LINQ).

| Input | Output |
|---|---|
| `[3, -1, 7, 0]` | `(-1, 7)` |
| `[5]` | `(5, 5)` |

- Throw `ArgumentNullException` for `null` and `ArgumentException` for an empty array.

### Exercise 02 — Reverse in place
File: `Exercise02_ReverseInPlace.cs`

```csharp
public static void Reverse(int[] numbers)
```

| Before | After |
|---|---|
| `[1, 2, 3, 4]` | `[4, 3, 2, 1]` |

- Don't allocate a new array and don't use `Array.Reverse`. Throw `ArgumentNullException` for `null`.
- **Target: O(n) time, O(1) space.**

<details><summary>Hint</summary>

Swap `numbers[left]` and `numbers[right]`, then move both indices toward the middle.
</details>

### Exercise 03 — Rotate right by k
File: `Exercise03_RotateRight.cs`

```csharp
public static void RotateRight(int[] numbers, int k)
```

Shift every element `k` positions to the right; elements that fall off the end wrap around to the front.

| Input | Output |
|---|---|
| `[1, 2, 3, 4, 5, 6, 7]`, `k = 3` | `[5, 6, 7, 1, 2, 3, 4]` |
| `[1, 2, 3]`, `k = 10` | `[3, 1, 2]` |

- `k` may be larger than the length. Throw `ArgumentOutOfRangeException` when `k < 0`, `ArgumentNullException` for `null`.
- **Target: O(n) time, O(1) extra space.** Rotating one step at a time `k` times is O(n·k) and will time out.

<details><summary>Hint</summary>

Reduce `k %= length`. Reverse the whole array, then reverse the first `k` elements, then the remaining ones.
</details>

### Exercise 04 — Remove duplicates from a sorted array
File: `Exercise04_RemoveDuplicatesFromSorted.cs`

```csharp
public static int Remove(int[] sorted)
```

Rearrange the **sorted** array in place so its first `k` elements are the distinct values in ascending order,
and return `k`. What remains after position `k` doesn't matter.

| Input | Returns | First k elements |
|---|---|---|
| `[1, 1, 2]` | `2` | `[1, 2]` |
| `[0, 0, 1, 1, 1, 2, 2, 3, 3, 4]` | `5` | `[0, 1, 2, 3, 4]` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n) time, O(1) space.**

<details><summary>Hint</summary>

Use a *read* index that scans every element and a *write* index that marks where the next distinct value goes.
</details>

### Exercise 05 — Move zeroes to the end
File: `Exercise05_MoveZeroes.cs`

```csharp
public static void MoveZeroes(int[] numbers)
```

Move all `0`s to the end while keeping the **relative order** of the non-zero elements.

| Before | After |
|---|---|
| `[0, 1, 0, 3, 12]` | `[1, 3, 12, 0, 0]` |

- In place. Throw `ArgumentNullException` for `null`.
- **Target: O(n) time.**

### Exercise 06 — Merge two sorted arrays
File: `Exercise06_MergeSortedArrays.cs`

```csharp
public static int[] Merge(int[] first, int[] second)
```

Both inputs are sorted ascending. Return a **new** sorted array with all the elements of both.

| Input | Output |
|---|---|
| `[1, 3, 5]`, `[2, 4, 6, 8]` | `[1, 2, 3, 4, 5, 6, 8]` |
| `[]`, `[1]` | `[1]` |

- Don't concatenate and sort. Throw `ArgumentNullException` if either is `null`.
- **Target: O(n + m).**

<details><summary>Hint</summary>

Keep one index per input array and repeatedly copy the smaller current element. Copy the leftovers at the end.
</details>

### Exercise 07 — Product of array except self
File: `Exercise07_ProductExceptSelf.cs`

```csharp
public static long[] Compute(int[] numbers)
```

Return an array where `result[i]` is the product of every element except `numbers[i]`, **without using division**.

| Input | Output |
|---|---|
| `[1, 2, 3, 4]` | `[24, 12, 8, 6]` |
| `[-1, 1, 0, -3, 3]` | `[0, 0, 9, 0, 0]` |

- Every product fits in a `long`. Throw `ArgumentNullException` for `null`.
- **Target: O(n).**

<details><summary>Hint</summary>

`result[i] = (product of everything left of i) * (product of everything right of i)`.
Fill `result` with the left products in one pass, then multiply in the right products in a second pass going backwards.
</details>

### Exercise 08 — Spiral order
File: `Exercise08_SpiralOrder.cs`

```csharp
public static IList<int> Traverse(int[,] matrix)
```

Return the elements of a rectangular matrix in clockwise spiral order, starting at the top-left corner.

```
 1  2  3  4
 5  6  7  8     →  1, 2, 3, 4, 8, 12, 11, 10, 9, 5, 6, 7
 9 10 11 12
```

- Throw `ArgumentNullException` for `null`. An empty matrix returns an empty list.

<details><summary>Hint</summary>

Keep four boundaries: `top`, `bottom`, `left`, `right`. Walk the top row, the right column, the bottom row
(if `top <= bottom`) and the left column (if `left <= right`), shrinking the boundaries after each walk.
</details>

### Exercise 09 — Rotate a square matrix 90° clockwise
File: `Exercise09_RotateMatrix.cs`

```csharp
public static void RotateClockwise(int[][] matrix)
```

```
1 2 3        7 4 1
4 5 6   →    8 5 2
7 8 9        9 6 3
```

- In place. Throw `ArgumentNullException` for `null` and `ArgumentException` when the matrix is not square.

<details><summary>Hint</summary>

Transpose the matrix (swap `m[i][j]` with `m[j][i]`), then reverse each row.
</details>

## Running the tests

```bash
dotnet test 03-Arrays/Arrays.Tests
dotnet test 03-Arrays/Arrays.Tests --filter "FullyQualifiedName~Exercise03"
```
