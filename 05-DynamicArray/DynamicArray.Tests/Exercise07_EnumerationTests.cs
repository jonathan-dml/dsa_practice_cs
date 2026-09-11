using static DsaPractice.DynamicArray.Tests.ListTestHelpers;

namespace DsaPractice.DynamicArray.Tests;

public class Exercise07_EnumerationTests
{
    [Fact]
    public void ForeachYieldsItemsInOrder()
    {
        var list = ListOf(3, 1, 4, 1, 5);
        var seen = new List<int>();

        foreach (int item in list) seen.Add(item);

        Assert.Equal([3, 1, 4, 1, 5], seen);
    }

    [Fact]
    public void EmptyListYieldsNothing()
    {
        Assert.Empty(new MyList<string>(10));
    }

    [Fact]
    public void YieldsOnlyCountItems()
    {
        var list = ListOf(1, 2, 3);
        list.RemoveAt(2);

        Assert.Equal([1, 2], list);
    }

    [Fact]
    public void WorksWithLinq()
    {
        var list = ListOf(1, 2, 3, 4);

        Assert.Equal(10, list.Sum());
        Assert.Equal([2, 4], list.Where(x => x % 2 == 0));
    }

    [Fact]
    public void EnumeratorsAreIndependent()
    {
        var list = ListOf("a", "b");
        using var first = list.GetEnumerator();
        using var second = list.GetEnumerator();

        Assert.True(first.MoveNext());
        Assert.True(first.MoveNext());
        Assert.True(second.MoveNext());

        Assert.Equal("b", first.Current);
        Assert.Equal("a", second.Current);
    }

    public static TheoryData<string, Action<MyList<int>>> Modifications => new()
    {
        { "Add", l => l.Add(9) },
        { "Insert", l => l.Insert(0, 9) },
        { "RemoveAt", l => l.RemoveAt(0) },
        { "Remove", l => l.Remove(3) },
        { "Clear", l => l.Clear() },
        { "Reverse", l => l.Reverse() },
        { "Indexer", l => l[0] = 9 },
    };

    [Theory]
    [MemberData(nameof(Modifications))]
    public void ThrowsWhenModifiedDuringEnumeration(string name, Action<MyList<int>> modify)
    {
        Assert.NotNull(name);
        var list = ListOf(1, 2, 3);

        Assert.Throws<InvalidOperationException>(() =>
        {
            foreach (int _ in list) modify(list);
        });
    }

    [Fact]
    public void ThrowsWhenModifiedAfterTheLastItem()
    {
        var list = ListOf(1, 2);
        using var enumerator = list.GetEnumerator();
        Assert.True(enumerator.MoveNext());
        Assert.True(enumerator.MoveNext());

        list.Add(3);

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }
}
