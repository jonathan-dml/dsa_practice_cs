# Lesson 17 — Dynamic Programming

## Learning goals

- Recognize problems with **overlapping sub-problems** and **optimal substructure**.
- Define a DP **state**, a **recurrence**, **base cases** and an evaluation **order**.
- Implement DP top-down (memoization) and bottom-up (tabulation), and reduce memory by keeping only the rows you need.
- Solve the classic families: counting paths, min/max decisions, subsequences, knapsack, string edits.

## The concept

Dynamic programming is recursion plus **remembering answers**. If a problem can be split into smaller versions of
itself (optimal substructure) and those smaller versions repeat (overlapping sub-problems), compute each one **once**
and reuse it.

Lesson 02's Fibonacci is the smallest example: the naive recursion is O(2ⁿ); storing results makes it O(n).

### A recipe

1. **State** — what does `dp[i]` (or `dp[i][j]`) *mean*? Write it down in words.
   *"dp[i] = the fewest coins needed to make amount i."*
2. **Recurrence** — how is a state computed from smaller states?
   *"dp[i] = 1 + min(dp[i - coin]) over every coin ≤ i."*
3. **Base cases** — the states you know directly. *"dp[0] = 0."*
4. **Order** — compute states so that everything a state depends on is already known (usually increasing `i`).
5. **Answer** — which state holds the final result? *"dp[amount]."*

### Top-down vs bottom-up

```csharp
// Top-down: recursion + memo (natural to write, may hit deep recursion)
long Ways(int n) => n <= 1 ? 1 : memo[n] != 0 ? memo[n] : memo[n] = Ways(n - 1) + Ways(n - 2);

// Bottom-up: fill a table in dependency order (no recursion)
dp[0] = dp[1] = 1;
for (int i = 2; i <= n; i++) dp[i] = dp[i - 1] + dp[i - 2];
```

### Saving memory

If `dp[i]` depends only on the previous row or the last few values, keep just those. The 0/1 knapsack table
`dp[item][capacity]` becomes a single array iterated **from high capacity to low** so each item is used at most once.

### Two-dimensional example: longest common subsequence

`dp[i][j]` = LCS length of `a[..i]` and `b[..j]`.

```
            ""  a   c   e
        ""   0   0   0   0
        a    0   1   1   1        if a[i-1] == b[j-1]: dp[i][j] = dp[i-1][j-1] + 1
        b    0   1   1   1        else:               dp[i][j] = max(dp[i-1][j], dp[i][j-1])
        c    0   1   2   2
        d    0   1   2   2
        e    0   1   2   3   ← answer
```

### Classic problems and their complexity

| Problem | State | Time | Space (optimized) |
|---|---|---|---|
| Climbing stairs | ways to reach step i | O(n) | O(1) |
| Coin change (min coins) | fewest coins for amount i | O(n · amount) | O(amount) |
| House robber | best loot using houses ..i | O(n) | O(1) |
| Longest increasing subsequence | smallest tail of an increasing run of length k | O(n log n) | O(n) |
| Longest common subsequence | LCS of prefixes | O(n · m) | O(min(n, m)) |
| 0/1 knapsack | best value with capacity c | O(n · W) | O(W) |
| Edit distance | edits to turn prefix into prefix | O(n · m) | O(min(n, m)) |
| Unique paths in a grid | paths to cell (r, c) | O(rows · cols) | O(cols) |

### Common pitfalls

