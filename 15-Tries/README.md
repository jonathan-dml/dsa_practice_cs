# Lesson 15 — Tries (Prefix Trees)

## Learning goals

- Store a set of strings so that shared **prefixes** are stored only once.
- Implement insert, search and prefix queries in time proportional to the **length of the word**, not the number of words.
- Extend the basic trie with counters, deletion with pruning, sorted enumeration and wildcard search.

## The concept

A **trie** is a tree where each edge is labelled with a character. The path from the root to a node spells a prefix;
nodes where a word ends are **marked**.

```
words: car, card, care, cat, dog

             (root)
            /      \
           c        d
           |        |
           a        o
          / \       |
         r*  t*     g*
        / \
       d*  e*                 * = end of a word
```

Looking up `"care"` walks `c → a → r → e` and checks the mark. Checking whether *any* word starts with `"ca"` just walks
`c → a` — no need to look at the individual words.

### A node

```csharp
private sealed class Node
{
    public Dictionary<char, Node> Children { get; } = new();
    public bool IsWord { get; set; }
}
```

For lowercase English letters only, `Node[] children = new Node[26]` indexed by `c - 'a'` is faster and uses less
hashing. A `Dictionary<char, Node>` supports any character. Extra fields make many problems easy: a count of words
passing through the node, a count of words ending there, or the word itself.

### Operations

`L` is the length of the word or prefix.

| Operation | Trie | `HashSet<string>` | Sorted list + binary search |
|---|---|---|---|
| Insert | O(L) | O(L) average | O(n) (shifting) |
| Exact search | O(L) | O(L) average | O(L log n) |
| "Any word with prefix p?" | O(L) | O(n · L) | O(L log n) |
| All words with prefix, in order | O(L + output) | O(n · L + sort) | O(L log n + output) |

Memory is the price: every character can become a node object.

### Deleting a word

Unmark the end node, then walk back up removing nodes that no longer lead to any word (no children and not marked).
A recursive helper that returns "can this child be removed?" handles this neatly.

### Wildcard search

For a pattern like `"b.d"` where `.` matches any character, recurse: on a regular character follow that one child, on a
`.` try **every** child.

### Common pitfalls

- Treating a prefix as a word (`"app"` is not a word just because `"apple"` was inserted) — check the mark.
- Forgetting the empty string: it corresponds to the root node itself.
- Removing nodes that are still needed by other words when deleting.

## Exercises

Implement your own trie nodes in each exercise; don't use `HashSet`/`Dictionary` of whole words or `StartsWith` scans
as the main data structure (dictionaries *inside* nodes for children are fine).

### Exercise 01 — Trie
File: `Exercise01_Trie.cs`

```csharp
public void Insert(string word)
public bool Search(string word)
public bool StartsWith(string prefix)
```

```
Insert("apple")
Search("apple")    → true
Search("app")      → false
StartsWith("app")  → true
Insert("app")
Search("app")      → true
```

- Comparisons are case-sensitive; any characters are allowed. The empty string is a valid word.
- `StartsWith("")` is `true` if at least one word (possibly `""`) was inserted.
- All methods throw `ArgumentNullException` for `null`. **Target: O(L)** per operation.

### Exercise 02 — Prefix counter
File: `Exercise02_PrefixCounter.cs`

```csharp
public void Insert(string word)
public int CountWordsEqualTo(string word)
public int CountWordsStartingWith(string prefix)
public bool Erase(string word)
```

Like a trie, but it counts: inserting the same word twice counts twice. `Erase` removes **one** occurrence and returns
`false` if the word isn't present.

```
Insert("apple"), Insert("apple")
CountWordsEqualTo("apple")      → 2
CountWordsStartingWith("app")   → 2
Erase("apple")                  → true
CountWordsEqualTo("apple")      → 1
```

- All methods throw `ArgumentNullException` for `null`. **Target: O(L)** per operation.

<details><summary>Hint</summary>

Store two counters in every node: how many words **pass through** it and how many words **end** at it.
</details>

### Exercise 03 — Autocomplete
File: `Exercise03_Autocomplete.cs`

```csharp
public Autocomplete(IEnumerable<string> words)
public IList<string> Suggest(string prefix, int limit)
```

Return up to `limit` distinct words starting with `prefix`, in **ordinal** (character code) order.

```
words: car, card, care, cart, cat, dog
Suggest("car", 3)  → [car, card, care]
Suggest("ca", 10)  → [car, card, care, cart, cat]
Suggest("z", 5)    → []
```

- Duplicate input words appear once. Throw `ArgumentNullException` for `null` arguments and
  `ArgumentOutOfRangeException` for a negative `limit`.
- **Target: O(L + visited nodes)** — walk to the prefix node, then do a depth-first search visiting children in character
  order and stop as soon as `limit` words are collected.

### Exercise 04 — Trie with deletion
File: `Exercise04_DeletableTrie.cs`

```csharp
public int NodeCount { get; }
public bool Insert(string word)
public bool Contains(string word)
public bool Delete(string word)
```

- `Insert` returns `false` if the word was already present; `Delete` returns `false` if it wasn't.
- `NodeCount` is the number of nodes **excluding the root**. Deleting must **prune** nodes that no longer belong to any word.

```
Insert("apple"), Insert("app")   NodeCount → 5   (a, p, p, l, e)
Delete("apple")                  NodeCount → 3
Delete("app")                    NodeCount → 0
```

- All methods throw `ArgumentNullException` for `null`.

### Exercise 05 — Longest common prefix with a trie
File: `Exercise05_TrieLongestCommonPrefix.cs`

```csharp
public static string Find(string[] words)
```

| Input | Output |
|---|---|
| `["flower", "flow", "flight"]` | `"fl"` |
| `["dog", "racecar", "car"]` | `""` |
| `["abc", "ab"]` | `"ab"` |

- Solve it by inserting all words into a trie, then walking down from the root while the current node has exactly
  one child and doesn't mark the end of a word.
- An empty array returns `""`. Throw `ArgumentNullException` for `null`.

### Exercise 06 — Word dictionary with wildcards
File: `Exercise06_WordDictionary.cs`

```csharp
public void AddWord(string word)
public bool Search(string pattern)
```

`pattern` may contain `.` which matches exactly one arbitrary character.

```
AddWord("bad"), AddWord("dad"), AddWord("mad")
Search("pad")  → false
Search("bad")  → true
Search(".ad")  → true
Search("b..")  → true
Search("....") → false
```

- Both methods throw `ArgumentNullException` for `null`.

### Exercise 07 — Replace words with their roots
File: `Exercise07_ReplaceWords.cs`

```csharp
public static string Replace(IEnumerable<string> roots, string sentence)
```

Replace every word of the sentence that starts with a root by the **shortest** such root. Words are separated by single
spaces.

| Roots | Sentence | Output |
|---|---|---|
| `cat, bat, rat` | `"the cattle was rattled by the battery"` | `"the cat was rat by the bat"` |
| `a, b, c` | `"aadsfasf absbs bbab cadsfafs"` | `"a a b c"` |

- Throw `ArgumentNullException` for `null` arguments.
- **Target: O(total characters)** — for each word walk the trie and stop at the first node that ends a root.

## Running the tests

```bash
dotnet test 15-Tries/Tries.Tests
dotnet test 15-Tries/Tries.Tests --filter "FullyQualifiedName~Exercise04"
```
