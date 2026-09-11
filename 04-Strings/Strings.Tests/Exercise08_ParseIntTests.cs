namespace DsaPractice.Strings.Tests;

public class Exercise08_ParseIntTests
{
    [Theory]
    [InlineData("0", 0)]
    [InlineData("42", 42)]
    [InlineData("-42", -42)]
    [InlineData("+7", 7)]
    [InlineData("+007", 7)]
    [InlineData("-0", 0)]
    [InlineData("   123  ", 123)]
    [InlineData("\t-17\n", -17)]
    [InlineData("2147483647", int.MaxValue)]
    [InlineData("-2147483648", int.MinValue)]
    [InlineData("000000000000000000001", 1)]
    public void ParsesValidNumbers(string text, int expected)
    {
        Assert.Equal(expected, IntegerParser.Parse(text));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("+")]
    [InlineData("-")]
    [InlineData("12a")]
    [InlineData("a12")]
    [InlineData("4 2")]
    [InlineData("--5")]
    [InlineData("+-5")]
    [InlineData("1.5")]
    [InlineData("- 5")]
    public void ThrowsFormatExceptionForInvalidText(string text)
    {
        Assert.Throws<FormatException>(() => IntegerParser.Parse(text));
    }

    [Theory]
    [InlineData("2147483648")]
    [InlineData("-2147483649")]
    [InlineData("99999999999999999999")]
    [InlineData("-99999999999999999999999999")]
    public void ThrowsOverflowExceptionForOutOfRangeValues(string text)
    {
        Assert.Throws<OverflowException>(() => IntegerParser.Parse(text));
    }

    [Fact]
    public void MatchesIntParseOnRandomValues()
    {
        var random = new Random(8);
        for (int i = 0; i < 1000; i++)
        {
            int value = random.Next(int.MinValue, int.MaxValue);

            Assert.Equal(value, IntegerParser.Parse(value.ToString(System.Globalization.CultureInfo.InvariantCulture)));
        }
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => IntegerParser.Parse(null!));
    }
}
