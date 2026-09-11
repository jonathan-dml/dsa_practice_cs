namespace DsaPractice.Strings.Tests;

public class Exercise07_CaesarCipherTests
{
    [Theory]
    [InlineData("", 3, "")]
    [InlineData("abc", 0, "abc")]
    [InlineData("abc", 1, "bcd")]
    [InlineData("xyz", 2, "zab")]
    [InlineData("abc", -1, "zab")]
    [InlineData("abc", 26, "abc")]
    [InlineData("abc", 29, "def")]
    [InlineData("abc", -27, "zab")]
    [InlineData("Hello, World!", 3, "Khoor, Zruog!")]
    [InlineData("ABC xyz 123 é", 13, "NOP klm 123 é")]
    public void EncryptsText(string text, int shift, string expected)
    {
        Assert.Equal(expected, CaesarCipher.Encrypt(text, shift));
    }

    [Theory]
    [InlineData("Khoor, Zruog!", 3, "Hello, World!")]
    [InlineData("zab", 2, "xyz")]
    [InlineData("zab", -1, "abc")]
    public void DecryptsText(string text, int shift, string expected)
    {
        Assert.Equal(expected, CaesarCipher.Decrypt(text, shift));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(25)]
    [InlineData(-100)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void DecryptUndoesEncrypt(int shift)
    {
        const string text = "The Quick Brown Fox Jumps Over The Lazy Dog, 42 times!";

        Assert.Equal(text, CaesarCipher.Decrypt(CaesarCipher.Encrypt(text, shift), shift));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => CaesarCipher.Encrypt(null!, 1));
        Assert.Throws<ArgumentNullException>(() => CaesarCipher.Decrypt(null!, 1));
    }
}
