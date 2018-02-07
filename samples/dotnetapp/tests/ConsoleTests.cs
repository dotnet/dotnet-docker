using System;
using Xunit;
using Utils;

namespace Tests
{
    public class ConsoleTests
    {
        [Fact]
        public void PrintStringWithColor()
        {
            var color = Console.ForegroundColor;
            ConsoleUtils.PrintStringWithColor("asdsad", ConsoleColor.DarkGreen);
            Assert.True(color == Console.ForegroundColor, "The input string was not reversed correctly.");
        }

        [Fact]
        public void PrintStringWithRandomColor()
        {
            var color = Console.ForegroundColor;
            ConsoleUtils.PrintStringWithRandomColor("asdsad\nasdsadsadasd\nadsdfsadsad");
            Assert.True(color == Console.ForegroundColor, "The input string was not reversed correctly.");
        }
    }
}
