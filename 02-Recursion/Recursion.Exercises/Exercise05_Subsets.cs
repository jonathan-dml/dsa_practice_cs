namespace DsaPractice.Recursion;

/// <summary>
/// Exercise 05 — Generate every subset (the power set) of distinct items. See README.md for details.
/// </summary>
public static class Subsets
{
    public static IList<IList<int>> Generate(int[] items)
    {
        ArgumentNullException.ThrowIfNull(items);
        IList<IList<int>> subsets = new List<IList<int>>();
        List<int> current = new();
        GenerateSubsets(items, 0, current, subsets);
        return subsets;

    }

    private static void GenerateSubsets(int[] items, int index, List<int> current,  IList<IList<int>> subsets)
    {
        if(index == items.Length)
        {
            subsets.Add([..current]);
            return;
        }
        // Don't include the current element
        GenerateSubsets(items, index + 1, current, subsets);
        // Include the current element
        current.Add(items[index]);
        
        GenerateSubsets(items, index + 1, current, subsets);

        // Undo the choice (backtrack)
        current.RemoveAt(current.Count - 1);
    }

    /* Iterative solution 
    public static IList<IList<int>> Generate(int[] items)
    {
        ArgumentNullException.ThrowIfNull(items);

        IList<IList<int>> subsets = new List<IList<int>>();
        //adds empty list
        subsets.Add(new List<int>());

        for(int i = 0; i < items.Length; i++)
        {
            //iterates over collections already added
            for(int j = subsets.Count - 1; j >= 0; j--)
            {
                //append current element to every collection and add as a new collection
                subsets.Add([..subsets[j], items[i]]);
            }
        }
        return subsets;
    } */
}
