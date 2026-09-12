namespace DsaPractice.Complexity;

public enum BigO
{
    Constant,
    Logarithmic,
    SquareRoot,
    Linear,
    Linearithmic,
    Quadratic,
    Exponential,
}

/// <summary>
/// Exercise 04 — Return the time complexity of each snippet (as a function of the input size n).
/// See README.md for details.
/// </summary>
public static class ComplexityQuiz
{
    // int First(int[] items) => items[0];
    public static BigO SnippetA() => BigO.Constant;

    // int CountHalvings(int n) { int steps = 0; while (n > 1) { n /= 2; steps++; } return steps; }
    public static BigO SnippetB() => BigO.Logarithmic;

    // long Sum(int[] items) { long total = 0; foreach (var x in items) total += x; return total; }
    public static BigO SnippetC() => BigO.Linear;

    // int CountEqualPairs(int[] items)
    // {
    //     int count = 0;
    //     for (int i = 0; i < items.Length; i++)
    //         for (int j = 0; j < items.Length; j++)
    //             if (i != j && items[i] == items[j]) count++;
    //     return count;
    // }
    public static BigO SnippetD() => BigO.Quadratic;

    // long Work(int n)
    // {
    //     long work = 0;
    //     for (int i = 0; i < n; i++)
    //         for (int j = 1; j < n; j *= 2) work++;
    //     return work;
    // }
    public static BigO SnippetE() => BigO.Linearithmic;

    // long Fib(int n) => n < 2 ? n : Fib(n - 1) + Fib(n - 2);
    public static BigO SnippetF() => BigO.Exponential;

    // int Range(int[] items)
    // {
    //     int max = int.MinValue, min = int.MaxValue;
    //     foreach (var x in items) max = Math.Max(max, x);
    //     foreach (var x in items) min = Math.Min(min, x);
    //     return max - min;
    // }
    public static BigO SnippetG() => BigO.Linear;

    // long Work(int n)
    // {
    //     long work = 0;
    //     for (int i = 0; i < n; i++)
    //         for (int j = 0; j < 100; j++) work++;
    //     return work;
    // }
    public static BigO SnippetH() => BigO.Linear;

    // bool HasDivisor(int n)
    // {
    //     for (int d = 2; d * d <= n; d++) if (n % d == 0) return true;
    //     return false;
    // }
    public static BigO SnippetI() => BigO.SquareRoot;
}
