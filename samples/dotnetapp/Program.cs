using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Console;

// Variant of https://github.com/dotnet/core/tree/main/samples/dotnet-runtimeinfo
// Ascii text: https://ascii.co.uk/text (Univers font)

WriteLine(
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
          break;
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

// Cgroup information
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && 
    Directory.Exists("/sys/fs/cgroup/cpu") &&
    Directory.Exists("/sys/fs/cgroup/memory"))
{
    string cpuquota = File.ReadAllLines("/sys/fs/cgroup/cpu/cpu.cfs_quota_us")[0];
    if (int.TryParse(cpuquota, out int quota) && quota > 0)
    {
        WriteLine($"cfs_quota_us: {quota}");
    }
    string usageBytes = File.ReadAllLines("/sys/fs/cgroup/memory/memory.usage_in_bytes")[0];
    string limitBytes = File.ReadAllLines("/sys/fs/cgroup/memory/memory.limit_in_bytes")[0];

    long.TryParse(usageBytes, out long usage);
    long.TryParse(limitBytes, out long limit);

    WriteLine($"usage_in_bytes: {usageBytes} {GetInBiggerUnit(usage)}");
    WriteLine($"limit_in_bytes: {limitBytes} {GetInBiggerUnit(limit)}");
}

string GetInBiggerUnit(long size)
{
    long mebi = 1048576;
    long gibi = 134217728;
    if (size < mebi)
    {
        return string.Empty;
    }
    else if (size < gibi)
    {
        long mebibytes = size / mebi;
        return $"({mebibytes} MiB)";
    }
    else
    {
        long gibibytes = size / gibi;
        return $"({gibibytes} GiB)";
    }
}