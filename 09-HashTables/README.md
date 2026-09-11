# Lesson 09 — Hash Tables

## Learning goals

- Explain how a hash table turns a key into an array index, and why lookups are O(1) **on average**.
- Handle **collisions** with separate chaining and keep chains short by **resizing**.
- Know the `Equals` / `GetHashCode` contract in .NET.
- Recognize the hash-map patterns: counting, "have I seen the complement?", grouping by a canonical key,
  prefix sums, and combining a map with a linked list (LRU cache).

## The concept

A hash table stores key/value pairs in an array of **buckets**. To find the bucket of a key:

```
index = (hash(key) & 0x7FFFFFFF) % buckets.Length
```

```
key "cat" ──hash──▶ 1 950 437 ──mod 8──▶ bucket 5

buckets
  0 │ ·
  1 │ ("dog", 3)
  2 │ ·
  3 │ ("owl", 9) ──▶ ("emu", 4)      ← collision: two keys in the same bucket (a chain)
  4 │ ·
  5 │ ("cat", 7)
  6 │ ·
  7 │ ("ant", 1)
```

If the hash function spreads keys evenly and the table isn't too full, each bucket holds very few entries, so
`Put`, `Get` and `Remove` only look at a handful of items — **O(1) on average**.

### Collisions

Two different keys can land in the same bucket. Two classic strategies:

- **Separate chaining** — each bucket holds a small linked list (or list) of entries. *Used in this lesson.*
- **Open addressing** — on collision, probe the next slots (`index + 1`, `index + 2`, …) until a free one is found.

### Load factor and resizing

`load factor = Count / BucketCount`. As it grows, chains get longer and operations slow down. When it exceeds a
threshold (0.75 here), allocate twice as many buckets and **re-insert every entry** (their indices change because
the modulus changed). Like a dynamic array, this makes insertion amortized O(1).

### Operations

| Operation | Average | Worst case (all keys collide) |
|---|---|---|
| `Put` / `Get` / `ContainsKey` / `Remove` | O(1) | O(n) |
| Iterate all entries | O(n + buckets) | O(n + buckets) |

.NET provides `Dictionary<TKey, TValue>` and `HashSet<T>`.

### `Equals` and `GetHashCode`

A hash table relies on two rules:

1. If `a.Equals(b)`, then `a.GetHashCode() == b.GetHashCode()`.
2. A key's hash code must not change while it's stored in the table (don't mutate keys!).

`record` types and tuples implement both for you. Use `EqualityComparer<TKey>.Default` inside generic code.

### Common patterns

| Pattern | Example |
|---|---|
| Count occurrences | word frequencies, anagram checks |
| Seen set / complement lookup | two sum: have I seen `target - x`? |
| Canonical key | group anagrams by their sorted letters |
| Prefix sums + map | count subarrays with sum `k` |
| Map + linked list | LRU cache with O(1) get and eviction |

### Common pitfalls

- `Math.Abs(int.MinValue)` is still negative — mask the sign bit instead: `hash & 0x7FFFFFFF`.
- Forgetting to rehash when resizing (entries become unreachable).
- Integer overflow when computing `target - x`.

## Exercises

### Exercise 01 — Build a hash map
File: `Exercise01_MyHashMap.cs`

```csharp
public const int InitialBucketCount = 16;
public const double MaxLoadFactor = 0.75;

public int Count { get; }
public int BucketCount { get; }
public void Put(TKey key, TValue value)
public TValue Get(TKey key)
public bool TryGet(TKey key, out TValue value)
public bool ContainsKey(TKey key)
public bool Remove(TKey key)
```

- Use **separate chaining** over an array of buckets. Don't use `Dictionary`, `HashSet` or other hash-based collections.
- `Put` adds the key or replaces its value.
- After adding a **new** key, if `Count > BucketCount * MaxLoadFactor`, double the bucket count and rehash.
  (So: 12 keys → 16 buckets, 13 keys → 32 buckets.)
- `Get` throws `KeyNotFoundException` for a missing key. All methods throw `ArgumentNullException` for a `null` key.
- Hash codes can be negative (even `int.MinValue`).
- **Target: O(1) average** per operation.

### Exercise 02 — Two sum
File: `Exercise02_TwoSum.cs`

```csharp
public static (int First, int Second)? Find(int[] numbers, int target)
```

Return indices `First < Second` with `numbers[First] + numbers[Second] == target`, or `null` if no such pair exists.
Any valid pair is accepted.

| Input | Output |
|---|---|
| `[2, 7, 11, 15]`, `9` | `(0, 1)` |
| `[3, 2, 4]`, `6` | `(1, 2)` |
| `[3]`, `6` | `null` |

- Throw `ArgumentNullException` for `null`. Watch out for overflow.
- **Target: O(n).**

### Exercise 03 — Group anagrams
File: `Exercise03_GroupAnagrams.cs`

```csharp
public static IList<IList<string>> Group(string[] words)
```

Group words that are anagrams of each other. The order of groups and of words inside a group doesn't matter.

| Input | Output |
|---|---|
| `["eat", "tea", "tan", "ate", "nat", "bat"]` | `[["eat", "tea", "ate"], ["tan", "nat"], ["bat"]]` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n · k log k)** where `k` is the word length.

