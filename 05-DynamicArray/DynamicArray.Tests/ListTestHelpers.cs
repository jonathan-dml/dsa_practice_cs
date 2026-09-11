namespace DsaPractice.DynamicArray.Tests;

internal static class ListTestHelpers
{
    /// <summary>Reads the list's elements using only Count and the indexer (Exercises 01–02).</summary>
    public static T[] Contents<T>(this MyList<T> list)
    {
        var result = new T[list.Count];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = list[i];
        }

        return result;
    }

    public static MyList<T> ListOf<T>(params T[] items)
    {
        var list = new MyList<T>();
        foreach (var item in items)
        {
            list.Add(item);
        }

        return list;
    }
}
