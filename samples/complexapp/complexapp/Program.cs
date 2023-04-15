using libbar;
using libfoo;
using static System.Console;

namespace complexapp;

class Program
{
    static void Main(string[] args)
    {
        string initialString = StringLibrary.GetString();
        string reversedString = StringUtils.ReverseString(initialString);

        WriteLine($"string: {initialString}");
        WriteLine($"reversed: {reversedString}");
    }
}
