using libbar;
using libfoo;
using static System.Console;

string initialString = StringLibrary.GetString();
string reversedString = StringUtils.ReverseString(initialString);

WriteLine($"string: {initialString}");
WriteLine($"reversed: {reversedString}");
