# Lesson 02 — Recursion

## Learning goals

- Break a problem into a **base case** and a **recursive case**.
- Visualize the **call stack** and understand why deep recursion can cause a `StackOverflowException`.
- Speed up overlapping sub-problems with **memoization**.
- Generate combinations with **backtracking** (subsets, permutations).

## The concept

A recursive function solves a problem by calling itself on a *smaller* version of the same problem.
Every recursive function needs two parts:

1. **Base case** — an input small enough to answer directly (no further calls).
2. **Recursive case** — reduce the input, call yourself, and combine the result.

```csharp
static long Factorial(int n)
{
    if (n == 0) return 1;            // base case
    return n * Factorial(n - 1);     // recursive case
}
```

### The call stack

Each call gets its own *stack frame* with its parameters and local variables. Frames pile up until the
base case is reached, then they return one by one:

```
Factorial(3)                          returns 3 * 2 = 6
 └─ Factorial(2)                      returns 2 * 1 = 2
     └─ Factorial(1)                  returns 1 * 1 = 1
         └─ Factorial(0)  ← base case returns 1
```

The stack is limited (about 1 MB per thread by default). A recursion that goes 100 000 levels deep will
crash the process with a `StackOverflowException`, which **cannot be caught**. C# does not guarantee
tail-call optimization, so very deep recursions should be rewritten with a loop or an explicit `Stack<T>`.

### Memoization

The naive Fibonacci `F(n) = F(n-1) + F(n-2)` recomputes the same values again and again:

```
                 F(5)
            /           \
         F(4)           F(3)
        /    \          /   \
     F(3)    F(2)    F(2)   F(1)
    /   \    ...     ...
  F(2)  F(1)
```

That is O(2ⁿ) calls. Storing each computed answer in a dictionary or array (**memoization**) means each
`F(k)` is computed once, giving O(n).

### Backtracking

To enumerate all subsets or permutations, make a choice, recurse, then *undo* the choice:

```
choose(path):
    if path is complete: record a copy of path; return
    for each option still available:
        path.Add(option)       // choose
        choose(path)           // explore
        path.RemoveAt(last)    // un-choose
```

### Complexity cheat sheet

| Algorithm | Time | Extra space (stack) |
|---|---|---|
| Factorial, sum of digits | O(n) / O(digits) | O(n) |
| Naive Fibonacci | O(2ⁿ) | O(n) |
| Memoized Fibonacci | O(n) | O(n) |
| All subsets of n items | O(n · 2ⁿ) | O(n) |
| All permutations of n items | O(n · n!) | O(n) |
| Tower of Hanoi with n disks | O(2ⁿ) moves | O(n) |

### Common pitfalls

- Forgetting the base case (or never reaching it) → infinite recursion → stack overflow.
- Adding the *same* `List<T>` instance to the results in backtracking instead of a **copy**.
- Recomputing overlapping sub-problems — memoize!

## Exercises

All exercises should be solved **recursively** (the tests can't enforce it, but that's the point of the lesson).

### Exercise 01 — Factorial
File: `Exercise01_Factorial.cs`

```csharp
public static long Compute(int n)
```

| Input | Output |
|---|---|
| `0` | `1` |
| `5` | `120` |
| `20` | `2432902008176640000` |

- Valid inputs are `0..20` (21! does not fit in a `long`). Throw `ArgumentOutOfRangeException` otherwise.

### Exercise 02 — Fibonacci, naive and memoized
File: `Exercise02_Fibonacci.cs`

```csharp
public static long Naive(int n)
public static long Memoized(int n)
```

`F(0) = 0`, `F(1) = 1`, `F(n) = F(n-1) + F(n-2)`.

| Input | Output |
|---|---|
| `0` | `0` |
| `10` | `55` |
| `90` | `2880067194370816120` |

- Valid inputs are `0..92`. Throw `ArgumentOutOfRangeException` otherwise.
- `Naive` is the direct translation of the formula — O(2ⁿ).
- `Memoized` must cache results — **O(n)**. It's tested with `n = 92`, which the naive version could never finish.

<details><summary>Hint</summary>

Create a `long[] memo = new long[n + 1]` (or `Dictionary<int, long>`) and pass it to a private recursive helper.
Before computing `F(k)`, check whether it's already stored.
</details>

