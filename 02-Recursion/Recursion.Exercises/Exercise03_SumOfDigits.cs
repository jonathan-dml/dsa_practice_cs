namespace DsaPractice.Recursion;

/// <summary>
/// Exercise 03 — Sum the decimal digits of a number (ignoring its sign). See README.md for details.
/// </summary>
public static class SumOfDigits
{
    public static int Compute(long n)
    {
        if(n < 0)
            n *= -1;
        if(n < 10)
            return (int) n % 10;
        return (int)(n % 10) + Compute(n/10);
    }
}
