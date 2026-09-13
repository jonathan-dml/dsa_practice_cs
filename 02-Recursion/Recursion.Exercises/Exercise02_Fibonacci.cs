namespace DsaPractice.Recursion;

/// <summary>
/// Exercise 02 — Fibonacci numbers: a naive O(2ⁿ) version and a memoized O(n) version.
/// See README.md for details.
/// </summary>
public static class Fibonacci
{
    private static Dictionary<int, long> storedValues = new();
    public static long Naive(int n)
    {
        if (n < 0 || n > 92)
            throw new ArgumentOutOfRangeException(nameof(n));
        if (n == 0)
            return 0;
        if (n == 1)
            return 1;

        return Naive(n - 1) + Naive(n - 2);
    }

    public static long Memoized(int n)
    {
        if (n < 0 || n > 92)
            throw new ArgumentOutOfRangeException(nameof(n));

        if (storedValues.ContainsKey(n))
        {
            return storedValues[n];
        }

        if (n == 0)
        {
            storedValues[n] = 0;
            return 0;
        }

        if (n == 1)
        {
            storedValues[n] = 1;
            return 1;
        }

        storedValues[n] = Memoized(n - 1) + Memoized(n - 2);

        return storedValues[n];
    }




}
