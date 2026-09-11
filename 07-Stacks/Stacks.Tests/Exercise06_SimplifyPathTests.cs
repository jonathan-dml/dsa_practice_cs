namespace DsaPractice.Stacks.Tests;

public class Exercise06_SimplifyPathTests
{
    [Theory]
    [InlineData("/", "/")]
    [InlineData("/home", "/home")]
    [InlineData("/home/", "/home")]
    [InlineData("/../", "/")]
    [InlineData("/..", "/")]
    [InlineData("/.", "/")]
    [InlineData("/home//foo/", "/home/foo")]
    [InlineData("/a/./b/../../c/", "/c")]
    [InlineData("/a/../../b/../c//.//", "/c")]
    [InlineData("/a//b////c/d//././/..", "/a/b/c")]
    [InlineData("/...", "/...")]
    [InlineData("/.hidden/..config/", "/.hidden/..config")]
    [InlineData("///usr///local///bin///", "/usr/local/bin")]
    public void SimplifiesPaths(string path, string expected)
    {
        Assert.Equal(expected, PathSimplifier.Simplify(path));
    }

    [Theory]
    [InlineData("")]
    [InlineData("home/user")]
    [InlineData("./a")]
    public void ThrowsForRelativePaths(string path)
    {
        Assert.Throws<ArgumentException>(() => PathSimplifier.Simplify(path));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => PathSimplifier.Simplify(null!));
    }
}
