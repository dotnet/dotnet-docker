using System.Globalization;
using static System.Console;

const string nlsVar = "DOTNET_SYSTEM_GLOBALIZATION_USENLS";
const string icuVar = "ICU_EXPECTED";

GetEnvironmentVariableValue(nlsVar);
bool icuExpected = GetEnvironmentVariableValue(icuVar);

bool icuMode = IsIcuMode();
WriteLine($"Detected ICU mode: {icuMode}");

if (icuMode != icuExpected)
{
    throw new Exception($"ICU mode detected as {icuMode}, expected {icuExpected}");
}

// https://learn.microsoft.com/dotnet/core/extensions/globalization-icu#stringendswith
Assert(
    """ "abc".EndsWith("\0")""",
    "abc".EndsWith("\0").ToString(),
    icuResult: true.ToString(),
    nlsResult: false.ToString(),
    icuMode);
Assert(
    """ "abc".EndsWith("\0", StringComparison.CurrentCulture)""",
    "abc".EndsWith("\0", StringComparison.CurrentCulture).ToString(),
    icuResult: true.ToString(),
    nlsResult: false.ToString(),
    icuMode);

// https://learn.microsoft.com/dotnet/core/extensions/globalization-icu#stringstartswith
Assert(
    """ "foo".StartsWith("\0")""",
    "foo".StartsWith("\0").ToString(),
    icuResult: true.ToString(),
    nlsResult: false.ToString(),
    icuMode);
Assert(
    """ "foo".StartsWith("\0", StringComparison.CurrentCulture)""",
    "foo".StartsWith("\0", StringComparison.CurrentCulture).ToString(),
    icuResult: true.ToString(),
    nlsResult: false.ToString(),
    icuMode);

// https://learn.microsoft.com/dotnet/core/extensions/globalization-icu#stringindexof
Assert(
    """ "Hel\0lo".IndexOf("\0")""",
    "Hel\0lo".IndexOf("\0").ToString(),
    icuResult: "0",
    nlsResult: "3",
    icuMode);
Assert(
    """ "Hel\0lo".IndexOf("\0", StringComparison.CurrentCulture)""",
    "Hel\0lo".IndexOf("\0", StringComparison.CurrentCulture).ToString(),
    icuResult: "0",
    nlsResult: "3",
    icuMode);

WriteLine("All assertions passed!");

// https://learn.microsoft.com/dotnet/core/extensions/globalization-icu#determine-if-your-app-is-using-icu
bool IsIcuMode()
{
    SortVersion sortVersion = CultureInfo.InvariantCulture.CompareInfo.Version;
    byte[] bytes = sortVersion.SortId.ToByteArray();
    int version = bytes[3] << 24 | bytes[2] << 16 | bytes[1] << 8 | bytes[0];
    return version != 0 && version == sortVersion.FullVersion;
}

void Assert(string functionText, string actualResult, string icuResult, string nlsResult, bool icuExpected)
{
    string expectedResult = icuExpected ? icuResult : nlsResult;
    Console.WriteLine($"{functionText} returned {actualResult}");
    if (actualResult != expectedResult)
    {
        throw new Exception($"Assertion failed: {functionText} returned {actualResult}, expected {expectedResult}");
    }
}

bool GetEnvironmentVariableValue(string variable)
{
    string value = Environment.GetEnvironmentVariable(variable);
    bool parsedValue = !string.IsNullOrWhiteSpace(value) && bool.Parse(value);
    WriteLine($"{variable} evaluated to {parsedValue}");
    return parsedValue;
}
