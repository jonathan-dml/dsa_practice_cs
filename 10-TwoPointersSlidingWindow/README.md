# Lesson 10 — Two Pointers and Sliding Window

## Learning goals

- Replace nested loops with **two indices** that each move in one direction only, turning O(n²) into O(n).
- Use **opposite-end pointers** on sorted data.
- Use **fixed-size** and **variable-size sliding windows** over arrays and strings.
- Recognize when these techniques apply (and when they don't).

## The concept

Many brute-force solutions look at every pair `(i, j)` — O(n²). If we can argue that some pairs never need to be
checked, two indices that only move **forward** (or toward each other) visit each position at most once or twice: O(n).

### Pattern 1 — Opposite ends (sorted input)

```
sorted:   [1, 2, 4, 7, 11, 15]     target = 15
           ▲                 ▲
          left             right       1 + 15 = 16 > 15 → right--

           ▲             ▲
          left         right           1 + 11 = 12 < 15 → left++

              ▲          ▲
             left      right           2 + 11 = 13 < 15 → left++

                 ▲       ▲
                left   right           4 + 11 = 15 ✔
```

Because the array is sorted, when the sum is too large, *no* pair using `right` with anything ≥ `left` can work,
so `right` can be discarded forever — and symmetrically for `left`.

### Pattern 2 — Fixed-size window

To examine every block of `k` consecutive elements, don't recompute each block from scratch. Slide the window:
add the element entering on the right, subtract the element leaving on the left.

```
[2, 1, 5, 1, 3, 2]   k = 3
 └──────┘            sum = 8
    └──────┘         sum = 8 - 2 + 1 = 7
       └──────┘      sum = 7 - 1 + 3 = 9
```

### Pattern 3 — Variable-size window

Grow the window by moving `right`; while the window violates (or satisfies) a condition, shrink it by moving `left`.

```csharp
int left = 0;
for (int right = 0; right < n; right++)
{
    // add items[right] to the window state
    while (/* window is invalid */)
    {
        // remove items[left] from the window state
        left++;
    }
    // window [left, right] is valid → update the answer
}
```

Each index enters and leaves the window at most once → O(n), even though there is a nested `while`.

### When does it work?

The key requirement is **monotonicity**: extending the window must only make it "more invalid" (e.g. with positive
numbers, a longer window has a bigger sum), and shrinking must only make it "more valid". With negative numbers,
the "sum ≥ target" window trick breaks — use prefix sums and a hash map instead (Lesson 09).

### Common pitfalls

- Using opposite-end pointers on **unsorted** data.
- Forgetting to skip duplicates when unique results are required (3Sum).
- In "longest substring without repeating characters", moving `left` backwards when you jump it to a
  previously-seen position (`"abba"`!). Always use `left = Math.Max(left, lastSeen + 1)`.
- Overflow in area or sum calculations — use `long`.

## Exercises

### Exercise 01 — Pair with target sum (sorted)
File: `Exercise01_PairSumSorted.cs`

```csharp
public static (int Left, int Right)? FindPair(int[] sorted, int target)
```

`sorted` is in ascending order. Return indices `Left < Right` whose values add up to `target`, or `null`.
Any valid pair is accepted.

| Input | Output |
|---|---|
| `[2, 5, 9, 11]`, `11` | `(0, 2)` |
| `[1, 2]`, `10` | `null` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n) time, O(1) space** — no hash set.

### Exercise 02 — 3Sum
File: `Exercise02_ThreeSum.cs`

```csharp
public static IList<(int A, int B, int C)> FindTriplets(int[] numbers)
```

Return all **unique** triplets of values (from three different positions) that add up to 0. Each triplet must be in
ascending order (`A <= B <= C`); the order of the triplets in the list doesn't matter.

| Input | Output |
|---|---|
| `[-1, 0, 1, 2, -1, -4]` | `[(-1, -1, 2), (-1, 0, 1)]` |
| `[0, 0, 0, 0]` | `[(0, 0, 0)]` |
| `[0, 1, 1]` | `[]` |

- Don't modify the input array. Throw `ArgumentNullException` for `null`.
- **Target: O(n²).** The triple loop is O(n³) and will time out.

<details><summary>Hint</summary>

Sort a copy. For each index `i` (skipping values equal to the previous one), run Exercise 01's two-pointer search on the
rest of the array for `-numbers[i]`, skipping duplicates after each match.
</details>

