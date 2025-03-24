namespace OtlpTestListener.Extensions;

public static class Helpers
{
    public static string? GetServiceId(this Resource resource) =>
        resource.Attributes
            .FirstOrDefault(attr => attr.Key == OtlpApplication.SERVICE_INSTANCE_ID)
            ?.Value
            .ValueString();

    public static bool FindStringValue(this RepeatedField<KeyValue> attributes, string key, out string? value)
    {
        value = null;
        foreach (var kv in attributes)
        {
            if (kv.Key == key)
            {
                value = kv.Value.ValueString();
                return true;
            }
        }
        return false;
    }

    public static string FindStringValueOrDefault(this RepeatedField<KeyValue> attributes, string key, string defaultValue)
    {
        foreach (var kv in attributes)
        {
            if (kv.Key == key)
            {
                return kv.Value.ValueString();
            }
        }
        return defaultValue;
    }

    public static string Left(this string value, int length) =>
        value.Length <= length ? value : value[..length];

    public static string Right(this string value, int length) =>
        value.Length <= length ? value : value.Substring(value.Length - length, length);

    public static string HtmlEncode(this string text) =>
        HttpUtility.HtmlEncode(text);

    public static string ValueString(this AnyValue value) =>
        value.ValueCase switch
        {
            AnyValue.ValueOneofCase.StringValue => value.StringValue,
            AnyValue.ValueOneofCase.IntValue => value.IntValue.ToString(),
            AnyValue.ValueOneofCase.DoubleValue => value.DoubleValue.ToString(),
            AnyValue.ValueOneofCase.BoolValue => value.BoolValue.ToString(),
            AnyValue.ValueOneofCase.BytesValue => value.BytesValue.ToHexString(),
            _ => value.ToString(),
        };

    public static Dictionary<string, string> ToDictionary(this RepeatedField<KeyValue> attributes)
    {
        var dict = new Dictionary<string, string>();
        for (var i = attributes.Count - 1; i >= 0; i--)
        {
            var kv = attributes[i];
            dict[kv.Key] = kv.Value.ValueString();
        }
        return dict;
    }

    public static string ConcatString(this IReadOnlyDictionary<string, string> dict)
    {
        StringBuilder sb = new();
        var first = true;
        foreach (var kv in dict)
        {
            if (!first)
            {
                sb.Append(", ");
            }
            first = false;
            sb.Append($"{kv.Key}: ");
            sb.Append(string.IsNullOrEmpty(kv.Value) ? "\'\'" : kv.Value);
        }
        return sb.ToString();
    }

    public static string ValueOrDefault(this Dictionary<string, string> dict, string key, string defaultValue) =>
        dict.TryGetValue(key, out var value) ? value : defaultValue;

    private const int DaysPerYear = 365;
    // Number of days in 4 years
    private const int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461
    // Number of days in 100 years
    private const int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
    // Number of days in 400 years
    private const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097
    private const int DaysTo1970 = DaysPer400Years * 4 + DaysPer100Years * 3 + DaysPer4Years * 17 + DaysPerYear;
    private const int HoursPerDay = 24;
    internal const int MicrosecondsPerMillisecond = 1000;
    private const long TicksPerMicrosecond = 10;
    private const long TicksPerMillisecond = TicksPerMicrosecond * MicrosecondsPerMillisecond;
    private const long TicksPerSecond = TicksPerMillisecond * 1000;
    private const long TicksPerMinute = TicksPerSecond * 60;
    private const long TicksPerHour = TicksPerMinute * 60;
    private const long TicksPerDay = TicksPerHour * HoursPerDay;
    private const long UnixEpochTicks = DaysTo1970 * TicksPerDay; // Use DateTimeOffset.UnixEpoch.Ticks instead

    public static DateTime UnixNanoSecondsToDateTime(ulong unixTimeNanoSeconds)
    {
        var milliseconds = (long)unixTimeNanoSeconds / 1_000_000;
        return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).DateTime;
    }

    public static string ToJSArray(this double[] values) =>
        $"[{string.Join(",", values)}]";

    public static string ToJSArray(this string[] values) =>
    $"['{string.Join("','", values)}']";

    public static string ToHexString(this Google.Protobuf.ByteString bytes)
    {
        if (bytes is null or { Length: 0 })
        {
            return null!;
        }
        var sb = new StringBuilder();
        foreach (var b in bytes)
        {
            sb.Append($"{b:x2}");
        }
        return sb.ToString();
    }
}
