namespace DsaPractice.Tries.Tests;

public class Exercise02_PrefixCounterTests
{
    [Fact]
    public void CountsWordsAndPrefixes()
    {
        var counter = new PrefixCounter();

        counter.Insert("apple");
        counter.Insert("apple");
        Assert.Equal(2, counter.CountWordsEqualTo("apple"));
        Assert.Equal(2, counter.CountWordsStartingWith("app"));

        Assert.True(counter.Erase("apple"));
        Assert.Equal(1, counter.CountWordsEqualTo("apple"));
        Assert.Equal(1, counter.CountWordsStartingWith("app"));

        Assert.True(counter.Erase("apple"));
        Assert.Equal(0, counter.CountWordsStartingWith("app"));
        Assert.False(counter.Erase("apple"));
    }

    [Fact]
    public void DistinguishesWordsFromPrefixes()
    {
        var counter = new PrefixCounter();
        counter.Insert("app");
        counter.Insert("apple");
        counter.Insert("apply");
        counter.Insert("banana");

        Assert.Equal(1, counter.CountWordsEqualTo("app"));
        Assert.Equal(0, counter.CountWordsEqualTo("appl"));
        Assert.Equal(3, counter.CountWordsStartingWith("app"));
        Assert.Equal(2, counter.CountWordsStartingWith("appl"));
        Assert.Equal(4, counter.CountWordsStartingWith(""));
        Assert.Equal(0, counter.CountWordsStartingWith("c"));

        Assert.False(counter.Erase("appl"));
        Assert.True(counter.Erase("app"));
        Assert.Equal(2, counter.CountWordsStartingWith("app"));
        Assert.Equal(0, counter.CountWordsEqualTo("app"));
    }

    [Fact]
    public void EmptyWordIsCounted()
    {
        var counter = new PrefixCounter();

        counter.Insert("");

        Assert.Equal(1, counter.CountWordsEqualTo(""));
        Assert.Equal(1, counter.CountWordsStartingWith(""));
        Assert.True(counter.Erase(""));
        Assert.Equal(0, counter.CountWordsStartingWith(""));
    }

    [Fact]
    public void ThrowsForNull()
    {
        var counter = new PrefixCounter();

        Assert.Throws<ArgumentNullException>(() => counter.Insert(null!));
        Assert.Throws<ArgumentNullException>(() => counter.CountWordsEqualTo(null!));
        Assert.Throws<ArgumentNullException>(() => counter.CountWordsStartingWith(null!));
        Assert.Throws<ArgumentNullException>(() => counter.Erase(null!));
    }

    [Fact]
    public void MatchesBruteForceOnRandomOperations()
    {
        var random = new Random(123);
        var counter = new PrefixCounter();
        var words = new List<string>();

        for (int i = 0; i < 3000; i++)
        {
            string word = WordGenerator.RandomWord(random, 0, 4, "ab");
            switch (random.Next(4))
            {
                case 0:
                case 1:
                    counter.Insert(word);
                    words.Add(word);
                    break;
                case 2:
                    Assert.Equal(words.Remove(word), counter.Erase(word));
                    break;
                default:
                    Assert.Equal(words.Count(w => w == word), counter.CountWordsEqualTo(word));
                    Assert.Equal(words.Count(w => w.StartsWith(word, StringComparison.Ordinal)), counter.CountWordsStartingWith(word));
                    break;
            }
        }
    }
}
