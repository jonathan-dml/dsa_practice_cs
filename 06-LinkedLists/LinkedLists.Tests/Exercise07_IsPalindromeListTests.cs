namespace DsaPractice.LinkedLists.Tests;

public class Exercise07_IsPalindromeListTests
{
    [Theory]
    [InlineData(new int[] { }, true)]
    [InlineData(new[] { 1 }, true)]
    [InlineData(new[] { 1, 1 }, true)]
    [InlineData(new[] { 1, 2 }, false)]
    [InlineData(new[] { 1, 2, 1 }, true)]
    [InlineData(new[] { 1, 2, 2, 1 }, true)]
    [InlineData(new[] { 1, 2, 3, 2, 1 }, true)]
    [InlineData(new[] { 1, 2, 3, 1 }, false)]
    [InlineData(new[] { 1, 2, 3, 3, 1 }, false)]
    public void ChecksPalindromes(int[] values, bool expected)
    {
        Assert.Equal(expected, PalindromeList.IsPalindrome(ListNode.FromValues(values)));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 2, 1 })]
    [InlineData(new[] { 1, 2, 2, 1 })]
    [InlineData(new[] { 1, 2, 3, 4 })]
    public void LeavesTheListUnchanged(int[] values)
    {
        var head = ListNode.FromValues(values);

        PalindromeList.IsPalindrome(head);

        Assert.Equal(values, ListNode.ToArray(head));
    }

    [Fact]
    public void HandlesLongLists()
    {
        int[] half = Enumerable.Range(0, 250_000).ToArray();
        int[] palindrome = [.. half, .. half.Reverse()];
        int[] almost = [.. palindrome];
        almost[^10] = -1;

        PerformanceAssert.CompletesWithin(TimeSpan.FromSeconds(2), () =>
        {
            Assert.True(PalindromeList.IsPalindrome(ListNode.FromValues(palindrome)));
            Assert.False(PalindromeList.IsPalindrome(ListNode.FromValues(almost)));
        });
    }
}
