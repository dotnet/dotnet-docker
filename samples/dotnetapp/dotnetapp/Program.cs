using System;
using System.Runtime.InteropServices;
using Utils;
using System.IO;
using static System.Console;

public static class Program
{
  public static void Main(string[] args) 
  {
        string message = "Hello .NET Framework!";
          
        if (args.Length > 0) 
        {
          message = String.Join(" ",args);
        }

        var bot = $"    {message}{GetBot()}";

        ConsoleUtils.PrintStringWithRandomColor(bot);

        WriteLine("**Environment**");
        WriteLine($"Platform: .NET Framework 4.7.1");
        WriteLine($"OS: {RuntimeInformation.OSDescription}");
        WriteLine();
  }

  public static string GetBot() 
  {
          
          return @"
    __________________
                      \
                       \
                          ....
                          ....'
                           ....
                        ..........
                    .............'..'..
                 ................'..'.....
               .......'..........'..'..'....
              ........'..........'..'..'.....
             .'....'..'..........'..'.......'.
             .'..................'...   ......
             .  ......'.........         .....
             .                           ......
            ..    .            ..        ......
           ....       .                 .......
           ......  .......          ............
            ................  ......................
            ........................'................
           ......................'..'......    .......
        .........................'..'.....       .......
     ........    ..'.............'..'....      ..........
   ..'..'...      ...............'.......      ..........
  ...'......     ...... ..........  ......         .......
 ...........   .......              ........        ......
.......        '...'.'.              '.'.'.'         ....
.......       .....'..               ..'.....
   ..       ..........               ..'........
          ............               ..............
         .............               '..............
        ...........'..              .'.'............
       ...............              .'.'.............
      .............'..               ..'..'...........
      ...............                 .'..............
       .........                        ..............
        .....

";
  }
}
