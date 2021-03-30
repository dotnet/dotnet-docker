using System;
using Xunit;
using libbar;
using libfoo;

namespace tests
{
    public class StringTests
    {
        private const string InputString = "The quick brown fox jumps over the lazy dog";
        [Fact]
        public void ReverseString()
        {
            string expectedString = "god yzal eht revo spmuj xof nworb kciuq ehT";
            string returnedString = StringUtils.ReverseString(InputString);
            Assert.True(expectedString == returnedString, "The input string was not reversed correctly.");
        }

        [Fact]
        public void SourceString()
        {
            string returnedString = StringLibrary.GetString();
            Assert.True(InputString == returnedString, $"The {nameof(StringLibrary)} string was not correct.");
        }

#if NET5_0
        [Fact]
        public void SourceSpan()
        {
            ReadOnlySpan<char> returnedString = StringLibrary.GetSpan();
            Assert.True(InputString == returnedString, $"The {nameof(StringLibrary)} string was not correct.");
        }
#endif
    }
}
