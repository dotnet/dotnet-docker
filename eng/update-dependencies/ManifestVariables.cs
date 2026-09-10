// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Dotnet.Docker;

/// <summary>
/// Holds an editable, in-memory manifest whose string variables can refer to one another
/// using <c>$(name)</c>. Names are case-sensitive.
/// </summary>
/// <remarks>
/// Edits affect only existing values in the root <c>variables</c> object. Unrelated JSON,
/// comments, whitespace, and property names are preserved in <see cref="Content"/>.
/// Reading a resolved value never rewrites its references. Changes are not saved to disk;
/// the caller decides when to write Content and which file encoding to use.
/// </remarks>
public sealed partial class ManifestVariables
{
    // Token ranges always refer to the original text. Keeping edits separately avoids shifting
    // later ranges and lets a reverted value recover its original JSON escaping.
    private readonly string _originalContent;
    private readonly Dictionary<string, Variable> _variables = new(StringComparer.Ordinal);

    /// <summary>
    /// Creates an editable snapshot of a complete manifest.
    /// </summary>
    /// <param name="content">
    /// JSON with exactly one root <c>variables</c> object containing unique names and string
    /// values. Comments and trailing commas are accepted. References are validated only
    /// when resolved, so an unresolved or cyclic reference can be repaired by editing.
    /// </param>
    /// <exception cref="JsonException">
    /// The JSON is malformed, the variables object is missing or duplicated, or its entries
    /// have duplicate names or non-string values.
    /// </exception>
    public ManifestVariables(string content)
    {
        _originalContent = content;

        byte[] utf8 = Encoding.UTF8.GetBytes(content);
        var readerOptions = new JsonReaderOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };
        var reader = new Utf8JsonReader(utf8, readerOptions);

        reader.Read();
        RequireToken(reader, JsonTokenType.StartObject);
        bool foundVariables = false;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            RequireToken(reader, JsonTokenType.PropertyName);
            string name = reader.GetString()
                ?? throw new JsonException("Manifest property name cannot be null.");
            reader.Read();

            if (name != "variables")
            {
                reader.Skip();
                continue;
            }

            if (foundVariables)
            {
                throw new JsonException("Manifest contains more than one 'variables' property.");
            }

