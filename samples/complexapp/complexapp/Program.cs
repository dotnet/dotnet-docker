using System;
using libbar;
using libfoo;
using static System.Console;

namespace complexapp
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialString = StringLibrary.GetString();
            var reversedString = StringUtils.ReverseString(initialString);

            WriteLine($"string: {initialString}");
            WriteLine($"reversed: {reversedString}");
        }
    }
}
