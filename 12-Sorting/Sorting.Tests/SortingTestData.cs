namespace DsaPractice.Sorting.Tests;

public static class SortingTestData
{
    /// <summary>Arrays covering the usual edge cases plus a few random ones.</summary>
    public static TheoryData<int[]> Arrays
    {
        get
        {
            var data = new TheoryData<int[]>
            {
                Array.Empty<int>(),
                new[] { 1 },
                new[] { 2, 1 },
                new[] { 1, 2, 3, 4, 5 },
                new[] { 5, 4, 3, 2, 1 },
                new[] { 3, 1, 2, 3, 1, 2 },
                new[] { -5, 3, 0, -1, 8, -5 },
                new[] { 7, 7, 7, 7 },
                new[] { int.MaxValue, int.MinValue, 0, -1, 1 },
            };

            data.Add(Random(seed: 1, length: 100, min: -1000, max: 1000));
            data.Add(Random(seed: 2, length: 1000, min: 0, max: 50));
            data.Add(Random(seed: 3, length: 1000, min: int.MinValue, max: int.MaxValue));
            return data;
        }
    }

    public static int[] Random(int seed, int length, int min, int max)
    {
        var random = new System.Random(seed);
        return Enumerable.Range(0, length).Select(_ => random.Next(min, max)).ToArray();
    }

    public static void AssertSortsLike(int[] input, Action<int[]> sort)
    {
        int[] expected = input.Order().ToArray();
        int[] actual = input.ToArray();

        sort(actual);

        Assert.Equal(expected, actual);
    }
}
