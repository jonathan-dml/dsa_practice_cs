# Data Structures & Algorithms Practice in C#

A hands-on course: **17 lessons, 133 exercises**. Every lesson has a README that explains the concept and describes the
exercises, a project with exercise stubs for you to implement, and an xUnit project whose tests tell you when your
solution is correct — and fast enough.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Any editor: Visual Studio, VS Code with C# Dev Kit, or JetBrains Rider

## How it works

1. Open a lesson folder and read its `README.md`.
2. Open the `*.Exercises` project and pick an exercise file (`Exercise01_...cs`). Every method throws
   `NotImplementedException`.
3. Implement it.
4. Run that exercise's tests until they're green, then move on.

All tests fail at the start — that's expected. Each exercise has its own test class named `ExerciseNN_...Tests`.

```bash
# Build everything
dotnet build

# Run all tests of one lesson
dotnet test 03-Arrays/Arrays.Tests

# Run only the tests of one exercise
dotnet test 03-Arrays/Arrays.Tests --filter "FullyQualifiedName~Exercise03"

# Run one specific test
dotnet test 03-Arrays/Arrays.Tests --filter "FullyQualifiedName~Exercise03_RotateRightTests.RunsInLinearTime"
```

In Visual Studio or VS Code the tests also appear in the Test Explorer, grouped by lesson and exercise.

## Repository layout

```
dsa_practice_cs/
├─ DsaPractice.slnx                 solution with every project
├─ Directory.Build.props            shared settings (net10.0, nullable, xUnit for *.Tests projects)
├─ Directory.Packages.props         central NuGet package versions
├─ Shared/DsaPractice.Testing/      test helpers (PerformanceAssert)
└─ NN-Topic/
   ├─ README.md                     concept explanation + exercise descriptions
   ├─ Topic.Exercises/              your code goes here
   └─ Topic.Tests/                  xUnit tests (don't change them)
```

## Performance tests

Many exercises have a target complexity (for example "O(n) — the nested-loop solution will time out"). Those tests use
`PerformanceAssert.CompletesWithin`, which runs your code with a generous time limit on large inputs. A correct
solution with the right complexity finishes in milliseconds; a solution with the wrong complexity takes seconds or
minutes and the test fails with a hint.

Timings are measured in Debug builds on typical hardware. If a correct solution fails only on a very slow machine,
run the tests in Release: `dotnet test -c Release`.

## Roadmap

| # | Lesson | Exercises | Highlights |
|---|---|---|---|
| 01 | [Complexity (Big-O)](01-Complexity/README.md) | 7 | reading complexity, hash-based speedups, prefix sums, fast power |
| 02 | [Recursion](02-Recursion/README.md) | 8 | call stack, memoization, backtracking (subsets, permutations), Hanoi |
| 03 | [Arrays](03-Arrays/README.md) | 9 | in-place algorithms, rotation, merging, matrices |
| 04 | [Strings](04-Strings/README.md) | 8 | immutability, `StringBuilder`, anagrams, parsing |
| 05 | [Dynamic Array](05-DynamicArray/README.md) | 8 | build your own `List<T>`: growth, shifting, enumerators |
| 06 | [Linked Lists](06-LinkedLists/README.md) | 8 | singly/doubly linked lists, reversal, Floyd's cycle detection |
| 07 | [Stacks](07-Stacks/README.md) | 7 | array-backed stack, brackets, RPN, monotonic stack |
| 08 | [Queues and Deques](08-QueuesAndDeques/README.md) | 7 | circular buffer, two-stack queue, sliding window maximum |
| 09 | [Hash Tables](09-HashTables/README.md) | 8 | build a hash map, two sum, prefix sums + map, LRU cache |
| 10 | [Two Pointers & Sliding Window](10-TwoPointersSlidingWindow/README.md) | 8 | 3Sum, longest unique substring, trapping rain water |
| 11 | [Binary Search](11-BinarySearch/README.md) | 8 | boundaries, rotated arrays, binary search on the answer |
| 12 | [Sorting](12-Sorting/README.md) | 8 | elementary sorts, merge sort, quicksort, counting sort, quickselect |
| 13 | [Trees and BST](13-TreesAndBST/README.md) | 8 | traversals, validation, BST insert/delete, diameter |
| 14 | [Heaps](14-Heaps/README.md) | 8 | binary heap, heap sort, top k, k-way merge, running median |
| 15 | [Tries](15-Tries/README.md) | 7 | prefix trees, autocomplete, deletion, wildcard search |
| 16 | [Graphs](16-Graphs/README.md) | 8 | BFS/DFS, cycles, topological sort, Dijkstra, union-find |
| 17 | [Dynamic Programming](17-DynamicProgramming/README.md) | 8 | coin change, LIS, LCS, knapsack, edit distance |

The lessons build on each other (for example, Lesson 05 reuses ideas from Lesson 03, and Lesson 16 uses queues,
stacks, heaps and hash sets), so following the order is recommended.

## Tips

- Read the whole exercise description first — it lists the edge cases and exceptions the tests check.
- If you're stuck, open the collapsible **Hint** under the exercise before looking anything up.
- Start with a simple correct solution, make the correctness tests pass, then optimize for the performance test.
- The tests are part of the specification: reading them after an attempt is a good way to discover a missed edge case.
