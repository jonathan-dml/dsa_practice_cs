# Lesson 04 — Strings

## Learning goals

- Understand that .NET strings are **immutable** arrays of UTF-16 `char`s, and what that costs.
- Build strings efficiently with `StringBuilder`.
- Solve text problems with character counting, two indices and single-pass parsing.

## The concept

A `string` in C# is a sequence of `char` values (UTF-16 code units) stored contiguously — conceptually a
read-only `char[]`. You can index it in O(1) (`s[i]`) and get its length in O(1) (`s.Length`).

```
"hello"
index:  0    1    2    3    4
      ┌────┬────┬────┬────┬────┐
      │ 'h'│ 'e'│ 'l'│ 'l'│ 'o'│
      └────┴────┴────┴────┴────┘
```

### Immutability

Strings can't be modified. Every "modification" creates a **new** string:

```csharp
string s = "";
for (int i = 0; i < n; i++)
    s += "x";          // copies the whole string every iteration → O(n²) total!
```

Use `StringBuilder` (a growable `char` buffer) when building a string piece by piece:

```csharp
var sb = new StringBuilder();
for (int i = 0; i < n; i++)
    sb.Append('x');    // amortized O(1)
string s = sb.ToString();
```

Alternatives: `new string(char[])` after editing a `char[]`, `string.Join`, `string.Concat`, or `string.Create`.

### Useful APIs

| API | Notes |
|---|---|
| `s.ToCharArray()` / `new string(chars)` | Convert to a mutable array and back |
| `char.IsLetterOrDigit(c)`, `char.IsWhiteSpace(c)` | Character classification |
| `char.ToLowerInvariant(c)` | Culture-independent case conversion |
| `c - 'a'` | Maps `'a'..'z'` to `0..25` (handy for `int[26]` counters) |
| `s.Split(' ', StringSplitOptions.RemoveEmptyEntries)` | Tokenize |
| `string.CompareOrdinal`, `StringComparer.Ordinal` | Fast, culture-independent comparison |

### Operations

| Operation | Time |
|---|---|
| `s[i]`, `s.Length` | O(1) |
| `s + t`, `s.Substring(...)` | O(n + m) — allocates a new string |
| `s.Contains(t)`, `s.IndexOf(t)` | O(n · m) worst case |
| `StringBuilder.Append` | amortized O(1) |

### Common pitfalls

- Concatenating in a loop (quadratic time).
- Culture-sensitive comparisons (`ToLower()` in Turkish turns `'I'` into `'ı'`). Prefer ordinal / invariant versions.
- Forgetting that `char` is a UTF-16 code unit: emoji and some symbols use **two** `char`s. The exercises here stick
  to simple characters.

## Exercises

### Exercise 01 — Reverse the words of a sentence
File: `Exercise01_ReverseWords.cs`

```csharp
public static string ReverseWords(string sentence)
```

Words are separated by one or more spaces. Return the words in reverse order joined by a **single** space,
with no leading or trailing spaces.

| Input | Output |
|---|---|
| `"the sky is blue"` | `"blue is sky the"` |
| `"  hello world  "` | `"world hello"` |
| `"a good   example"` | `"example good a"` |
| `"   "` | `""` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n).** Building the result with `+=` in a loop is O(n²) and will time out.

### Exercise 02 — Valid anagram
File: `Exercise02_IsAnagram.cs`

```csharp
public static bool IsAnagram(string first, string second)
```

Return `true` if `second` is a rearrangement of all the characters of `first` (case-sensitive).

| Input | Output |
|---|---|
| `"anagram"`, `"nagaram"` | `true` |
| `"rat"`, `"car"` | `false` |
| `"a"`, `"A"` | `false` |

- Throw `ArgumentNullException` if either argument is `null`.
- **Target: O(n).**

<details><summary>Hint</summary>

Count characters of the first string in a `Dictionary<char, int>` (increment), then decrement for the second.
All counts must end at zero.
</details>

### Exercise 03 — Valid palindrome
File: `Exercise03_IsPalindrome.cs`

```csharp
public static bool IsPalindrome(string text)
```

Considering **only letters and digits** and ignoring case, does the text read the same forwards and backwards?