### Exercise 03 — Sum of digits
File: `Exercise03_SumOfDigits.cs`

```csharp
public static int Compute(long n)
```

| Input | Output |
|---|---|
| `0` | `0` |
| `123` | `6` |
| `-123` | `6` (the sign is ignored) |

- Inputs are greater than `long.MinValue`.

<details><summary>Hint</summary>

The last digit is `n % 10`; the rest of the number is `n / 10`.
</details>

### Exercise 04 — Reverse a string
File: `Exercise04_ReverseString.cs`

```csharp
public static string Reverse(string s)
```

| Input | Output |
|---|---|
| `"hello"` | `"olleh"` |
| `""` | `""` |

- Throw `ArgumentNullException` for `null`.

<details><summary>Hint</summary>

`Reverse(s) = Reverse(s[1..]) + s[0]`. For better performance, use a helper that swaps
`chars[left]` and `chars[right]` on a `char[]` and recurses with `left + 1, right - 1`.
</details>

### Exercise 05 — All subsets (power set)
File: `Exercise05_Subsets.cs`

```csharp
public static IList<IList<int>> Generate(int[] items)
```

Return every subset of `items` (which contains **distinct** values). The order of the subsets and the order of
the values inside each subset don't matter.

| Input | Output |
|---|---|
| `[]` | `[[]]` |
| `[1, 2]` | `[[], [1], [2], [1, 2]]` |
| `[1, 2, 3]` | 8 subsets |

- Throw `ArgumentNullException` for `null`.

<details><summary>Hint</summary>

For each item there are two choices: include it or not. Recurse on `index + 1` for both choices;
when `index == items.Length`, add a **copy** of the current subset to the results.
</details>

### Exercise 06 — Unique permutations
File: `Exercise06_Permutations.cs`

```csharp
public static IList<string> Generate(string s)
```

Return all **distinct** rearrangements of the characters of `s`, in any order.

| Input | Output |
|---|---|
| `"ab"` | `["ab", "ba"]` |
| `"aab"` | `["aab", "aba", "baa"]` |
| `""` | `[""]` |

- Throw `ArgumentNullException` for `null`.

<details><summary>Hint</summary>

Sort the characters first and keep a `bool[] used`. When building a position, skip a character if it equals
the previous one and the previous one is **not** currently used — that avoids duplicate permutations.
</details>

### Exercise 07 — Tower of Hanoi
File: `Exercise07_TowerOfHanoi.cs`

```csharp
public static IList<(char From, char To)> Solve(int disks, char from = 'A', char to = 'C', char via = 'B')
```

Move a tower of `disks` disks from peg `from` to peg `to`. Only one disk moves at a time and a larger disk may
never be placed on a smaller one. Return the list of moves.

| Input | Output |
|---|---|
| `Solve(1)` | `[(A, C)]` |
| `Solve(2)` | `[(A, B), (A, C), (B, C)]` |

- Valid inputs are `0..20`; throw `ArgumentOutOfRangeException` otherwise.
- The solution must use exactly `2ⁿ - 1` moves.

<details><summary>Hint</summary>

To move `n` disks from `from` to `to`: move `n - 1` disks to `via`, move the biggest disk to `to`,
then move the `n - 1` disks from `via` to `to`.
</details>

### Exercise 08 — Flatten a nested list
File: `Exercise08_FlattenNested.cs`

```csharp
public static IList<int> Flatten(IEnumerable<object> items)
```

Each element of `items` is either an `int` or another `IEnumerable<object>` (nested to any depth).
Return all the integers in order.

| Input | Output |
|---|---|
| `[1, [2, [3, 4]], 5]` | `[1, 2, 3, 4, 5]` |
| `[[], [[]]]` | `[]` |

- Throw `ArgumentNullException` when `items` is `null`.
- Throw `ArgumentException` when an element is neither an `int` nor an `IEnumerable<object>` (e.g. a `string`).

<details><summary>Hint</summary>

Use pattern matching: `if (item is int value) ... else if (item is IEnumerable<object> nested) ...`
</details>

## Running the tests

```bash
dotnet test 02-Recursion/Recursion.Tests
dotnet test 02-Recursion/Recursion.Tests --filter "FullyQualifiedName~Exercise05"
```
