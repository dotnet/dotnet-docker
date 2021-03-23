using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Console;
using static System.IO.File;

// Variant of https://github.com/dotnet/core/tree/main/samples/dotnet-runtimeinfo

WriteLine("\r\n" +
"         42\r\n" +
"         42              ,d                             ,d\r\n" +  
"         42              42                             42\r\n" +
" ,adPPYb,42  ,adPPYba, MM42MMM 8b,dPPYba,   ,adPPYba, MM42MMM\r\n" +
"a8\"    `Y42 a8\"     \"8a  42    42P\'   `\"8a a8P_____42   42\r\n" +  
"8b       42 8b       d8  42    42       42 8PP\"\"\"\"\"\"\"   42\r\n" +    
"\"8a,   ,d42 \"8a,   ,a8\"  42,   42       42 \"8b,   ,aa   42,\r\n" +
" `\"8bbdP\"Y8  `\"YbbdP\"\'   \"Y428 42       42  `\"Ybbd8\"\'   \"Y428\r\n");


// .NET information
WriteLine(RuntimeInformation.FrameworkDescription);

// OS information
string osrel = "/etc/os-release";
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && 
    File.Exists(osrel))
{
  string prettyname = "PRETTY_NAME";
  foreach(string line in File.ReadAllLines(osrel))
  {
      if (line.StartsWith(prettyname))
      {
          ReadOnlySpan<char> value = line.AsSpan()[(prettyname.Length + 2)..^1];
          Console.WriteLine(value.ToString());
      }
  }
}
else
{
    WriteLine(RuntimeInformation.OSDescription);
}

WriteLine();

// Environment information
WriteLine($"{nameof(RuntimeInformation.OSArchitecture)}: {RuntimeInformation.OSArchitecture}");
WriteLine($"{nameof(Environment.ProcessorCount)}: {Environment.ProcessorCount}");

if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && 
    Directory.Exists("/sys/fs/cgroup/cpu") &&
    Directory.Exists("/sys/fs/cgroup/memory"))
{
    WriteLine($"cfs_quota_us: {ReadAllLines("/sys/fs/cgroup/cpu/cpu.cfs_quota_us")[0]}");
    WriteLine($"memory.limit_in_bytes: {ReadAllLines("/sys/fs/cgroup/memory/memory.limit_in_bytes")[0]}");
    WriteLine($"memory.usage_in_bytes: {ReadAllLines("/sys/fs/cgroup/memory/memory.usage_in_bytes")[0]}");
}

