# Lesson 01 — Algorithm Complexity (Big-O)

## Learning goals

- Describe how the running time and memory of an algorithm grow with the input size `n`.
- Read a piece of code and classify it as O(1), O(log n), O(√n), O(n), O(n log n), O(n²) or O(2ⁿ).
- Trade memory for speed (hash sets, prefix sums) to turn an O(n²) solution into O(n).

## The concept

When we say an algorithm is **O(f(n))** we describe an *upper bound on how its cost grows* as the input
gets bigger, ignoring constant factors and smaller terms. `3n² + 10n + 7` is simply **O(n²)**: when `n`
doubles, the work roughly quadruples, and that is what matters for large inputs.

```
operations
    ▲                                    O(2ⁿ)   O(n²)
    │                                    │      /
    │                                    │    /
    │                                    │  /         O(n log n)
    │                                   │ /       ___/
    │                                  │/    ___/      O(n)
    │                                 /  __/     _____/
    │                              _/_/    _____/
    │                         ___/___------          O(√n)
    │               ____----‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾ O(log n)
    │═══════════════════════════════════════════════ O(1)
    └───────────────────────────────────────────────▶ n
```

### How to read code

| Pattern | Complexity |
|---|---|
| A fixed number of statements, array index, dictionary lookup | O(1) |
| A loop that halves (or doubles) its variable each step | O(log n) |
| A loop while `i * i <= n` | O(√n) |
| One loop over the input | O(n) |
| Two loops **one after the other** | O(n) + O(n) = O(n) |
| A loop over the input with a halving loop inside | O(n log n) |
| Two loops **nested** over the input | O(n²) |
| A loop whose bound is a constant (`for i < 100`) | O(1) — it does not grow with `n` |
| A recursive function that calls itself twice with `n - 1` | O(2ⁿ) |

### Space complexity

Space complexity measures the **extra** memory an algorithm allocates. Reversing an array in place is O(1)
extra space; copying it into a `HashSet<int>` is O(n).

### How big is "too slow"?

A modern machine does roughly 10⁸–10⁹ simple operations per second.

| n | O(n log n) | O(n²) |
|---|---|---|
| 1 000 | 10 000 | 1 000 000 |
| 100 000 | 1 700 000 | 10 000 000 000 (seconds to minutes) |
| 1 000 000 | 20 000 000 | 10¹² (hours) |

That is why the tests in this lesson use large inputs: an O(n²) solution will not finish in time.

### Common pitfalls

- **Hidden loops**: `list.Contains(x)`, `list.IndexOf(x)`, `list.Insert(0, x)`, `string + string` inside a loop
  and LINQ calls like `.Count()` on an `IEnumerable` are all O(n).
- **Integer overflow**: `int + int` can overflow silently. Use `long` when sums can exceed ~2.1 billion.
- **Best vs worst case**: Big-O usually refers to the worst case unless stated otherwise.

## Exercises

Implement each method in `Complexity.Exercises`. Every method currently throws `NotImplementedException`.

### Exercise 01 — Count pairs with a given sum
File: `Exercise01_CountPairsWithSum.cs`

```csharp
public static long Count(int[] numbers, int target)
```

Return how many index pairs `(i, j)` with `i < j` satisfy `numbers[i] + numbers[j] == target`.

| Input | Output |
|---|---|
| `[1, 2, 3, 4]`, target `5` | `2` — (1,4) and (2,3) |
| `[1, 1, 1, 1]`, target `2` | `6` |
| `[]`, target `5` | `0` |

- Throw `ArgumentNullException` when `numbers` is `null`.
- Values are in `[-10⁹, 10⁹]` — beware of overflow when adding two of them.
- **Target: O(n) time**, O(n) space. The obvious nested loop is O(n²) and will time out.

<details><summary>Hint</summary>

Walk the array once. For each value `x`, the number of earlier values equal to `target - x` is exactly
the number of new pairs `x` completes. Keep those counts in a `Dictionary<long, int>`.
</details>

### Exercise 02 — Contains duplicate
File: `Exercise02_ContainsDuplicate.cs`

```csharp
public static bool HasDuplicate(int[] numbers)
```

Return `true` if any value appears at least twice.

| Input | Output |
|---|---|
| `[1, 2, 3, 1]` | `true` |
| `[1, 2, 3, 4]` | `false` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n) time.**

<details><summary>Hint</summary>

`HashSet<int>.Add` returns `false` when the value is already present.
</details>

### Exercise 03 — Range sum queries (prefix sums)
File: `Exercise03_RangeSumQuery.cs`

```csharp
public RangeSumQuery(int[] numbers)
public long SumRange(int left, int right)   // inclusive
```

The constructor receives the data once; afterwards many `SumRange` queries are answered.

