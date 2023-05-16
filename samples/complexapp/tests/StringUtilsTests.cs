using System;
using libbar;
using Xunit;

namespace tests;

public class StringUtilsTests
{
    [Fact]
    public void ReverseString_WhenInputIsNull_ThrowsCorrectException()
    {
        string inputString = null;
        Assert.Throws<ArgumentNullException>(() => StringUtils.ReverseString(inputString));
    }

    [Fact]
    public void ReverseString_ReturnsCorrectResult()
    {
        var inputString = "The quick brown fox jumps over the lazy dog";
        var expectedString = "god yzal eht revo spmuj xof nworb kciuq ehT";
        var returnedString = StringUtils.ReverseString(inputString);
        Assert.True(expectedString == returnedString, "The input string was not reversed correctly.");
    }
}
