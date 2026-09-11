# Lesson 08 — Queues and Deques

## Learning goals

- Understand the **FIFO** (first in, first out) principle.
- Implement a queue with a **circular buffer** so neither end requires shifting elements.
- Build a queue out of two stacks and reason about **amortized** cost.
- Use a **double-ended queue (deque)** — including the monotonic deque trick for sliding windows.

## The concept

A queue works like a line at a shop: people join at the **back** (`Enqueue`) and are served from the **front**
(`Dequeue`). The first to arrive is the first to leave.

```
Enqueue(1), Enqueue(2), Enqueue(3)          Dequeue() → 1

front                 back                  front          back
  ▼                     ▼                     ▼              ▼
┌────┬────┬────┐                            ┌────┬────┐
│ 1  │ 2  │ 3  │                            │ 2  │ 3  │
└────┴────┴────┘                            └────┴────┘
```

### Why not just use an array and shift?

If the front is always index 0, every `Dequeue` shifts all remaining elements one position left — O(n).
A **circular buffer** avoids this: keep the index of the front element (`head`) and the number of elements (`count`).
The back is at `(head + count) % capacity`, and indices *wrap around* the end of the array.

```
capacity 6, head = 4, count = 4

index:   0     1     2     3     4     5
       ┌─────┬─────┬─────┬─────┬─────┬─────┐
       │  C  │  D  │  ·  │  ·  │  A  │  B  │
       └─────┴─────┴─────┴─────┴─────┴─────┘
                     ▲           ▲
                   back        head (front)

order of the queue: A, B, C, D
```

When the buffer is full, allocate a bigger array and copy the elements **in queue order** (starting at `head`),
then reset `head` to 0.

### Operations

| Operation | Circular buffer | Two stacks |
|---|---|---|
| `Enqueue` | amortized O(1) | O(1) |
| `Dequeue` | O(1) | amortized O(1) |
| `Peek` | O(1) | amortized O(1) |

.NET provides `Queue<T>` (a circular buffer). For a deque there is no dedicated type — `LinkedList<T>` is often used.

### Queue from two stacks

Push new items onto an **inbox** stack. To dequeue, pop from an **outbox** stack; only when the outbox is empty,
move *everything* from the inbox to the outbox (which reverses the order, putting the oldest item on top).
Each element is moved at most once, so the total work for `n` operations is O(n): **amortized O(1)** each.

### Deque (double-ended queue)

A deque supports adding and removing at **both** ends in O(1). A circular buffer handles it naturally:
`AddFirst` moves `head` one step back (`(head - 1 + capacity) % capacity`).

### Monotonic deque

To get the maximum of every window of size `k`, keep a deque of **indices** whose values are decreasing:

1. Remove the front index if it has left the window.
2. Remove indices from the back while their values are ≤ the new value (they can never be the maximum again).
3. Add the new index at the back. The front is the maximum of the current window.

Each index is added and removed at most once → O(n).

### Where queues appear

Breadth-first search, task scheduling, buffering data between producers and consumers, rate limiting
("how many requests in the last N ms?").

### Common pitfalls

- Forgetting the modulo when an index moves past the end of the array (or below 0 for `AddFirst`).
- Copying the old buffer "as is" when growing — the elements must be unwrapped starting at `head`.
- In the two-stack queue, moving items from inbox to outbox while the outbox **still has items** breaks the order.

## Exercises

### Exercise 01 — Circular-buffer queue
File: `Exercise01_MyQueue.cs`

```csharp
public int Count { get; }
public bool IsEmpty { get; }
public void Enqueue(T item)
public T Dequeue()
public T Peek()
```

- Use a `T[]` with head/count indices that wrap around; double the array when full.
  Don't use `Queue<T>`, `List<T>` or `LinkedList<T>`.
- `Dequeue` and `Peek` throw `InvalidOperationException` when empty.
- **Target: O(1)** `Dequeue` (no shifting) and amortized O(1) `Enqueue`.

