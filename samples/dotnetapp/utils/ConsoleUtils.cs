using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Utils
{
    public static class ConsoleUtils
    {
        public static void PrintStringWithColor(string input, ConsoleColor color)
        {
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(input);
            Console.ForegroundColor = tempColor;
        }

        public static void PrintStringWithRandomColor(string input)
        {
            var line = string.Empty;
            var reader = new StringReader(input);
            var random = new Random();
            var index = random.Next(10)+2;

            while ((line = reader.ReadLine()) != null)
            {
                index = (index % 14) + 2;
                var color = (ConsoleColor)index++;
                PrintStringWithColor(line, color);
            }
        }
    }
}
