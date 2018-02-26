using System;
using Xunit;
using Utils;

namespace Tests
{
    public class ConsoleUtilTests
    {
        [Fact]
        public void PrintStringWithColor()
        {
            var color = Console.ForegroundColor;
            ConsoleUtils.PrintStringWithColor("test text.", ConsoleColor.DarkGreen);
            Assert.True(color == Console.ForegroundColor, "The input string was not reversed correctly.");
        }

        [Fact]
        public void PrintStringWithRandomColor()
        {
            var color = Console.ForegroundColor;
            ConsoleUtils.PrintStringWithRandomColor("test text 1\ntest text 2\ntest text 3");
            Assert.True(color == Console.ForegroundColor, "The input string was not reversed correctly.");
        }
    }
}