| Input | Output |
|---|---|
| `"A man, a plan, a canal: Panama"` | `true` |
| `"race a car"` | `false` |
| `""` | `true` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n) time, O(1) extra space** (don't build a cleaned copy).

<details><summary>Hint</summary>

Two indices from both ends. Skip characters that aren't `char.IsLetterOrDigit`, compare with `char.ToLowerInvariant`.
</details>

### Exercise 04 — First unique character
File: `Exercise04_FirstUniqueCharacter.cs`

```csharp
public static int FindIndex(string text)
```

Return the index of the first character that appears exactly once, or `-1` if there is none.

| Input | Output |
|---|---|
| `"leetcode"` | `0` |
| `"loveleetcode"` | `2` |
| `"aabb"` | `-1` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n).** Checking every character against every other character is O(n²) and will time out.

### Exercise 05 — String compression
File: `Exercise05_Compress.cs`

```csharp
public static string Compress(string text)
```

Replace each run of repeated characters with the character followed by the run length. If the compressed string
is **not shorter** than the original, return the original.

| Input | Output |
|---|---|
| `"aabcccccaaa"` | `"a2b1c5a3"` |
| `"abc"` | `"abc"` (compressed `"a1b1c1"` is longer) |
| `"aabb"` | `"aabb"` (same length) |
| `"aaaaaaaaaaaa"` | `"a12"` |

- Input contains letters only. Throw `ArgumentNullException` for `null`.
- **Target: O(n)** — use `StringBuilder`.

### Exercise 06 — Longest common prefix
File: `Exercise06_LongestCommonPrefix.cs`

```csharp
public static string Find(string[] words)
```

| Input | Output |
|---|---|
| `["flower", "flow", "flight"]` | `"fl"` |
| `["dog", "racecar", "car"]` | `""` |
| `[]` | `""` |

- Throw `ArgumentNullException` for `null`.

<details><summary>Hint</summary>

Compare column by column: for index `i`, check `words[k][i]` for all words; stop at the first mismatch
or when any word ends.
</details>

### Exercise 07 — Caesar cipher
File: `Exercise07_CaesarCipher.cs`

```csharp
public static string Encrypt(string text, int shift)
public static string Decrypt(string text, int shift)
```

Shift every ASCII letter `shift` positions forward in the alphabet, wrapping around from `z` to `a`.
Keep the case; leave every other character unchanged. `Decrypt` undoes `Encrypt`.

| Input | Output |
|---|---|
| `Encrypt("Hello, World!", 3)` | `"Khoor, Zruog!"` |
| `Encrypt("xyz", 2)` | `"zab"` |
| `Encrypt("abc", -1)` | `"zab"` |
| `Decrypt("Khoor, Zruog!", 3)` | `"Hello, World!"` |

- `shift` can be negative or larger than 26. Throw `ArgumentNullException` for `null`.

<details><summary>Hint</summary>

Normalize the shift with `((shift % 26) + 26) % 26`. For a lowercase letter: `(char)('a' + (c - 'a' + shift) % 26)`.
</details>

### Exercise 08 — Parse an integer
File: `Exercise08_ParseInt.cs`

```csharp
public static int Parse(string text)
```

Convert text to an `int` **without** `int.Parse`, `Convert` or similar helpers. The format is:
optional surrounding whitespace, an optional `+` or `-` sign, then one or more decimal digits.

| Input | Output |
|---|---|
| `"42"` | `42` |
| `"   -17  "` | `-17` |
| `"+007"` | `7` |
| `"-2147483648"` | `int.MinValue` |
| `"12a"`, `""`, `"+"`, `"4 2"` | `FormatException` |
| `"2147483648"` | `OverflowException` |

- Throw `ArgumentNullException` for `null`.

<details><summary>Hint</summary>

Accumulate into a `long`: `value = value * 10 + (c - '0')`. As soon as the value exceeds `2147483648`
you know it overflows (even with a minus sign), so you can stop early.
</details>

## Running the tests

```bash
dotnet test 04-Strings/Strings.Tests
dotnet test 04-Strings/Strings.Tests --filter "FullyQualifiedName~Exercise08"
```
