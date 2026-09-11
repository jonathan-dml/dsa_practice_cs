namespace DsaPractice.Recursion.Tests;

public class Exercise08_FlattenNestedTests
{
    [Fact]
    public void ReturnsFlatInputUnchanged()
    {
        Assert.Equal([1, 2, 3], NestedFlattener.Flatten([1, 2, 3]));
    }

    [Fact]
    public void FlattensNestedArrays()
    {
        object[] input = [1, new object[] { 2, new object[] { 3, 4 } }, 5];

        Assert.Equal([1, 2, 3, 4, 5], NestedFlattener.Flatten(input));
    }

    [Fact]
    public void FlattensMixedCollectionTypes()
    {
        object[] input = [new List<object> { 1, new object[] { 2 } }, 3, new List<object> { new List<object> { 4, 5 } }];

        Assert.Equal([1, 2, 3, 4, 5], NestedFlattener.Flatten(input));
    }

    [Fact]
    public void HandlesEmptyNestedCollections()
    {
        object[] input = [new object[] { }, new object[] { new object[] { } }];

        Assert.Empty(NestedFlattener.Flatten(input));
    }

    [Fact]
    public void HandlesDeepNesting()
    {
        object current = new object[] { 42 };
        for (int i = 0; i < 500; i++)
        {
            current = new object[] { i, current };
        }

        var result = NestedFlattener.Flatten((object[])current);

        Assert.Equal(501, result.Count);
        Assert.Equal(499, result[0]);
        Assert.Equal(42, result[^1]);
    }

    [Fact]
    public void ThrowsForUnsupportedElements()
    {
        object[] input = [1, "two", 3];

        Assert.Throws<ArgumentException>(() => NestedFlattener.Flatten(input));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => NestedFlattener.Flatten(null!));
    }
}
