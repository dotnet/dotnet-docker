using libfoo;
using Xunit;

namespace tests;

public class StringLibraryTests
{
    [Fact]
    public void InputString_ReturnsCorrectString()
    {
        var inputString = "The quick brown fox jumps over the lazy dog";
        var returnedString = StringLibrary.GetString();
        Assert.True(inputString == returnedString, "The input string was not correct.");
    }
}
