namespace DsaPractice.Tries.Tests;

public class Exercise04_DeletableTrieTests
{
    [Fact]
    public void DeletingPrunesUnusedNodes()
    {
        var trie = new DeletableTrie();

        Assert.True(trie.Insert("apple"));
        Assert.True(trie.Insert("app"));
        Assert.Equal(5, trie.NodeCount);

        Assert.True(trie.Delete("apple"));
        Assert.Equal(3, trie.NodeCount);
        Assert.True(trie.Contains("app"));
        Assert.False(trie.Contains("apple"));

        Assert.True(trie.Delete("app"));
        Assert.Equal(0, trie.NodeCount);
        Assert.False(trie.Contains("app"));
    }

    [Fact]
    public void KeepsNodesSharedByOtherWords()
    {
        var trie = new DeletableTrie();
        trie.Insert("ab");
        trie.Insert("abc");
        trie.Insert("abd");
        Assert.Equal(4, trie.NodeCount);

        Assert.True(trie.Delete("abc"));
        Assert.Equal(3, trie.NodeCount);

        Assert.True(trie.Delete("ab"));
        Assert.Equal(3, trie.NodeCount);
        Assert.False(trie.Contains("ab"));
        Assert.True(trie.Contains("abd"));

        Assert.True(trie.Delete("abd"));
        Assert.Equal(0, trie.NodeCount);
    }

    [Fact]
    public void InsertAndDeleteReportChanges()
    {
        var trie = new DeletableTrie();

        Assert.True(trie.Insert("word"));
        Assert.False(trie.Insert("word"));
        Assert.False(trie.Delete("wor"));
        Assert.False(trie.Delete("words"));
        Assert.False(trie.Delete("other"));
        Assert.Equal(4, trie.NodeCount);
        Assert.True(trie.Delete("word"));
        Assert.False(trie.Delete("word"));
    }

    [Fact]
    public void EmptyStringUsesNoNodes()
    {
        var trie = new DeletableTrie();

        Assert.True(trie.Insert(""));
        Assert.True(trie.Contains(""));
        Assert.Equal(0, trie.NodeCount);
        Assert.True(trie.Delete(""));
        Assert.False(trie.Contains(""));
    }

    [Fact]
    public void ThrowsForNull()
    {
        var trie = new DeletableTrie();

        Assert.Throws<ArgumentNullException>(() => trie.Insert(null!));
        Assert.Throws<ArgumentNullException>(() => trie.Contains(null!));
        Assert.Throws<ArgumentNullException>(() => trie.Delete(null!));
    }

    [Fact]
    public void NodeCountMatchesDistinctPrefixesOnRandomOperations()
    {
        var random = new Random(128);
        var trie = new DeletableTrie();
        var words = new HashSet<string>();

        for (int i = 0; i < 3000; i++)
        {
            string word = WordGenerator.RandomWord(random, 0, 5, "abc");
            if (random.Next(2) == 0)
            {
                Assert.Equal(words.Add(word), trie.Insert(word));
            }
            else
            {
                Assert.Equal(words.Remove(word), trie.Delete(word));
            }

            Assert.Equal(words.Contains(word), trie.Contains(word));
            int expectedNodes = words.SelectMany(w => Enumerable.Range(1, w.Length).Select(len => w[..len])).Distinct().Count();
            Assert.Equal(expectedNodes, trie.NodeCount);
        }
    }
}
