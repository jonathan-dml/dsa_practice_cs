namespace DsaPractice.Complexity;

/// <summary>
/// Exercise 02 — Return true if any value appears at least twice.
/// Target complexity: O(n) time. See README.md for details.
/// </summary>
public static class ContainsDuplicate
{
    public static bool HasDuplicate(int[] numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);
        if(numbers.Length < 2)
            return false;
        HashSet<int> numberSet = [];
        foreach(int n in numbers)
        {
            if(numberSet.Add(n) == false)
                return true;
        }
        return false;
    }
}
