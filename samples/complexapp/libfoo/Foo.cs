using System;

namespace libfoo
{
    public class StringLibrary
    {
        private const string Phrase = "The quick brown fox jumps over the lazy dog";
        public static string GetString() => Phrase;
#if NET5_0
        public static ReadOnlySpan<char> GetSpan() => Phrase.AsSpan();
#endif
    }
}
