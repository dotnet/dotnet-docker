using System.Globalization;
using static System.Console;

const string envVarName = "DOTNET_SYSTEM_GLOBALIZATION_INVARIANT";
string envVarValue = Environment.GetEnvironmentVariable(envVarName) ?? "null";
bool invariantModeEnvVarSetting = envVarValue == "true" || envVarValue == "1";
bool invariantModeEnabled = IsInvariantModeEnabled();

string isEnabledString = $"Globalization invariant mode is {(invariantModeEnabled ? "enabled" : "disabled")}";
WriteLine(isEnabledString);

if (invariantModeEnabled != invariantModeEnvVarSetting)
{
    throw new Exception("Environment variable mis-match: " + isEnabledString
        + $", but {envVarName} is set to `{envVarValue}`, which evaluates to {invariantModeEnvVarSetting}.");
}

try
{
    WriteLine($"The following should {(invariantModeEnabled ? "" : "not ")}produce an exception:");
    TestGlobalizationFunctionality();
    TestTimeZoneFunctionality();
    if (invariantModeEnabled)
    {
        throw new Exception("Expected an exception when testing globalization functionality but one did not occur.");
    }
}
catch (CultureNotFoundException)
{
    if (invariantModeEnabled)
    {
        WriteLine("Successfully caught a CultureNotFoundException, invariant mode is working as expected.");
    }
    else
    {
        throw;
    }
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
bool IsInvariantModeEnabled()
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