- An unclear state definition — most DP bugs come from not knowing exactly what `dp[i]` means.
- Wrong iteration direction in 1-D knapsack (low → high reuses items: that's the *unbounded* knapsack).
- Initializing "impossible" states with `int.MaxValue` and then adding 1 to them (overflow).
- Counting problems overflow quickly — use `long`.

## Exercises

### Exercise 01 — Climbing stairs
File: `Exercise01_ClimbingStairs.cs`

```csharp
public static long Ways(int steps)
```

You can climb 1 or 2 steps at a time. In how many distinct ways can you reach the top?

| Input | Output |
|---|---|
| `0` | `1` |
| `2` | `2` |
| `3` | `3` (1+1+1, 1+2, 2+1) |
| `10` | `89` |

- Valid inputs are `0..90`; throw `ArgumentOutOfRangeException` otherwise. **Target: O(n).**

### Exercise 02 — Coin change
File: `Exercise02_CoinChange.cs`

```csharp
public static int MinCoins(int[] coins, int amount)
public static long CountWays(int[] coins, int amount)
```

- `MinCoins` returns the fewest coins that add up to `amount` (unlimited coins of each value), or `-1` if impossible.
- `CountWays` returns how many **combinations** (order doesn't matter) add up to `amount`.

| Input | MinCoins | CountWays |
|---|---|---|
| `[1, 2, 5]`, `11` | `3` (5+5+1) | `11` |
| `[1, 2, 5]`, `5` | `1` | `4` |
| `[2]`, `3` | `-1` | `0` |
| any, `0` | `0` | `1` |

- Throw `ArgumentNullException` for `null`, `ArgumentOutOfRangeException` for a negative amount and `ArgumentException`
  for a coin value ≤ 0. **Target: O(coins · amount).**

<details><summary>Hint</summary>

For `CountWays`, loop over **coins in the outer loop** and amounts in the inner loop: `ways[a] += ways[a - coin]`.
Swapping the loops counts ordered sequences (permutations) instead.
</details>

### Exercise 03 — House robber
File: `Exercise03_HouseRobber.cs`

```csharp
public static long MaxLoot(int[] houses)
public static long MaxLootCircular(int[] houses)
```

You can't rob two adjacent houses. Return the maximum total. In `MaxLootCircular` the houses are arranged in a circle,
so the first and last houses are adjacent too.

| Input | MaxLoot | MaxLootCircular |
|---|---|---|
| `[1, 2, 3, 1]` | `4` | `4` |
| `[2, 7, 9, 3, 1]` | `12` | `11` |
| `[2, 3, 2]` | `4` | `3` |

- Throw `ArgumentNullException` for `null` and `ArgumentException` for negative amounts.
- **Target: O(n) time, O(1) space.**

<details><summary>Hint</summary>

`best(i) = max(best(i - 1), best(i - 2) + houses[i])`. For the circle, take the better of "skip the first house" and
"skip the last house".
</details>

### Exercise 04 — Longest increasing subsequence
File: `Exercise04_LongestIncreasingSubsequence.cs`

```csharp
public static int Length(int[] numbers)
```

Length of the longest **strictly** increasing subsequence (not necessarily contiguous).

| Input | Output |
|---|---|
| `[10, 9, 2, 5, 3, 7, 101, 18]` | `4` (2, 3, 7, 18) |
| `[0, 1, 0, 3, 2, 3]` | `4` |
| `[7, 7, 7, 7]` | `1` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n log n).** Start with the O(n²) DP, then optimize: the test with 200 000 values needs the faster version.

<details><summary>Hint</summary>

Keep `tails[k]` = the smallest possible tail of an increasing subsequence of length `k + 1`. For each value, binary
search the first tail ≥ value and replace it (or append if none). The answer is `tails.Count`.
</details>

### Exercise 05 — Longest common subsequence
File: `Exercise05_LongestCommonSubsequence.cs`

```csharp
public static int Length(string first, string second)
```

| Input | Output |
|---|---|
| `"abcde"`, `"ace"` | `3` |
| `"abc"`, `"def"` | `0` |

- Throw `ArgumentNullException` for `null`. **Target: O(n · m).**

### Exercise 06 — 0/1 knapsack
File: `Exercise06_Knapsack.cs`

```csharp
public static long MaxValue(int[] weights, int[] values, int capacity)
```

Each item can be taken at most once. Return the maximum total value with total weight ≤ `capacity`.

| Weights | Values | Capacity | Output |
|---|---|---|---|
| `[1, 3, 4, 5]` | `[1, 4, 5, 7]` | `7` | `9` |
| `[10, 20, 30]` | `[60, 100, 120]` | `50` | `220` |

- Throw `ArgumentNullException` for `null`, `ArgumentException` when the arrays have different lengths or contain negative
  numbers, and `ArgumentOutOfRangeException` for a negative capacity.
- **Target: O(n · capacity) time, O(capacity) space.**

### Exercise 07 — Edit distance
File: `Exercise07_EditDistance.cs`

```csharp
public static int Compute(string source, string target)
```

The minimum number of single-character insertions, deletions and substitutions that turn `source` into `target`.

| Input | Output |
|---|---|
| `"horse"`, `"ros"` | `3` |
| `"intention"`, `"execution"` | `5` |
| `"kitten"`, `"sitting"` | `3` |

- Throw `ArgumentNullException` for `null`. **Target: O(n · m).**

<details><summary>Hint</summary>

`dp[i][j]` = distance between `source[..i]` and `target[..j]`. If the last characters match, `dp[i][j] = dp[i-1][j-1]`;
otherwise `1 + min(dp[i-1][j] (delete), dp[i][j-1] (insert), dp[i-1][j-1] (substitute))`.
</details>

### Exercise 08 — Unique paths with obstacles
File: `Exercise08_UniquePaths.cs`

```csharp
public static long WithObstacles(int[][] grid)
```

A robot starts at the top-left cell and must reach the bottom-right cell, moving only **right** or **down**. Cells with
`1` are obstacles. Count the distinct paths.

```
0 0 0
0 1 0      → 2
0 0 0
```

- If the start or the end is an obstacle, the answer is `0`. An empty grid has `0` paths.
- Throw `ArgumentNullException` for `null` and `ArgumentException` for ragged rows or values other than 0 and 1.
- **Target: O(rows · cols).** The answer for a 30 × 30 empty grid is 30 067 266 499 541 040 — no brute force!

## Running the tests

```bash
dotnet test 17-DynamicProgramming/DynamicProgramming.Tests
dotnet test 17-DynamicProgramming/DynamicProgramming.Tests --filter "FullyQualifiedName~Exercise06"
```
