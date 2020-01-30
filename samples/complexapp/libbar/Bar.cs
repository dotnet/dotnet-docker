using System;

namespace libbar
{
    public class StringUtils
    {
        public static string ReverseString(string input)
        {
            var chars = input.ToCharArray();
            Array.Reverse(chars);
            var reversedString = new string(chars);
            return reversedString;
        }
    }
}
