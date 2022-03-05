using System.Runtime.InteropServices;

public class EnvironmentInfo
{
    public EnvironmentInfo()
    {
        RuntimeVersion = RuntimeInformation.FrameworkDescription;
        OSVersion = RuntimeInformation.OSDescription;
        OSArchitecture = RuntimeInformation.OSArchitecture.ToString();
        ProcessorCount = Environment.ProcessorCount;
    }
    
    public string RuntimeVersion { get; set; }
    public string OSVersion { get; set; }
    public string OSArchitecture { get; set; }
    public int ProcessorCount { get; set; }
}
