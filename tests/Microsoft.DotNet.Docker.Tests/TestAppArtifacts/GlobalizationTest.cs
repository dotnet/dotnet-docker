using System.Globalization;
using static System.Console;

string envVar = Environment.GetEnvironmentVariable("DOTNET_SYSTEM_GLOBALIZATION_INVARIANT") ?? "null";

bool globalizationInvariantModeEnabled = IsGlobalizationInvariantModeEnabled();
if (globalizationInvariantModeEnabled)
{
    if (envVar != "true" && envVar != "1")
    {
        throw new Exception("Globalization invariant mode is enabled, but the environment variable is not set to true or 1.");
    }

    Console.WriteLine("Globalization invariant mode is enabled.");

    try
    {
        TestGlobalizationFunctionality();
        TestTimeZoneFunctionality();
    }
    catch (CultureNotFoundException)
    {
        WriteLine("Successfully caught a CultureNotFoundException, invariant mode is working as expected.");
    }
}
else
{
    WriteLine("Globalization invariant mode is disabled. The following should not throw any exceptions:");
    TestGlobalizationFunctionality();
    TestTimeZoneFunctionality();
}

WriteLine("Globalization test succeeded");

void TestGlobalizationFunctionality()
{
    const int Value = 1337;
    WriteLine($"Value: {Value}");
    WriteLine($"  en-US: {Value.ToString("c", new CultureInfo("en-US"))}");
    WriteLine($"  jp-JP: {Value.ToString("c", new CultureInfo("jp-JP"))}");
}

void TestTimeZoneFunctionality()
{
    DateTime localTime = DateTime.Now;
    TimeZoneInfo pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
    DateTime pacificTime = TimeZoneInfo.ConvertTime(localTime, pacificZone);
    WriteLine("Local Time: " + localTime);
    WriteLine("Pacific Time: " + pacificTime);
}

// https://stackoverflow.com/a/75299176
bool IsGlobalizationInvariantModeEnabled()
{
    try
    {
        return CultureInfo.GetCultureInfo("en-US").NumberFormat.CurrencySymbol == "¤";
    }
    catch (CultureNotFoundException)
    {
        return true;
    }
}