### Exercise 03 — Container with most water
File: `Exercise03_ContainerWithMostWater.cs`

```csharp
public static long MaxArea(int[] heights)
```

`heights[i]` is a vertical line at position `i`. Choose two lines that, together with the x-axis, hold the most water:
`area = (j - i) * min(heights[i], heights[j])`.

| Input | Output |
|---|---|
| `[1, 8, 6, 2, 5, 4, 8, 3, 7]` | `49` |
| `[1, 1]` | `1` |
| `[5]` | `0` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n).**

<details><summary>Hint</summary>

Start with the widest container. The shorter line limits the area, so moving the *taller* line inward can never help —
always move the shorter one.
</details>

### Exercise 04 — Maximum sum of a window of size k
File: `Exercise04_MaxSumSubarray.cs`

```csharp
public static long MaxSumOfSizeK(int[] numbers, int k)
```

| Input | Output |
|---|---|
| `[2, 1, 5, 1, 3, 2]`, `k = 3` | `9` |
| `[-1, -2, -3]`, `k = 2` | `-3` |

- Throw `ArgumentNullException` for `null` and `ArgumentOutOfRangeException` when `k < 1` or `k > numbers.Length`.
- **Target: O(n).**

### Exercise 05 — Longest substring without repeating characters
File: `Exercise05_LongestUniqueSubstring.cs`

```csharp
public static int Length(string text)
```

| Input | Output |
|---|---|
| `"abcabcbb"` | `3` — `"abc"` |
| `"bbbbb"` | `1` |
| `"pwwkew"` | `3` — `"wke"` |
| `"abba"` | `2` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n).**

<details><summary>Hint</summary>

Keep a `Dictionary<char, int>` with the last index of each character. When `text[right]` was seen at an index
`>= left`, move `left` to one past that index.
</details>

### Exercise 06 — Minimum size subarray sum
File: `Exercise06_MinSubarrayLength.cs`

```csharp
public static int MinLength(int[] positives, int target)
```

All values are positive. Return the length of the shortest contiguous subarray whose sum is **at least** `target`,
or `0` if there is none.

| Input | Output |
|---|---|
| `[2, 3, 1, 2, 4, 3]`, `7` | `2` — `[4, 3]` |
| `[1, 4, 4]`, `4` | `1` |
| `[1, 1, 1, 1]`, `11` | `0` |

- Throw `ArgumentNullException` for `null` and `ArgumentOutOfRangeException` when `target < 1`.
- **Target: O(n).**

### Exercise 07 — Sort colors (Dutch national flag)
File: `Exercise07_SortColors.cs`

```csharp
public static void Sort(int[] colors)
```

The array contains only `0`, `1` and `2`. Sort it **in place** in a single pass, without `Array.Sort` or counting.

| Before | After |
|---|---|
| `[2, 0, 2, 1, 1, 0]` | `[0, 0, 1, 1, 2, 2]` |

- Throw `ArgumentException` if a value other than 0, 1 or 2 is found, `ArgumentNullException` for `null`.

<details><summary>Hint</summary>

Three pointers: everything before `low` is 0, everything after `high` is 2, `mid` scans. Swap 0s to `low` (advance both),
swap 2s to `high` (only move `high` — the swapped-in value still needs checking), and step over 1s.
</details>

### Exercise 08 — Trapping rain water
File: `Exercise08_TrappingRainWater.cs`

```csharp
public static long Trap(int[] heights)
```

`heights` describes an elevation map where each bar has width 1. Compute how much water is trapped after raining.

```
heights = [0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1]

       █
   █≈≈≈██≈█
 █≈██≈██████         → 6 units of water (≈)
```

| Input | Output |
|---|---|
| `[0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1]` | `6` |
| `[4, 2, 0, 3, 2, 5]` | `9` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n) time, O(1) space.**

<details><summary>Hint</summary>

The water above bar `i` is `min(maxLeft, maxRight) - heights[i]`. With two pointers, whichever side has the smaller max
is the limiting side, so you can settle that bar and move its pointer inward.
</details>

## Running the tests

```bash
dotnet test 10-TwoPointersSlidingWindow/TwoPointersSlidingWindow.Tests
dotnet test 10-TwoPointersSlidingWindow/TwoPointersSlidingWindow.Tests --filter "FullyQualifiedName~Exercise05"
```
