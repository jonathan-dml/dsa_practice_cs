namespace DsaPractice.Complexity;

/// <summary>
/// Exercise 05 — Compute (baseValue ^ exponent) mod modulus.
/// Target complexity: O(log exponent). See README.md for details.
/// </summary>
public static class FastModPower
{
    public static long Pow(long baseValue, long exponent, long modulus)
    {
        if(baseValue < 0 || exponent < 0 || modulus < 1 || modulus > int.MaxValue)
            throw new ArgumentOutOfRangeException();
        
        long result = 1 % modulus;
        baseValue %= modulus;
        while(exponent > 0)
        {
            if(exponent % 2 == 1)
                result = result * baseValue % modulus;

            baseValue = baseValue * baseValue % modulus;
            exponent /= 2;
        }
        return result;
    }
}