### Exercise 02 — Queue with two stacks
File: `Exercise02_QueueWithTwoStacks.cs`

```csharp
public int Count { get; }
public void Enqueue(T item)
public T Dequeue()
public T Peek()
```

- Use exactly two `Stack<T>` instances for storage.
- `Dequeue` and `Peek` throw `InvalidOperationException` when empty.
- **Target: amortized O(1)** per operation.

### Exercise 03 — Deque
File: `Exercise03_MyDeque.cs`

```csharp
public int Count { get; }
public void AddFirst(T item)
public void AddLast(T item)
public T RemoveFirst()
public T RemoveLast()
public T PeekFirst()
public T PeekLast()
```

- Implement with a circular buffer. Removing or peeking on an empty deque throws `InvalidOperationException`.
- **Target: O(1)** at both ends (amortized for adds).

### Exercise 04 — Sliding window maximum
File: `Exercise04_SlidingWindowMaximum.cs`

```csharp
public static int[] MaxInWindows(int[] numbers, int k)
```

Return the maximum of each contiguous window of size `k`, from left to right.

| Input | Output |
|---|---|
| `[1, 3, -1, -3, 5, 3, 6, 7]`, `k = 3` | `[3, 3, 5, 5, 6, 7]` |
| `[9, 8, 7]`, `k = 1` | `[9, 8, 7]` |

- Throw `ArgumentNullException` for `null` and `ArgumentOutOfRangeException` when `k < 1` or `k > numbers.Length`.
- **Target: O(n)** with a monotonic deque. Scanning each window is O(n·k) and will time out.

### Exercise 05 — Recent counter
File: `Exercise05_RecentCounter.cs`

```csharp
public int Ping(int timestamp)
```

Each call records a request at `timestamp` (milliseconds) and returns how many requests happened in the inclusive
range `[timestamp - 3000, timestamp]`, including this one.

| Calls | Returns |
|---|---|
| `Ping(1)` | `1` |
| `Ping(100)` | `2` |
| `Ping(3001)` | `3` |
| `Ping(3002)` | `3` (the ping at 1 expired) |

- Timestamps must be strictly increasing; throw `ArgumentException` otherwise.
- **Target: amortized O(1)** — discard expired pings from the front of a queue.

### Exercise 06 — Josephus problem
File: `Exercise06_Josephus.cs`

```csharp
public static IList<int> EliminationOrder(int people, int step)
public static int LastSurvivor(int people, int step)
```

`people` stand in a circle, numbered `1..people`. Starting from person 1, count `step` people; the one reached is
eliminated. Counting restarts from the next person, until everyone is gone.

| Input | Elimination order | Survivor |
|---|---|---|
| `people = 7, step = 3` | `[3, 6, 2, 7, 5, 1, 4]` | `4` |
| `people = 5, step = 2` | `[2, 4, 1, 5, 3]` | `3` |

- Throw `ArgumentOutOfRangeException` when `people < 1` or `step < 1`.

<details><summary>Hint</summary>

Put everyone in a queue. Move `step - 1` people from the front to the back, then dequeue the next one: that person
is eliminated.
</details>

### Exercise 07 — Generate binary numbers
File: `Exercise07_BinaryNumbers.cs`

```csharp
public static IList<string> Generate(int count)
```

Return the binary representations of `1..count` in order, generated with a queue (no `Convert.ToString(x, 2)`).

| Input | Output |
|---|---|
| `5` | `["1", "10", "11", "100", "101"]` |
| `0` | `[]` |

- Throw `ArgumentOutOfRangeException` for a negative `count`.

<details><summary>Hint</summary>

Start with `"1"` in the queue. Each time you dequeue `s`, output it and enqueue `s + "0"` and `s + "1"`.
</details>

## Running the tests

```bash
dotnet test 08-QueuesAndDeques/QueuesAndDeques.Tests
dotnet test 08-QueuesAndDeques/QueuesAndDeques.Tests --filter "FullyQualifiedName~Exercise04"
```
