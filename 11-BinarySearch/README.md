# Lesson 11 — Binary Search

## Learning goals

- Search a sorted collection in **O(log n)** by halving the search space.
- Write bug-free binary searches using a clear **loop invariant**.
- Find boundaries (first/last occurrence, insertion point) with lower-bound style searches.
- Apply binary search to rotated arrays, peaks, and to **the answer itself** ("binary search on the answer").

## The concept

Guessing a number between 1 and 100? Ask "is it bigger than 50?" and you eliminate half the options with one question.
Binary search does exactly that on sorted data:

```
target = 23
index:   0   1   2   3   4   5   6   7   8   9
       [ 2,  5,  8, 12, 16, 23, 38, 56, 72, 91]
         lo              mid                 hi     16 < 23 → lo = mid + 1

                             lo      mid     hi     56 > 23 → hi = mid - 1

                             lo  hi                 mid = 5 → 23 == 23 ✔
                             mid
```

Each step halves the range, so at most ⌈log₂(n + 1)⌉ steps are needed: about 20 for a million elements,
about 30 for a billion.

### A template with an invariant

```csharp
int lo = 0, hi = array.Length - 1;          // invariant: if target exists, it is inside [lo, hi]
while (lo <= hi)
{
    int mid = lo + (hi - lo) / 2;           // avoids int overflow of (lo + hi)
    if (array[mid] == target) return mid;
    if (array[mid] < target) lo = mid + 1;  // target can only be to the right
    else hi = mid - 1;                      // target can only be to the left
}
return -1;                                  // range is empty
```

### Lower bound (first position where a condition becomes true)

Many problems ask for a **boundary** rather than an exact match. If a predicate is `false, false, …, false, true, …, true`
over the indices, binary search finds the first `true`:

```csharp
int lo = 0, hi = n;                         // answer is in [lo, hi]; hi = n means "never true"
while (lo < hi)
{
    int mid = lo + (hi - lo) / 2;
    if (Predicate(mid)) hi = mid;           // mid might be the answer
    else lo = mid + 1;                      // answer is to the right of mid
}
return lo;
```

- First index with `array[i] >= target` → insertion position / first occurrence.
- First index with `array[i] > target` → one past the last occurrence.

### Binary search on the answer

If you can answer "is `x` good enough?" and the answer is **monotonic** (once good, every bigger `x` is also good),
binary search over `x` directly — even when there's no array. Example: the minimum eating speed such that all piles
are finished in `h` hours.

### Complexity

| Search | Time | Space |
|---|---|---|
| Linear search | O(n) | O(1) |
| Binary search (iterative) | O(log n) | O(1) |
| Binary search on the answer over range `R` with an O(n) check | O(n log R) | O(1) |

.NET provides `Array.BinarySearch` and `List<T>.BinarySearch` (they return the bitwise complement of the insertion
point when the value is missing). Don't use them in these exercises.

### Common pitfalls

- Infinite loops: with `while (lo < hi)` and `mid` rounding down, always make `lo = mid + 1` in the "go right" branch.
- `(lo + hi) / 2` overflows for huge ranges — use `lo + (hi - lo) / 2`.
- `mid * mid` overflows when searching square roots — use `long` or compare `mid <= x / mid`.
- Mixing up inclusive `[lo, hi]` and half-open `[lo, hi)` conventions within the same loop.

## Exercises

### Exercise 01 — Classic binary search
File: `Exercise01_ClassicBinarySearch.cs`

```csharp
public static int IndexOf(int[] sorted, int target)
```

`sorted` contains distinct values in ascending order. Return the index of `target`, or `-1`.

| Input | Output |
|---|---|
| `[1, 3, 5, 7, 9, 11]`, `7` | `3` |
| `[1, 3, 5, 7, 9, 11]`, `4` | `-1` |

- Throw `ArgumentNullException` for `null`. Don't use `Array.BinarySearch`.
- **Target: O(log n).**

### Exercise 02 — First and last position
File: `Exercise02_FirstAndLastPosition.cs`

```csharp
public static (int First, int Last) Find(int[] sorted, int target)
```

`sorted` is ascending and may contain duplicates. Return the first and last index of `target`, or `(-1, -1)`.

