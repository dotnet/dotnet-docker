using OtlpTestListener.DataModel;

namespace OtlpTestListener.Extensions;

public static class Helpers
{
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
