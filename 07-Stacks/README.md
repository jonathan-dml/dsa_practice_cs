# Lesson 07 — Stacks

## Learning goals

- Understand the **LIFO** (last in, first out) principle.
- Implement a stack on top of an array.
- Recognize problems that a stack solves naturally: matching brackets, evaluating expressions,
  undoing work, parsing nested structures.
- Use a **monotonic stack** to answer "next greater element" questions in O(n).

## The concept

A stack is like a pile of plates: you can only add (**push**) a plate on top, look at the top plate (**peek**),
or take the top plate off (**pop**).

```
push(1)   push(2)   push(3)   pop() → 3   peek() → 2

                    ┌───┐
          ┌───┐     │ 3 │     ┌───┐       ┌───┐
┌───┐     │ 2 │     │ 2 │     │ 2 │       │ 2 │ ← top
│ 1 │     │ 1 │     │ 1 │     │ 1 │       │ 1 │
└───┘     └───┘     └───┘     └───┘       └───┘
```

### Operations

| Operation | Time |
|---|---|
| `Push` | amortized O(1) (array) / O(1) (linked list) |
| `Pop`, `Peek` | O(1) |
| `Count`, `IsEmpty` | O(1) |
| Search | O(n) — not what a stack is for |

.NET provides `Stack<T>` (array-backed). The program's **call stack** is the most famous stack: every method call
pushes a frame, every return pops one.

### Array-backed implementation

Keep an array and a `count`. The top of the stack is `items[count - 1]`. Push writes at `items[count++]` (growing
the array when full); pop reads `items[--count]`. Both ends of the operation happen at the end of the array, so no
element is ever shifted.

### Classic patterns

**Matching pairs** — push every opening symbol; on a closing symbol, the top of the stack must be its partner.

**Expression evaluation (Reverse Polish Notation)** — `3 4 + 2 *` means `(3 + 4) * 2`. Push numbers; on an operator,
pop the right operand, then the left operand, and push the result.

**Nested structures** — when you meet an opening bracket, push the current state (e.g. the string built so far and a
repeat count) and start fresh; when you meet the closing bracket, pop and combine.

**Monotonic stack** — to find, for each element, the next element that is greater, keep a stack of indices whose
answer is still unknown. Their values are decreasing from bottom to top. A new value pops (and answers) every smaller
value on top. Each index is pushed and popped at most once → O(n) total.

```
temperatures: [73, 74, 75, 71, 69, 72, 76, 73]
at 72: stack holds indices of [75, 71, 69] → pop 69 (answer 1 day), pop 71 (answer 2 days), push 72
```

### Common pitfalls

- Popping from an empty stack — check first, or throw a clear exception.
- Operand order: for `a - b` in RPN, the **first** pop is `b`.
- Forgetting that leftover items on the stack at the end usually mean the input was invalid.

## Exercises

### Exercise 01 — Array-backed stack
File: `Exercise01_MyStack.cs`

```csharp
public int Count { get; }
public bool IsEmpty { get; }
public void Push(T item)
public T Pop()
public T Peek()
```

- Store items in a `T[]` that doubles when full. Don't use `Stack<T>` or `List<T>`.
- `Pop` and `Peek` throw `InvalidOperationException` when the stack is empty.
- **Target: amortized O(1) per operation.**

### Exercise 02 — Balanced brackets
File: `Exercise02_ValidParentheses.cs`

```csharp
public static bool IsValid(string text)
```

Return `true` if every `(`, `[` and `{` is closed by the matching bracket in the correct order.
All other characters are ignored.

| Input | Output |
|---|---|
| `"()[]{}"` | `true` |
| `"{[()]}"` | `true` |
| `"([)]"` | `false` |
| `"(a + b) * [c]"` | `true` |
| `"(("` | `false` |

- Throw `ArgumentNullException` for `null`. **Target: O(n).**

### Exercise 03 — Evaluate Reverse Polish Notation
File: `Exercise03_EvaluateRpn.cs`

```csharp
public static int Evaluate(string[] tokens)
```

Each token is an integer (possibly negative, like `"-11"`) or one of the operators `+`, `-`, `*`, `/`.
Division truncates toward zero (C#'s normal `int` division).

| Input | Output |
|---|---|
| `["2", "1", "+", "3", "*"]` | `9` — `(2 + 1) * 3` |
| `["4", "13", "5", "/", "+"]` | `6` — `4 + 13 / 5` |

- Throw `InvalidOperationException` for a malformed expression (an operator without two operands, leftover operands,
  or no tokens at all).
- Invalid number tokens throw `FormatException` and division by zero throws `DivideByZeroException`
  (`int.Parse` and `/` already do this for you).
- Throw `ArgumentNullException` for `null`.

### Exercise 04 — Min stack
File: `Exercise04_MinStack.cs`

```csharp
public int Count { get; }
public void Push(int value)
public int Pop()
public int Top()
public int GetMin()
```

A stack of integers that can also return its current minimum.

- **Every operation must be O(1)**, including `GetMin`.
- `Pop`, `Top` and `GetMin` throw `InvalidOperationException` when empty.

<details><summary>Hint</summary>

Push pairs `(value, minSoFar)` — or keep a second stack of minimums alongside the values.
</details>

### Exercise 05 — Days until a warmer temperature
File: `Exercise05_DailyTemperatures.cs`

```csharp
public static int[] DaysUntilWarmer(int[] temperatures)
```

For each day, return how many days you have to wait for a **strictly** warmer temperature, or `0` if none comes.

| Input | Output |
|---|---|
| `[73, 74, 75, 71, 69, 72, 76, 73]` | `[1, 1, 4, 2, 1, 1, 0, 0]` |
| `[30, 40, 50, 60]` | `[1, 1, 1, 0]` |

- Throw `ArgumentNullException` for `null`.
- **Target: O(n)** with a monotonic stack. The nested-loop solution is O(n²) and will time out.

### Exercise 06 — Simplify a Unix path
File: `Exercise06_SimplifyPath.cs`

```csharp
public static string Simplify(string path)
```

Given an absolute Unix-style path, return its canonical form: `.` means the current directory, `..` goes up one level
(staying at `/` if already at the root), and repeated slashes count as one.

| Input | Output |
|---|---|
| `"/home/"` | `"/home"` |
| `"/a/./b/../../c/"` | `"/c"` |
| `"/../"` | `"/"` |
| `"/home//foo/"` | `"/home/foo"` |
| `"/..."` | `"/..."` (a directory named `...`) |

- Throw `ArgumentException` if the path doesn't start with `/`, `ArgumentNullException` for `null`.

### Exercise 07 — Decode a string
File: `Exercise07_DecodeString.cs`

```csharp
public static string Decode(string encoded)
```

The pattern `k[text]` means `text` repeated `k` times. Patterns can be nested and `k` can have several digits.

| Input | Output |
|---|---|
| `"3[a]2[bc]"` | `"aaabcbc"` |
| `"3[a2[c]]"` | `"accaccacc"` |
| `"2[abc]3[cd]ef"` | `"abcabccdcdcdef"` |
| `"10[x]"` | `"xxxxxxxxxx"` |

- The input is always well formed. Throw `ArgumentNullException` for `null`.

<details><summary>Hint</summary>

Keep a `StringBuilder current` and an `int count`. On `[`, push `(current, count)` and start a new builder.
On `]`, pop `(previous, k)` and append `current` to `previous` `k` times.
</details>

## Running the tests

```bash
dotnet test 07-Stacks/Stacks.Tests
dotnet test 07-Stacks/Stacks.Tests --filter "FullyQualifiedName~Exercise05"
```
