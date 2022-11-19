using System.Runtime.InteropServices;

public struct EnvironmentInfo
{
    public EnvironmentInfo()
    {
        GCMemoryInfo gcInfo = GC.GetGCMemoryInfo();
        TotalAvailableMemoryBytes = gcInfo.TotalAvailableMemoryBytes;

        if (!RuntimeInformation.OSDescription.StartsWith("Linux"))
        {
            return;
        }

        string[] memoryLimits = new string[] 
        {
            "/sys/fs/cgroup/memory.max",
            "/sys/fs/cgroup/memory/memory.limit_in_bytes",
        };

        string[] currentMemory = new string[] 
        {
            "/sys/fs/cgroup/memory.current",
            "/sys/fs/cgroup/memory/memory.usage_in_bytes",
        };

        MemoryLimit = GetBestValue(memoryLimits);
        MemoryUsage = GetBestValue(currentMemory);
    }

    public string RuntimeVersion => RuntimeInformation.FrameworkDescription;
    public string OSVersion => RuntimeInformation.OSDescription;
    public string OSArchitecture => RuntimeInformation.OSArchitecture.ToString();
    public string User => Environment.UserName;
    public int ProcessorCount => Environment.ProcessorCount;
    public long TotalAvailableMemoryBytes { get; init; }
    public long MemoryLimit { get; init; }
    public long MemoryUsage { get; init; }

    private long GetBestValue(string[] paths)
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
}
