namespace DsaPractice.Complexity;

/// <summary>
/// Exercise 07 — Decide whether a number is prime.
/// Target complexity: O(√n). See README.md for details.
/// </summary>
public static class PrimeCheck
{
    public static bool IsPrime(long n)
    {
        if(n < 2)
            return false;
        for(long i = 2; i * i <= n; i++)
        {
            if(n % i == 0)
                return false;
        }
        return true;
    }
}
