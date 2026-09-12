using System.Security.Cryptography.X509Certificates;

namespace DsaPractice.Complexity;

/// <summary>
/// Exercise 01 — Count the index pairs (i &lt; j) whose values add up to a target.
/// Target complexity: O(n) time. See README.md for details.
/// </summary>
public static class CountPairsWithSum
{
    public static long Count(int[] numbers, int target)
    {
        if(numbers is null)
            throw new ArgumentNullException("Invalid input");
        // create a dictionary to store seen numbers
        // key = the number
        // value = how many times it appears 
        Dictionary<long, int> seen = new();
        //count how many pairs sum up to the targer value
        long count = 0;
        foreach(int x in numbers)
        {
            //this is the value that summed up with the current number would give the target
            long remaining = target - x;
            //checks if the remaining value was seen before
            if(seen.TryGetValue(remaining, out int currentCount))
            {
                // increment the count by the count of the times the value was seen
                count += currentCount;
            }
            //adds or increments the current number to the dictionary
            seen[x] = seen.GetValueOrDefault(x) + 1;
        }

        
        return count;
    }

    /* --- O(n²) SOLUTION 
    public static long Count(int[] numbers, int target)
    {
        if(numbers is null)
            throw new ArgumentException("Invalid input");
        long count = 0;
        for(int i = 0; i < numbers.Length; i++)
        {
            for(int j = 0; j <  numbers.Length; j++)
            {
                if(numbers[j] - numbers[i] == target)
                    count++;
            }
        }
        return count;
    } */
}
