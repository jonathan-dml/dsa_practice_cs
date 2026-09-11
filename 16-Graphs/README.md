# Lesson 16 — Graphs

## Learning goals

- Model relationships as **vertices** and **edges** (directed/undirected, weighted/unweighted).
- Choose between an **adjacency list** and an **adjacency matrix**.
- Traverse graphs with **BFS** and **DFS**, and know which problems each one solves.
- Detect cycles, order tasks with **topological sort**, find weighted shortest paths with **Dijkstra**, and group
  vertices with **union-find**.

## The concept

A graph `G = (V, E)` is a set of vertices connected by edges. Trees and linked lists are special graphs; roads,
networks, dependencies and social connections are general ones.

```
Undirected                         Directed (with weights)

  0 ─── 1                            0 ──4──▶ 1
  │     │                            │        │
  │     │                            1        1
  2 ─── 3 ─── 4                      ▼        ▼
                                     2 ──5──▶ 3 ──3──▶ 4
                                     └──2──▶ 1
```

### Representations

**Adjacency list** — for each vertex, the list of its neighbours. Memory O(V + E). Best for sparse graphs (most real
graphs).

```
0: [1, 2]
1: [0, 3]
2: [0, 3]
3: [1, 2, 4]
4: [3]
```

**Adjacency matrix** — a `V × V` boolean (or weight) table. Memory O(V²), but O(1) edge lookup.

| | Adjacency list | Adjacency matrix |
|---|---|---|
| Memory | O(V + E) | O(V²) |
| Is there an edge u → v? | O(degree) (O(1) with a hash set) | O(1) |
| Iterate neighbours of u | O(degree) | O(V) |

In the exercises most functions receive the graph as `vertexCount` plus an **edge list** `int[][] edges`
(`[from, to]` or `[from, to, weight]`), with vertices numbered `0 .. vertexCount - 1`. Build an adjacency list first,
adding neighbours **in the order the edges appear** (for undirected graphs, add each edge to both endpoints).

### Breadth-first search (BFS)

Explore in rings: first the start, then all vertices 1 edge away, then 2 edges away… using a **queue**.
BFS finds **shortest paths in unweighted graphs**.

```csharp
var visited = new bool[n];
var queue = new Queue<int>();
visited[start] = true;
queue.Enqueue(start);
while (queue.Count > 0)
{
    int v = queue.Dequeue();
    foreach (int w in adjacency[v])
        if (!visited[w]) { visited[w] = true; queue.Enqueue(w); }   // mark when enqueuing!
}
```

### Depth-first search (DFS)

Go as deep as possible before backtracking — recursion, or an explicit **stack**. DFS is the basis for connectivity,
cycle detection and topological sorting. Deep graphs can overflow the call stack with recursion; an explicit stack
avoids that (pop a vertex, skip it if already visited, mark it, push its neighbours in **reverse** order to get the same
order as the recursive version).

### Cycle detection in directed graphs

Color vertices during DFS: **white** (unvisited), **gray** (on the current path), **black** (finished). Reaching a gray
vertex means there's a back edge → a cycle. A plain `visited` flag is not enough: in the diamond `0→1, 0→2, 1→3, 2→3`,
vertex 3 is reached twice without any cycle.

### Topological sort

An order of the vertices of a **DAG** where every edge goes from earlier to later. **Kahn's algorithm**: repeatedly
remove vertices with in-degree 0 (a queue of "ready" vertices). If not all vertices get removed, there's a cycle.

### Dijkstra's algorithm

Shortest paths from one source when all weights are **non-negative**. Keep a priority queue of `(vertex, distance)`;
repeatedly take the closest unsettled vertex and **relax** its outgoing edges:

```
if dist[u] + w < dist[v]: dist[v] = dist[u] + w; queue.Enqueue(v, dist[v])
```

With a binary heap: O((V + E) log V). Skip queue entries whose distance is outdated.

### Union-find (disjoint set union)

Maintains a partition of elements into sets with two near-constant-time operations: `Find(x)` (the set's
representative) and `Union(a, b)`. Two optimizations make it fast:

- **Path compression** — while finding the root, point every visited node directly to it.
- **Union by size/rank** — attach the smaller tree under the larger one.

### Complexity summary

| Algorithm | Time |
|---|---|
| BFS / DFS | O(V + E) |
| Cycle detection (DFS colors) | O(V + E) |
| Topological sort (Kahn) | O(V + E) |
| Dijkstra (binary heap) | O((V + E) log V) |
| Union-find, m operations | O(m · α(n)) ≈ O(m) |

### Common pitfalls

- Marking vertices as visited when **dequeuing** instead of enqueuing in BFS (vertices get enqueued many times).
- Forgetting that undirected edges go both ways.
- Using Dijkstra with negative weights.
- Grid problems: checking bounds before indexing.

## Exercises

Unless stated otherwise: throw `ArgumentNullException` for `null` arguments, `ArgumentOutOfRangeException` for a negative
`vertexCount` or for vertices outside `0 .. vertexCount - 1`, and `ArgumentException` for edges with the wrong number of
values.

### Exercise 01 — Adjacency-list graph
File: `Exercise01_Graph.cs`