| numbers | Query | Output |
|---|---|---|
| `[-2, 0, 3, -5, 2, -1]` | `SumRange(0, 2)` | `1` |
| | `SumRange(2, 5)` | `-1` |
| | `SumRange(0, 5)` | `-3` |

- The constructor throws `ArgumentNullException` for `null`.
- `SumRange` throws `ArgumentOutOfRangeException` when `left < 0`, `right >= length` or `left > right`.
- **Target: O(n) constructor, O(1) per query.**

<details><summary>Hint</summary>

Precompute `prefix[i] = numbers[0] + ... + numbers[i - 1]` (with `prefix[0] = 0`).
Then `SumRange(l, r) = prefix[r + 1] - prefix[l]`.
</details>

### Exercise 04 — Classify the complexity
File: `Exercise04_ComplexityQuiz.cs`

Each method `SnippetA()` … `SnippetI()` must **return** the `BigO` value that describes the time complexity of
the snippet below (as a function of `n`, the input size). The snippets are also copied into the source file.

```csharp
// A
int First(int[] items) => items[0];

// B
int CountHalvings(int n) { int steps = 0; while (n > 1) { n /= 2; steps++; } return steps; }

// C
long Sum(int[] items) { long total = 0; foreach (var x in items) total += x; return total; }

// D
int CountEqualPairs(int[] items)
{
    int count = 0;
    for (int i = 0; i < items.Length; i++)
        for (int j = 0; j < items.Length; j++)
            if (i != j && items[i] == items[j]) count++;
    return count;
}

// E
long Work(int n)
{
    long work = 0;
    for (int i = 0; i < n; i++)
        for (int j = 1; j < n; j *= 2) work++;
    return work;
}

// F
long Fib(int n) => n < 2 ? n : Fib(n - 1) + Fib(n - 2);

// G
int Range(int[] items)
{
    int max = int.MinValue, min = int.MaxValue;
    foreach (var x in items) max = Math.Max(max, x);
    foreach (var x in items) min = Math.Min(min, x);
    return max - min;
}

// H
long Work(int n)
{
    long work = 0;
    for (int i = 0; i < n; i++)
        for (int j = 0; j < 100; j++) work++;
    return work;
}

// I
bool HasDivisor(int n)
{
    for (int d = 2; d * d <= n; d++) if (n % d == 0) return true;
    return false;
}
```

Try to answer before looking at the tests!

### Exercise 05 — Fast modular exponentiation
File: `Exercise05_FastModPower.cs`

```csharp
public static long Pow(long baseValue, long exponent, long modulus)
```

Return `(baseValue ^ exponent) mod modulus`.

| Input | Output |
|---|---|
| `Pow(2, 10, 1000)` | `24` |
| `Pow(3, 0, 7)` | `1` |
| `Pow(5, 3, 1)` | `0` |

- Constraints: `baseValue >= 0`, `exponent >= 0`, `1 <= modulus <= int.MaxValue`.
  Throw `ArgumentOutOfRangeException` otherwise.
- Don't use `BigInteger` or `Math.Pow`.
- **Target: O(log exponent).** Exponents can be as large as `long.MaxValue`.

<details><summary>Hint</summary>

Exponentiation by squaring: `x^e = (x²)^(e/2)` when `e` is even and `x · x^(e-1)` when it's odd.
Reduce modulo `modulus` after every multiplication so the numbers never overflow a `long`.
</details>

### Exercise 06 — Missing number
File: `Exercise06_MissingNumber.cs`

```csharp
public static int Find(int[] numbers)
```

`numbers` contains `n` **distinct** values taken from the range `[0, n]`, so exactly one value of that range is
missing. Return it.

| Input | Output |
|---|---|
| `[3, 0, 1]` | `2` |
| `[0, 1]` | `2` |
| `[]` | `0` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n) time and O(1) extra space.**

<details><summary>Hint</summary>

The sum of `0..n` is `n(n+1)/2`. Subtract the actual sum (use `long`!). XOR works too.
</details>

### Exercise 07 — Is prime?
File: `Exercise07_PrimeCheck.cs`

```csharp
public static bool IsPrime(long n)
```

| Input | Output |
|---|---|
| `2` | `true` |
| `1` | `false` |
| `91` | `false` (7 × 13) |
| `999_999_999_989` | `true` |

- Numbers less than 2 are not prime.
- **Target: O(√n).**

<details><summary>Hint</summary>

If `n = a · b` with `a <= b`, then `a <= √n`. You only need to test divisors while `d * d <= n`.
</details>

## Running the tests

```bash
dotnet test 01-Complexity/Complexity.Tests
dotnet test 01-Complexity/Complexity.Tests --filter "FullyQualifiedName~Exercise03"
```