            foundVariables = true;
            ReadVariables(ref reader, utf8);
        }

        RequireToken(reader, JsonTokenType.EndObject);
        if (reader.Read())
        {
            throw new JsonException("Unexpected content after the manifest object.");
        }

        if (!foundVariables)
        {
            throw new JsonException("Manifest is missing the 'variables' object.");
        }
    }

    /// <summary>
    /// Loads a snapshot of a manifest file. Subsequent edits neither write to nor reread the file.
    /// </summary>
    /// <param name="filePath">
    /// Path to the manifest. Text is read as UTF-8 unless a byte-order mark specifies another encoding.
    /// </param>
    /// <remarks>
    /// File access errors and the constructor's validation errors propagate to the caller.
    /// The original file encoding is not retained.
    /// </remarks>
    public static ManifestVariables FromFile(string filePath) => new(File.ReadAllText(filePath));

    /// <summary>
    /// Gets valid JSON reflecting the current stored values, without expanding references.
    /// </summary>
    /// <remarks>
    /// Only values different from their original decoded strings are serialized. All other
    /// text is preserved exactly, including original escape sequences. Reverting every edit
    /// therefore restores the original content, not merely equivalent JSON.
    /// </remarks>
    public string Content
    {
        get
        {
            var content = new StringBuilder();
            int position = 0;

            foreach (Variable variable in _variables.Values.OrderBy(variable => variable.OriginalTokenStart))
            {
                if (variable.Value == variable.OriginalValue)
                {
                    continue;
                }

                content.Append(_originalContent, position, variable.OriginalTokenStart - position);
                content.Append(JsonSerializer.Serialize(variable.Value));
                position = variable.OriginalTokenStart + variable.OriginalTokenLength;
            }

            content.Append(_originalContent, position, _originalContent.Length - position);
            return content.ToString();
        }
    }

    /// <summary>
    /// Gets the names declared in the root variables object. The set of names does not change
    /// when values are edited; nested or unrelated properties are excluded.
    /// </summary>
    public IEnumerable<string> Names => _variables.Keys;

    /// <summary>
    /// Reports whether a variable is declared, independently of whether its value is empty
    /// or its references can be resolved.
    /// </summary>
    /// <param name="name">The exact, case-sensitive variable name.</param>
    public bool Contains(string name) => _variables.ContainsKey(name);

    /// <summary>
    /// Gets the current stored string with JSON escapes decoded but <c>$(name)</c> references intact.
    /// </summary>
    /// <param name="name">The exact, case-sensitive variable name.</param>
    /// <exception cref="KeyNotFoundException">The named variable is not declared.</exception>
    public string GetRawValue(string name) => GetVariable(name).Value;

    /// <summary>
    /// Gets the current value with embedded <c>$(name)</c> references recursively expanded.
    /// Resolution observes all edits made so far and does not change the stored values or Content.
    /// </summary>
    /// <param name="name">The exact, case-sensitive variable name.</param>
    /// <exception cref="KeyNotFoundException">
    /// The requested variable or one of its references is missing. The message includes the reference chain.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// References form a cycle. The message includes the cycle's reference chain.
    /// </exception>
    public string GetValue(string name) => ResolveValue(name, []);

    /// <summary>
    /// Replaces exactly the named variable's stored string, even if it currently contains a reference.
    /// Referenced variables are never modified.
    /// </summary>
    /// <param name="name">An existing, case-sensitive variable name. New variables cannot be inserted.</param>
    /// <param name="value">
    /// The decoded replacement string, which may be empty or contain references.
    /// JSON escaping is handled internally; references are not validated until GetValue is called.
    /// </param>
    /// <returns>
    /// True if this call changed the stored string; false if it already equaled value.
    /// This is not a report of whether the whole manifest differs from its original content.
    /// </returns>
    /// <exception cref="KeyNotFoundException">The named variable is not declared.</exception>
    public bool SetValue(string name, string value)
    {
        Variable variable = GetVariable(name);
        if (variable.Value == value)
        {
            return false;
        }

        _variables[name] = variable with { Value = value };
        return true;
    }

    private Variable GetVariable(string name)
    {
        if (!_variables.TryGetValue(name, out Variable? variable))
        {
            throw new KeyNotFoundException($"Manifest variable '{name}' was not found.");
        }

        return variable;
    }

    private string ResolveValue(string name, List<string> path)
    {
        if (path.Contains(name))
        {
            string cycle = string.Join(" -> ", path.Append(name));
            throw new InvalidOperationException($"Manifest variable reference cycle: {cycle}");
        }

        path.Add(name);
        if (!Contains(name))
        {
            string chain = string.Join(" -> ", path);
            throw new KeyNotFoundException($"Manifest variable '{name}' was not found while resolving: {chain}");
        }

        string value = GetRawValue(name);
        string resolved = ReferenceRegex.Replace(
            value,
            match => ResolveValue(match.Groups["name"].Value, path));

        path.RemoveAt(path.Count - 1);
        return resolved;
    }

    /// <summary>
    /// Reads the variables object, entering on its StartObject token and returning on its
    /// matching EndObject token. The caller advances to the next root property.
    /// </summary>
    private void ReadVariables(ref Utf8JsonReader reader, byte[] utf8)
    {
        RequireToken(reader, JsonTokenType.StartObject);

        int bytePosition = 0;
        int characterPosition = 0;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            RequireToken(reader, JsonTokenType.PropertyName);
            string name = reader.GetString()
                ?? throw new JsonException("Manifest variable name cannot be null.");
            reader.Read();
            RequireToken(reader, JsonTokenType.String);

            string value = reader.GetString()
                ?? throw new JsonException($"Manifest variable '{name}' must have a string value.");
            int byteStart = checked((int)reader.TokenStartIndex);
            int byteLength = checked((int)reader.BytesConsumed - byteStart);

            // Advance from the previous token's end so each UTF-8 segment is counted once.
            // Token boundaries cannot split a character, making each segment safe to decode.
            int gapLength = byteStart - bytePosition;
            int gapCharacterCount = Encoding.UTF8.GetCharCount(utf8.AsSpan(bytePosition, gapLength));
            int originalTokenStart = characterPosition + gapCharacterCount;
            int originalTokenLength = Encoding.UTF8.GetCharCount(utf8.AsSpan(byteStart, byteLength));
            var variable = new Variable(value, value, originalTokenStart, originalTokenLength);

            bytePosition = byteStart + byteLength;
            characterPosition = originalTokenStart + originalTokenLength;

            if (!_variables.TryAdd(name, variable))
            {
                throw new JsonException($"Manifest variable '{name}' is defined more than once.");
            }
        }

        RequireToken(reader, JsonTokenType.EndObject);
    }

    private static void RequireToken(Utf8JsonReader reader, JsonTokenType expected)
    {
        if (reader.TokenType != expected)
        {
            throw new JsonException($"Expected {expected} in manifest, found {reader.TokenType} at byte {reader.TokenStartIndex}.");
        }
    }

    [GeneratedRegex(@"\$\((?<name>[^)]+)\)")]
    private static partial Regex ReferenceRegex { get; }

    // Coordinates include the surrounding quotes and use UTF-16 offsets in _originalContent.
    private sealed record Variable(
        string OriginalValue,
        string Value,
        int OriginalTokenStart,
        int OriginalTokenLength);
}