```csharp
public Graph(bool directed)
public bool IsDirected { get; }
public int VertexCount { get; }
public int EdgeCount { get; }
public IReadOnlyList<int> Vertices { get; }         // insertion order
public bool AddVertex(int vertex)                   // false if it already exists
public bool AddEdge(int from, int to)               // adds missing vertices; false if the edge exists
public bool RemoveEdge(int from, int to)            // false if the edge doesn't exist
public bool HasEdge(int from, int to)
public IReadOnlyList<int> Neighbors(int vertex)     // insertion order; KeyNotFoundException if unknown
```

- Vertices can be any `int`. In an undirected graph an edge connects both ways but counts **once** in `EdgeCount`.
- A self-loop `(v, v)` is allowed and appears once in `Neighbors(v)`.
- **Target: O(1)** average for `AddEdge`, `HasEdge` and `AddVertex` — even for a vertex with 100 000 neighbours.

### Exercise 02 — Breadth-first search
File: `Exercise02_GraphBfs.cs`

```csharp
public static IList<int> Order(int vertexCount, int[][] edges, int start, bool directed = false)
public static int ShortestPathLength(int vertexCount, int[][] edges, int source, int target, bool directed = false)
```

- `Order` returns the vertices reachable from `start` in BFS order (neighbours in edge-list order).
- `ShortestPathLength` returns the minimum number of edges from `source` to `target`, `0` when they're equal, `-1` if
  unreachable.

```
edges = [[0,1],[0,2],[1,3],[2,4],[3,5],[4,5]]      (undirected)
Order(6, edges, 0)                  → [0, 1, 2, 3, 4, 5]
ShortestPathLength(6, edges, 0, 5)  → 3
```

### Exercise 03 — Depth-first search
File: `Exercise03_GraphDfs.cs`

```csharp
public static IList<int> Order(int vertexCount, int[][] edges, int start, bool directed = false)
public static bool HasPath(int vertexCount, int[][] edges, int source, int target, bool directed = false)
```

- `Order` returns vertices in DFS **pre-order** — the order of the recursive algorithm that visits neighbours in edge-list order.

```
same edges as above
Order(6, edges, 0) → [0, 1, 3, 5, 4, 2]
```

- Graphs can be 200 000 vertices deep; the tests run your code on a thread with a large stack, but an iterative
  version is good practice.

### Exercise 04 — Number of islands
File: `Exercise04_NumberOfIslands.cs`

```csharp
public static int Count(char[][] grid)
```

`'1'` is land and `'0'` is water. An island is a group of land cells connected **horizontally or vertically**.

```
1 1 0 0 0
1 1 0 0 0      → 3 islands
0 0 1 0 0
0 0 0 1 1
```

- You may modify the grid. Throw `ArgumentNullException` for `null`.

### Exercise 05 — Cycle in a directed graph
File: `Exercise05_HasCycle.cs`

```csharp
public static bool HasCycleDirected(int vertexCount, int[][] edges)
```

| Edges | Output |
|---|---|
| `[[0,1],[1,2],[2,0]]` | `true` |
| `[[0,1],[0,2],[1,3],[2,3]]` | `false` (diamond) |
| `[[3,3]]` | `true` (self-loop) |

### Exercise 06 — Course schedule (topological sort)
File: `Exercise06_TopologicalSort.cs`

```csharp
public static int[]? CourseOrder(int courseCount, int[][] prerequisites)
```

Each prerequisite `[course, required]` means `required` must be taken **before** `course`. Return any valid order that
contains every course once, or `null` if it's impossible (a cycle).

| Input | Output |
|---|---|
| `2`, `[[1, 0]]` | `[0, 1]` |
| `4`, `[[1,0],[2,0],[3,1],[3,2]]` | `[0, 1, 2, 3]` or `[0, 2, 1, 3]` |
| `2`, `[[1,0],[0,1]]` | `null` |

### Exercise 07 — Dijkstra's shortest paths
File: `Exercise07_Dijkstra.cs`

```csharp
public static long[] ShortestDistances(int vertexCount, int[][] edges, int source)
```

`edges` are **directed** `[from, to, weight]`. Return the shortest distance from `source` to every vertex, or `-1` for
unreachable vertices.

```
edges = [[0,1,4],[0,2,1],[2,1,2],[1,3,1],[2,3,5],[3,4,3]]
ShortestDistances(5, edges, 0) → [0, 3, 1, 4, 7]
```

- Throw `ArgumentException` for a negative weight.
- **Target: O((V + E) log V)** with `PriorityQueue`.

### Exercise 08 — Union-find
File: `Exercise08_DisjointSet.cs`

```csharp
// class DisjointSet
public DisjointSet(int size)
public int SetCount { get; }
public int Find(int element)
public bool Union(int first, int second)      // false if already in the same set
public bool Connected(int first, int second)

// static class ConnectedComponents
public static int Count(int vertexCount, int[][] edges)
```

- Elements are `0 .. size - 1`; throw `ArgumentOutOfRangeException` for anything else (and for a negative size).
- `ConnectedComponents.Count` returns the number of connected components of an undirected graph — use your `DisjointSet`.
- **Target: nearly O(1)** amortized per operation with path compression and union by size.

## Running the tests

```bash
dotnet test 16-Graphs/Graphs.Tests
dotnet test 16-Graphs/Graphs.Tests --filter "FullyQualifiedName~Exercise07"
```
