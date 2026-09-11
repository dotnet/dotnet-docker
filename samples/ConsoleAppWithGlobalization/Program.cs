using System.Globalization;
using static System.Console;

// Hello world, indeed
WriteLine("Hello, World!");

DateTime nowUtc = DateTime.UtcNow;
DateTime now = DateTime.Now;

// Accessing UTC and Local time will always work
PrintHeader("Print baseline timezones");
WriteLine($"{nameof(TimeZoneInfo.Utc)}: {TimeZoneInfo.Utc}; {nowUtc}");
// Local will match UTC unless `$TZ` or `/etc/timezone` is set to another timezone
// This is demonstrated in the Dockerfile
WriteLine($"{nameof(TimeZoneInfo.Local)}: {TimeZoneInfo.Local}; {now}");

// Code after this point:
// Requires ICU and tzdata be installed
// Will not work with Globalization invariant mode
// https://aka.ms/GlobalizationInvariantMode

PrintHeader("Print specific timezone");
string home = "America/Los_Angeles";
var tz = TimeZoneInfo.FindSystemTimeZoneById(home);
DateTime nowAtHome = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, tz);
WriteLine($"Home timezone: {home}");
WriteLine($"DateTime at home: {nowAtHome}");

CultureInfo[] cultures =
{
    new CultureInfo("en-US"),
    new CultureInfo("en-CA"),
    new CultureInfo("fr-CA"),
    new CultureInfo("hr-HR"),
    new CultureInfo("jp-JP"),
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

// https://devblogs.microsoft.com/dotnet/handling-a-new-era-in-the-japanese-calendar-in-net/
// Test Japanese calendar
PrintHeader("Japanese calendar");
var cal = new JapaneseCalendar();
var jaJP = new CultureInfo("ja-JP");
jaJP.DateTimeFormat.Calendar = cal;

var date = cal.ToDateTime(31, 8, 18, 0, 0, 0, 0, 4);
WriteLine($"{date:d}");
WriteLine($"{date.ToString("d", jaJP)}");

var date89 = new DateTime(1989, 8, 18);
jaJP.DateTimeFormat.Calendar = cal;
CultureInfo.CurrentCulture = jaJP;

WriteLine($"{date89:ggy年M月d日}");
WriteLine($"{date89:ggy'年'M'月'd'日'}");

PrintHeader("String comparison");
WriteLine("Comparison results: `0` mean equal, `-1` is less than and `1` is greater");
CompareInfo turkishCompare = new CultureInfo("tr-TR").CompareInfo;
CompareInfo englishCompare = new CultureInfo("en-US").CompareInfo;

WriteLine("Test: compare i to (Turkish) \u0130; first test should be equal and second not");
WriteLine(turkishCompare.Compare("i", "\u0130", CompareOptions.IgnoreCase)); // print 0 as "i" is equal to Turkish I "\u0130".
WriteLine(englishCompare.Compare("i", "\u0130", CompareOptions.IgnoreCase)); // print -1 as "i" is not equal to Turkish I "\u0130" in non-Turkish cultures.

// Å LATIN CAPITAL LETTER A WITH RING ABOVE is equal to 'A'  + '\u030A'  
WriteLine("Test: compare \u00C5 A\u030A; should be equal");
WriteLine(englishCompare.Compare("\u00C5", "A\u030A"));
