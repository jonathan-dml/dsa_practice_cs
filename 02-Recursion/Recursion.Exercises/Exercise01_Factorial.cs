namespace DsaPractice.Recursion;

/// <summary>
/// Exercise 01 — Compute n! recursively for n in 0..20. See README.md for details.
/// </summary>
public static class Factorial
{
    public static long Compute(int n)
    {
        if(n > 20 || n < 0)
            throw new ArgumentOutOfRangeException(nameof(n));

        if(n == 0)
            return 1;
        return n * Compute(n-1);
    }
}
