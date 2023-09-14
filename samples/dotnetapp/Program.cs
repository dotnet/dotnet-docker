using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using static System.Console;

// Variant of https://github.com/dotnet/core/tree/main/samples/dotnet-runtimeinfo
// Ascii text: https://ascii.co.uk/text (Univers font)

WriteLine(""""""""
         88
         88              ,d                             ,d
         88              88                             88
 ,adPPYb,88  ,adPPYba, MM88MMM 8b,dPPYba,   ,adPPYba, MM88MMM
a8"    `Y88 a8"     "8a  88    88P'   `"8a a8P_____88   88
8b       88 8b       d8  88    88       88 8PP"""""""   88
"8a,   ,d88 "8a,   ,a8"  88,   88       88 "8b,   ,aa   88,
 `"8bbdP"Y8  `"YbbdP"'   "Y888 88       88  `"Ybbd8"'   "Y888

"""""""");

const long Mebi = 1024 * 1024;
const long Gibi = Mebi * 1024;
GCMemoryInfo gcInfo = GC.GetGCMemoryInfo();
long totalMemoryBytes = gcInfo.TotalAvailableMemoryBytes;

// OS and .NET information
WriteLine($"{nameof(RuntimeInformation.OSArchitecture)}: {RuntimeInformation.OSArchitecture}");
WriteLine($"{nameof(RuntimeInformation.OSDescription)}: {RuntimeInformation.OSDescription}");
WriteLine($"{nameof(RuntimeInformation.FrameworkDescription)}: {RuntimeInformation.FrameworkDescription}");
WriteLine();

// Environment information
WriteLine($"{nameof(Environment.UserName)}: {Environment.UserName}");
WriteLine($"HostName : {Dns.GetHostName()}");
WriteLine();

// Hardware information
WriteLine($"{nameof(Environment.ProcessorCount)}: {Environment.ProcessorCount}");
WriteLine($"{nameof(GCMemoryInfo.TotalAvailableMemoryBytes)}: {totalMemoryBytes} ({GetInBestUnit(totalMemoryBytes)})");

string[] memoryLimitPaths = new string[]
{
    "/sys/fs/cgroup/memory.max",
    "/sys/fs/cgroup/memory.high",
    "/sys/fs/cgroup/memory.low",
    "/sys/fs/cgroup/memory/memory.limit_in_bytes",
};

string[] currentMemoryPaths = new string[]
{
    "/sys/fs/cgroup/memory.current",
    "/sys/fs/cgroup/memory/memory.usage_in_bytes",
};

// cgroup information
if (OperatingSystem.IsLinux() &&
    GetBestValue(memoryLimitPaths, out long memoryLimit, out string? bestMemoryLimitPath) &&
    memoryLimit > 0)
{
    // get memory cgroup information
    GetBestValue(currentMemoryPaths, out long currentMemory, out string? memoryPath);

    WriteLine($"cgroup memory constraint: {bestMemoryLimitPath}");
    WriteLine($"cgroup memory limit: {memoryLimit} ({GetInBestUnit(memoryLimit)})");
    WriteLine($"cgroup memory usage: {currentMemory} ({GetInBestUnit(currentMemory)})");
    WriteLine($"GC Hard limit %: {(double)totalMemoryBytes/memoryLimit * 100:N0}");
}

string GetInBestUnit(long size)
{
    if (size < Mebi)
    {
        return $"{size} bytes";
    }
    else if (size < Gibi)
    {
        double mebibytes = (double)(size / Mebi);
        return $"{mebibytes:F} MiB";
    }
    else
    {
        double gibibytes = (double)(size / Gibi);
        return $"{gibibytes:F} GiB";
    }
}

bool GetBestValue(string[] paths, out long limit, [NotNullWhen(true)] out string? bestPath)
{
    foreach (string path in paths)
    {
        if (Path.Exists(path) &&
            long.TryParse(File.ReadAllText(path), out limit))
        {
            bestPath = path;
            return true;
        }
    }

    bestPath = null;
    limit = 0;
    return false;
}