<details><summary>Hint</summary>

Anagrams share the same letters once sorted: use `new string(word.Order().ToArray())` (or a letter-count signature)
as a dictionary key.
</details>

### Exercise 04 — Longest consecutive sequence
File: `Exercise04_LongestConsecutiveSequence.cs`

```csharp
public static int Length(int[] numbers)
```

Return the length of the longest run of consecutive integers that can be formed from the values (in any order).

| Input | Output |
|---|---|
| `[100, 4, 200, 1, 3, 2]` | `4` — `1, 2, 3, 4` |
| `[0, 3, 7, 2, 5, 8, 4, 6, 0, 1]` | `9` |

- Throw `ArgumentNullException` for `null`. Don't let `int.MaxValue + 1` wrap around to `int.MinValue`.
- **Target: O(n)** without sorting.

<details><summary>Hint</summary>

Put everything in a `HashSet<int>`. Only start counting from `x` when `x - 1` is **not** in the set — so each run
is walked once.
</details>

### Exercise 05 — Subarray sum equals k
File: `Exercise05_SubarraySumEqualsK.cs`

```csharp
public static long Count(int[] numbers, int k)
```

Count the contiguous subarrays whose elements add up to `k`. Values may be negative.

| Input | Output |
|---|---|
| `[1, 1, 1]`, `k = 2` | `2` |
| `[1, 2, 3]`, `k = 3` | `2` |
| `[1, -1, 0]`, `k = 0` | `3` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n).**

<details><summary>Hint</summary>

If `prefix[j] - prefix[i] == k`, the subarray `(i, j]` sums to `k`. Walk the array keeping a dictionary
"prefix sum → how many times seen", starting with `{0: 1}`.
</details>

### Exercise 06 — Isomorphic strings
File: `Exercise06_IsomorphicStrings.cs`

```csharp
public static bool AreIsomorphic(string first, string second)
```

Two strings are isomorphic if the characters of `first` can be replaced to get `second`, where every occurrence of a
character is replaced by the same character and no two characters map to the same one.

| Input | Output |
|---|---|
| `"egg"`, `"add"` | `true` |
| `"foo"`, `"bar"` | `false` |
| `"badc"`, `"baba"` | `false` |

- Throw `ArgumentNullException` if either is `null`.

### Exercise 07 — Top k frequent words
File: `Exercise07_TopKFrequentWords.cs`

```csharp
public static IList<string> TopK(string[] words, int k)
```

Return the `k` most frequent words, ordered by frequency (highest first); words with the same frequency are ordered
alphabetically using **ordinal** comparison.

| Input | Output |
|---|---|
| `["i", "love", "leetcode", "i", "love", "coding"]`, `k = 2` | `["i", "love"]` |
| `["the", "day", "is", "sunny", "the", "the", "the", "sunny", "is", "is"]`, `k = 4` | `["the", "is", "sunny", "day"]` |

- If `k` exceeds the number of distinct words, return all of them.
- Throw `ArgumentNullException` for `null` and `ArgumentOutOfRangeException` for a negative `k`.

### Exercise 08 — LRU cache
File: `Exercise08_LruCache.cs`

```csharp
public LruCache(int capacity)
public int Count { get; }
public bool TryGet(int key, out int value)
public void Put(int key, int value)
```

A cache with a fixed capacity that evicts the **least recently used** entry when full.

- `TryGet` returns the value and marks the key as most recently used.
- `Put` inserts or updates the value and marks the key as most recently used. If inserting a new key exceeds the
  capacity, evict the least recently used key first.
- The constructor throws `ArgumentOutOfRangeException` when `capacity < 1`.
- **Target: O(1)** for both operations.

```
capacity 2
Put(1, 1)          cache: [1]
Put(2, 2)          cache: [2, 1]           (most recent first)
TryGet(1) → 1      cache: [1, 2]
Put(3, 3)          cache: [3, 1]           evicts 2
TryGet(2) → false
```

<details><summary>Hint</summary>

Combine a `Dictionary<int, LinkedListNode<(int Key, int Value)>>` with a `LinkedList<(int Key, int Value)>`.
Move a node to the front on every access; evict from the back.
</details>

## Running the tests

```bash
dotnet test 09-HashTables/HashTables.Tests
dotnet test 09-HashTables/HashTables.Tests --filter "FullyQualifiedName~Exercise08"
```
