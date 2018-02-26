using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Utils;

namespace Tests
{
    public class StringUtilsTests
    {

        [Fact]
        public void ReverseString()
        {
            var inputString = "The quick brown fox jumps over the lazy dog";
            var expectedString = "god yzal eht revo spmuj xof nworb kciuq ehT";
            var returnedString = StringUtils.ReverseString(inputString);
            Assert.True(expectedString == returnedString, "The input string was not reversed correctly.");
        }
    }
}
