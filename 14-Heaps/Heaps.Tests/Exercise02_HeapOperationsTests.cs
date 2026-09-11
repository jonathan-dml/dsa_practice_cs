namespace DsaPractice.Heaps.Tests;

public class Exercise02_HeapOperationsTests
{
    public static TheoryData<int[]> Arrays
    {
        get
        {
            var random = new Random(103);
            return new TheoryData<int[]>
            {
                Array.Empty<int>(),
                new[] { 1 },
                new[] { 2, 1 },
                new[] { 5, 4, 3, 2, 1 },
                new[] { 1, 2, 3, 4, 5 },
                new[] { 3, 3, 1, 1, 2, 2 },
                new[] { int.MaxValue, int.MinValue, 0 },
                Enumerable.Range(0, 257).Select(_ => random.Next(-100, 100)).ToArray(),
                Enumerable.Range(0, 1000).Select(_ => random.Next()).ToArray(),
            };
        }
    }

    [Theory]
    [MemberData(nameof(Arrays))]
    public void BuildMinHeapSatisfiesTheHeapProperty(int[] input)
    {
        int[] values = input.ToArray();

        HeapOperations.BuildMinHeap(values);

        for (int i = 0; i < values.Length; i++)
        {
            int left = 2 * i + 1, right = 2 * i + 2;
            if (left < values.Length) Assert.True(values[i] <= values[left], $"values[{i}] > values[{left}]");
            if (right < values.Length) Assert.True(values[i] <= values[right], $"values[{i}] > values[{right}]");
        }
        Assert.Equal(input.Order(), values.Order());
    }

    [Theory]
    [MemberData(nameof(Arrays))]
    public void HeapSortSortsAscending(int[] input)
    {
        int[] values = input.ToArray();

        HeapOperations.HeapSort(values);

        Assert.Equal(input.Order(), values);
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => HeapOperations.BuildMinHeap(null!));
        Assert.Throws<ArgumentNullException>(() => HeapOperations.HeapSort(null!));
    }

    [Fact]
    public void HeapSortRunsInLinearithmicTime()
    {
        var random = new Random(104);
        int[] values = Enumerable.Range(0, 1_000_000).Select(_ => random.Next()).ToArray();
        int[] expected = values.Order().ToArray();

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () => HeapOperations.HeapSort(values),
            "Build a max-heap once, then repeatedly move the root to the end and sift down.");

        Assert.Equal(expected, values);
    }
}