| Input | Output |
|---|---|
| `[5, 7, 7, 8, 8, 10]`, `8` | `(3, 4)` |
| `[5, 7, 7, 8, 8, 10]`, `6` | `(-1, -1)` |
| `[2, 2, 2, 2]`, `2` | `(0, 3)` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(log n)** even when the value repeats a million times (don't walk outward from a match).

### Exercise 03 — Search insert position
File: `Exercise03_SearchInsert.cs`

```csharp
public static int Position(int[] sorted, int target)
```

`sorted` contains distinct values in ascending order. Return the index of `target` if present; otherwise the index
where it would be inserted to keep the order.

| Input | Output |
|---|---|
| `[1, 3, 5, 6]`, `5` | `2` |
| `[1, 3, 5, 6]`, `2` | `1` |
| `[1, 3, 5, 6]`, `7` | `4` |
| `[1, 3, 5, 6]`, `0` | `0` |

- Throw `ArgumentNullException` for `null`. **Target: O(log n).**

### Exercise 04 — Integer square root
File: `Exercise04_IntegerSqrt.cs`

```csharp
public static long Floor(long x)
```

Return the largest integer `r` such that `r * r <= x`, without `Math.Sqrt` or floating point.

| Input | Output |
|---|---|
| `8` | `2` |
| `16` | `4` |
| `1_000_000_000_000_000_000` | `1_000_000_000` |
| `long.MaxValue` | `3_037_000_499` |

- Throw `ArgumentOutOfRangeException` for negative `x`.
- **Target: O(log x).** Watch out for overflow when squaring.

### Exercise 05 — Search in a rotated sorted array
File: `Exercise05_SearchRotated.cs`

```csharp
public static int IndexOf(int[] rotated, int target)
```

A sorted array of **distinct** values was rotated at an unknown pivot (e.g. `[0, 1, 2, 4, 5, 6, 7]` became
`[4, 5, 6, 7, 0, 1, 2]`). Return the index of `target` or `-1`.

| Input | Output |
|---|---|
| `[4, 5, 6, 7, 0, 1, 2]`, `0` | `4` |
| `[4, 5, 6, 7, 0, 1, 2]`, `3` | `-1` |

- Throw `ArgumentNullException` for `null`. **Target: O(log n).**

<details><summary>Hint</summary>

At every step, at least one half (`[lo, mid]` or `[mid, hi]`) is sorted. Check whether the target lies inside the
sorted half's range; if so search there, otherwise search the other half.
</details>

### Exercise 06 — Find a peak element
File: `Exercise06_FindPeak.cs`

```csharp
public static int Index(int[] numbers)
```

A peak is an element strictly greater than its neighbours; positions outside the array count as `-∞`.
Adjacent elements are never equal. Return the index of **any** peak.

| Input | Output |
|---|---|
| `[1, 2, 3, 1]` | `2` |
| `[1, 2, 1, 3, 5, 6, 4]` | `1` or `5` |

- Throw `ArgumentNullException` for `null` and `ArgumentException` for an empty array.
- **Target: O(log n).**

<details><summary>Hint</summary>

If `numbers[mid] < numbers[mid + 1]`, you're on an uphill slope, so a peak must exist to the right. Otherwise one
exists at `mid` or to its left.
</details>

### Exercise 07 — Minimum in a rotated sorted array
File: `Exercise07_MinInRotated.cs`

```csharp
public static int Find(int[] rotated)
```

| Input | Output |
|---|---|
| `[3, 4, 5, 1, 2]` | `1` |
| `[11, 13, 15, 17]` | `11` |

- Values are distinct. Throw `ArgumentNullException` for `null` and `ArgumentException` for an empty array.
- **Target: O(log n).**

<details><summary>Hint</summary>

Compare `rotated[mid]` with `rotated[hi]`: if it's bigger, the minimum is to the right of `mid`; otherwise it's at
`mid` or to the left.
</details>

### Exercise 08 — Minimum eating speed (binary search on the answer)
File: `Exercise08_EatingSpeed.cs`

```csharp
public static int Minimum(int[] piles, int hours)
```

Koko eats bananas at speed `k` bananas per hour: each hour she picks one pile and eats `k` bananas from it (or the
whole pile if it has fewer). Return the smallest integer `k` that lets her finish all piles within `hours` hours.

| Input | Output |
|---|---|
| `[3, 6, 7, 11]`, `8` | `4` |
| `[30, 11, 23, 4, 20]`, `5` | `30` |
| `[30, 11, 23, 4, 20]`, `6` | `23` |

- Every pile has at least one banana. Throw `ArgumentNullException` for `null`, and `ArgumentException` when
  `piles` is empty or `hours < piles.Length` (impossible).
- **Target: O(n log max(pile)).** Trying speeds 1, 2, 3, … will time out.

<details><summary>Hint</summary>

Hours needed at speed `k` = sum of `ceil(pile / k)` = `(pile + k - 1) / k`. Use `long` for the sum. This is monotonic
in `k`, so binary search `k` over `[1, max(piles)]` for the first speed that fits.
</details>

## Running the tests

```bash
dotnet test 11-BinarySearch/BinarySearch.Tests
dotnet test 11-BinarySearch/BinarySearch.Tests --filter "FullyQualifiedName~Exercise08"
```
