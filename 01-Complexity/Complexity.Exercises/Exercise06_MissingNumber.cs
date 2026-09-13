namespace DsaPractice.Complexity;

/// <summary>
/// Exercise 06 — Find the single missing value of the range [0, n].
/// Target complexity: O(n) time, O(1) extra space. See README.md for details.
/// </summary>
public static class MissingNumber
{
    public static int Find(int[] numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);

        if(numbers.Length == 0)
            return 0;
        
        long n = numbers.Length;
        long expected = n * (n + 1) / 2;
        long actual = 0;
        foreach(int x in numbers)
            actual += x;
        return (int) (expected - actual);

    }

    /* Solution using sort     
    public static int Find(int[] numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);

        if(numbers.Length == 0)
            return 0;
        numbers.Sort();
        for(int i = 0; i < numbers.Length; i++)
        {
            if(numbers[i] != i)
                return i;
        }
        return numbers.Last() + 1;

    } */
}
