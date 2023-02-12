using System.Globalization;
using static System.Console;

// Hello world, indeed
WriteLine("Hello, World!");

DateTime nowUtc = DateTime.UtcNow;
DateTime now = DateTime.UtcNow;

// Accessing UTC and Local time will always work
PrintHeader("Print baseline timezones");
WriteLine($"{nameof(TimeZoneInfo.Utc)}: {TimeZoneInfo.Utc}; {nowUtc}");
// Local will match UTC unless `/etc/timezone` is set
// This is demonstrated in the Dockerfile
WriteLine($"{nameof(TimeZoneInfo.Local)}: {TimeZoneInfo.Local}; {now}");

// Code after this point:
// Requires ICU be installed
// will not with Globalization invariant mode
// https://aka.ms/GlobalizationInvariantMode

// This code requires tzdata
PrintHeader("Print specific timezone");
string home = "America/Los_Angeles";
var tz = TimeZoneInfo.FindSystemTimeZoneById(home);
DateTime nowAtHome = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, tz);
WriteLine($"Home timezone: {home}");
WriteLine($"DateTime at home: {nowAtHome}");

CultureInfo[] cultures =
{
    new CultureInfo("en-us"),
    new CultureInfo("en-ca"),
    new CultureInfo("hr-HR"),
    new CultureInfo("ko-KR"),
    new CultureInfo("pt-BR"),
    new CultureInfo("zh-CN")
};

// Print date in correct formats
PrintHeader("Culture-specific dates");
WriteLine($"Current: {nowUtc.ToString("d")}");
foreach (var culture in cultures)
{
    PrintDateWithCulture(nowUtc, culture);
}

void PrintDateWithCulture(DateTime dt, CultureInfo culture)
{
    WriteLine($"{culture.DisplayName} -- {culture}:");
    WriteLine(dt.ToString(culture));
    WriteLine(dt.ToString("d", culture));
    WriteLine(dt.ToString("t", culture));
}

PrintHeader("Culture-specific currency:");
var currencyValue = 1337;
WriteLine($"Current: {currencyValue.ToString("c")}");
foreach (var culture in cultures)
{
    PrintValueWithCurrency(currencyValue, culture);
}

void PrintValueWithCurrency(int value, CultureInfo culture)
{
    WriteLine($"{culture}: {value.ToString("c", culture)}");
}

void PrintHeader(string title)
{
    WriteLine();
    WriteLine($"****{title}**");
}