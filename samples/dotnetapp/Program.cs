using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static System.Console;

// Variant of https://github.com/dotnet/core/tree/main/samples/dotnet-runtimeinfo
// Ascii text: https://ascii.co.uk/text (Univers font)

string nl = Environment.NewLine;

WriteLine(
$"         42{nl}" +
$"         42              ,d                             ,d{nl}" +
$"         42              42                             42{nl}" +
$" ,adPPYb,42  ,adPPYba, MM42MMM 8b,dPPYba,   ,adPPYba, MM42MMM{nl}" +
$"a8\"    `Y42 a8\"     \"8a  42    42P\'   `\"8a a8P_____42   42{nl}" +
$"8b       42 8b       d8  42    42       42 8PP\"\"\"\"\"\"\"   42{nl}" +
$"\"8a,   ,d42 \"8a,   ,a8\"  42,   42       42 \"8b,   ,aa   42,{nl}" +
$" `\"8bbdP\"Y8  `\"YbbdP\"\'   \"Y428 42       42  `\"Ybbd8\"\'   \"Y428{nl}");


// .NET information
WriteLine(RuntimeInformation.FrameworkDescription);

// OS information
const string OSRel = "/etc/os-release";
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && 
    File.Exists(OSRel))
{
  const string PrettyName = "PRETTY_NAME";
  foreach(string line in File.ReadAllLines(OSRel))
  {
      if (line.StartsWith(PrettyName))
      {
          ReadOnlySpan<char> value = line.AsSpan()[(PrettyName.Length + 2)..^1];
          WriteLine(value.ToString());
          break;
      }
  }
}
else
{
    WriteLine(RuntimeInformation.OSDescription);
}

WriteLine();

const long Mebi = 1024 * 1024;
const long Gibi = Mebi * 1024;
GCMemoryInfo gcInfo = GC.GetGCMemoryInfo();
long totalMemoryBytes = gcInfo.TotalAvailableMemoryBytes;

// Environment information
WriteLine($"{nameof(RuntimeInformation.OSArchitecture)}: {RuntimeInformation.OSArchitecture}");
WriteLine($"{nameof(Environment.ProcessorCount)}: {Environment.ProcessorCount}");
WriteLine($"{nameof(GCMemoryInfo.TotalAvailableMemoryBytes)}: {totalMemoryBytes} ({GetInBestUnit(totalMemoryBytes)})");

string[] memoryLimitPaths = new string[] 
{
    "/sys/fs/cgroup/memory.max",
    "/sys/fs/cgroup/memory/memory.limit_in_bytes",
};

string[] currentMemoryPaths = new string[] 
{
    "/sys/fs/cgroup/memory.current",
    "/sys/fs/cgroup/memory/memory.usage_in_bytes",
};

// cgroup information
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    // get memory cgroup information
    long memoryLimit = GetBestValue(memoryLimitPaths);
    long currentMemory = GetBestValue(currentMemoryPaths);

    if (memoryLimit > 0)
    {
        WriteLine($"cgroup memory limit: {memoryLimit} ({GetInBestUnit(memoryLimit)})");
        WriteLine($"cgroup memory usage: {currentMemory} ({GetInBestUnit(currentMemory)})");
        WriteLine($"GC Hard limit %:  {decimal.Divide(totalMemoryBytes,memoryLimit) * 100}");
    }
}

string GetInBestUnit(long size)
{
    if (size < Mebi)
    {
        return $"{size} bytes";
    }
    else if (size < Gibi)
    {
        decimal mebibytes = Decimal.Divide(size, Mebi);
        return $"{mebibytes:F} MiB";
    }
    else
    {
        decimal gibibytes = Decimal.Divide(size, Gibi);
        return $"{gibibytes:F} GiB";
    }
}

long GetBestValue(string[] paths)
{
    string value = string.Empty;
    foreach(string path in paths)
    {
        if (Path.Exists(path))
        {
            value = File.ReadAllText(path);
            break;
        }
    }

    if (int.TryParse(value, out int result))
    {
        return result;
    }

    return 0;
}
