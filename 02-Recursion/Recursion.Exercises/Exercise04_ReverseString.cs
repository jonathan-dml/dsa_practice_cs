namespace DsaPractice.Recursion;

/// <summary>
/// Exercise 04 — Reverse a string recursively. See README.md for details.
/// </summary>
public static class StringReverser
{
    public static string Reverse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);
        
        return new string(ReverseString(s.ToCharArray(), 0, s.Length - 1));
    }

    private static char[] ReverseString(char[] s,int left,int right)
    {
        if(left >= right)
        {
            return s;
        }
        //tuple swap
        (s[left], s[right]) = (s[right], s[left]);

        return ReverseString(s,left+1,right-1);
    }

    /* Better iterative solution O(n/2) 
    public static string Reverse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);
        int left = 0;
        int right = s.Length - 1;
        char[] reversedString = s.ToCharArray();
        while(left < right)
        {
            char aux = reversedString[left];
            reversedString[left] = reversedString[right];
            reversedString[right] = aux;
            left++;
            right--;
        }
        return new string(reversedString);
    } */
    
    /* Simple recursive solution O(n)
    public static string Reverse(string s)
    {
        ArgumentNullException.ThrowIfNull(s);
        if(s.Length <= 1)
            return s;
        return Reverse(s[1..]) + s[0];
    } */
}
