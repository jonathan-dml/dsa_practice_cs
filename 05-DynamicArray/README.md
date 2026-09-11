# Lesson 05 — Dynamic Array (build your own `List<T>`)

## Learning goals

- Understand the difference between an array's **capacity** and a list's **count**.
- Implement geometric growth and explain why `Add` is **amortized O(1)**.
- Shift elements correctly when inserting and removing.
- Use `EqualityComparer<T>.Default` to compare values of a generic type.
- Implement `IEnumerable<T>` and detect modifications during enumeration.

## The concept

Arrays have a fixed size. A **dynamic array** (C#'s `List<T>`, Java's `ArrayList`, C++'s `std::vector`) hides a
regular array behind an object that tracks how many slots are actually used:

```
Count = 3, Capacity = 4

_items: ┌─────┬─────┬─────┬─────┐
        │ 10  │ 20  │ 30  │  ·  │   ← slot 3 is allocated but not part of the list
        └─────┴─────┴─────┴─────┘
```

When `Add` finds the backing array full, it allocates a **bigger** array, copies the elements, and continues:

```
Add(50) with Count == Capacity == 4:

old: [10, 20, 30, 40]
new: [10, 20, 30, 40, 50, ·, ·, ·]    ← capacity doubled to 8
```

### Why doubling? (amortized analysis)

If the capacity grew by 1 each time, adding `n` elements would copy `1 + 2 + … + n ≈ n²/2` elements — O(n²).
With doubling, copies happen at sizes 4, 8, 16, …, n, which add up to less than `2n`. Spread over `n` additions
that is a constant cost per `Add`: **amortized O(1)**. An individual `Add` may be O(n), but the average is O(1).

### Operations

| Operation | Time |
|---|---|
| `this[index]` get / set | O(1) |
| `Add` | amortized O(1) |
| `Insert(index, item)` | O(n) — shifts elements right |
| `RemoveAt(index)` | O(n) — shifts elements left |
| `IndexOf`, `Contains`, `Remove(item)` | O(n) |
| `Clear` | O(n) (to release references) |

### Implementation notes

- **Shifting direction matters.** For `Insert`, move elements from the *end* backwards (`_items[i] = _items[i - 1]`),
  otherwise you overwrite values before copying them. For `RemoveAt`, move forwards. `Array.Copy` handles
  overlapping ranges correctly too.
- **Clear unused slots** (`_items[i] = default!`) after removing, so the list doesn't keep objects alive for the GC.
- **Generic equality:** `==` doesn't compile for an unconstrained `T`. Use `EqualityComparer<T>.Default.Equals(a, b)`,
  which also handles `null`.
- **Enumerator invalidation:** keep a `_version` counter that every modification increments. The enumerator
  remembers the version it started with and throws `InvalidOperationException` if it changes — exactly what
  `List<T>` does.

### Common pitfalls

- Validating an index against `Capacity` instead of `Count`.
- Forgetting to grow before `Insert`.
- Returning the internal array from `ToArray` instead of a copy.

## Exercises

All exercises build the same class, `MyList<T>`, in `MyList.cs`. The members are grouped by `#region`.
**Do them in order** — the tests of later exercises use the members from earlier ones
(e.g. the indexer from Exercise 02 is used to check contents from Exercise 03 on).

### Exercise 01 — Construction, `Add`, `Count` and `Capacity`

```csharp
public MyList()
public MyList(int capacity)
public int Count { get; }
public int Capacity { get; }
public void Add(T item)
```

- `new MyList<T>()` starts with `Count == 0` and `Capacity == 0` (no array allocated yet, or an empty one).
- `new MyList<T>(capacity)` starts with exactly that capacity; throw `ArgumentOutOfRangeException` if it's negative.
- When there is no free slot, grow to `DefaultCapacity` (4) if the capacity is 0, otherwise to **twice** the current capacity.

| Items added (default constructor) | Capacity |
|---|---|
| 1–4 | 4 |
| 5–8 | 8 |
| 9–16 | 16 |

- **Target: amortized O(1) per `Add`** (tested with 2 million additions).

### Exercise 02 — Indexer

```csharp
public T this[int index] { get; set; }
```

- Get or replace the element at `index`.
- Throw `ArgumentOutOfRangeException` when `index < 0` or `index >= Count` — even if `index < Capacity`.

### Exercise 03 — `Insert`

```csharp
public void Insert(int index, T item)
```

- Insert `item` at `index`, shifting later elements one position to the right. Grow if needed.
- `index == Count` is valid (same as `Add`). Throw `ArgumentOutOfRangeException` when `index < 0` or `index > Count`.

| List | Call | Result |
|---|---|---|
| `[1, 2, 3]` | `Insert(0, 9)` | `[9, 1, 2, 3]` |
| `[1, 2, 3]` | `Insert(2, 9)` | `[1, 2, 9, 3]` |
| `[1, 2, 3]` | `Insert(3, 9)` | `[1, 2, 3, 9]` |

### Exercise 04 — `RemoveAt` and `Remove`

```csharp
public void RemoveAt(int index)
public bool Remove(T item)
```

- `RemoveAt` removes the element at `index` and shifts later elements left. Throw `ArgumentOutOfRangeException`
  for an invalid index. The capacity does not change.
- `Remove` removes the **first** occurrence of `item` and returns `true`, or returns `false` if it isn't found.

### Exercise 05 — `IndexOf` and `Contains`

```csharp
public int IndexOf(T item)
public bool Contains(T item)
```

- `IndexOf` returns the index of the first element equal to `item` (using `EqualityComparer<T>.Default`), or `-1`.
- Only the first `Count` slots belong to the list — never match stale values in the unused part of the array.

### Exercise 06 — `Clear` and `TrimExcess`

```csharp
public void Clear()
public void TrimExcess()
```

- `Clear` removes all elements: `Count` becomes 0, `Capacity` stays the same.
- `TrimExcess` shrinks the capacity to exactly `Count`, preserving the elements.

### Exercise 07 — Enumeration

```csharp
public IEnumerator<T> GetEnumerator()
```

- Yield the elements from index 0 to `Count - 1`, so `foreach` and LINQ work.
- If the list is modified (`Add`, `Insert`, `RemoveAt`, `Remove`, `Clear`, `Reverse` or the indexer setter)
  while an enumeration is in progress, the enumerator must throw `InvalidOperationException` on its next `MoveNext`.

<details><summary>Hint</summary>

Add a `private int _version;` incremented by every modifying method. In `GetEnumerator` (written with `yield return`),
capture the version at the start and compare it before yielding each element and once more after the loop.
</details>

### Exercise 08 — `Reverse` and `ToArray`

```csharp
public void Reverse()
public T[] ToArray()
```

- `Reverse` reverses the elements in place.
- `ToArray` returns a **new** array of length `Count` with the elements in order.

## Running the tests

```bash
dotnet test 05-DynamicArray/DynamicArray.Tests
dotnet test 05-DynamicArray/DynamicArray.Tests --filter "FullyQualifiedName~Exercise01"
```
