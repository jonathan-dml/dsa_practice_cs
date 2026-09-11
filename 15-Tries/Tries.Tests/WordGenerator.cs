namespace DsaPractice.Tries.Tests;

internal static class WordGenerator
{
    public static string RandomWord(Random random, int minLength, int maxLength, string alphabet = "abcdefghijklmnopqrstuvwxyz")
    {
        int length = random.Next(minLength, maxLength + 1);
        return string.Create(length, (random, alphabet), static (span, state) =>
        {
            for (int i = 0; i < span.Length; i++) span[i] = state.alphabet[state.random.Next(state.alphabet.Length)];
        });
    }

    public static string[] RandomWords(int seed, int count, int minLength, int maxLength, string alphabet = "abcdefghijklmnopqrstuvwxyz")
    {
        var random = new Random(seed);
        return Enumerable.Range(0, count).Select(_ => RandomWord(random, minLength, maxLength, alphabet)).ToArray();
    }

    /// <summary>Every string of length 0..maxLength over the alphabet.</summary>
    public static IEnumerable<string> AllStrings(string alphabet, int maxLength)
    {
        IEnumerable<string> level = [""];
        for (int length = 0; length <= maxLength; length++)
        {
            foreach (var s in level) yield return s;
            level = level.SelectMany(s => alphabet.Select(c => s + c)).ToList();
        }
    }
}
