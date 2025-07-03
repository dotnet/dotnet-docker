// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dotnet.Docker.Model.Release;

/// <summary>
/// This represents the configuration for a single run of the .NET release and staging pipelines. It contains
/// information about the .NET versions to be released as well as other information about the release.
/// </summary>
/// <remarks>
/// This record is a subset of the model used by the .NET staging and release pipelines.
/// This MUST stay in sync with https://dev.azure.com/dnceng/internal/_git/dotnet-release?path=%2Fsrc%2FMicrosoft.DotNet.Release%2FMicrosoft.DotNet.ReleaseLib%2FModels%2FMetadataConfig.cs
/// </remarks>
internal record ReleaseConfig
{
    public required string Channel { get; init; }

    public required string MajorVersion { get; init; }

    public required string Release { get; init; }

    public required string Runtime { get; init; }

    public required string Asp { get; init; }

    public required List<string> Sdks { get; init; }

    [JsonPropertyName("Runtime_Build")]
    public required string RuntimeBuild { get; init; }

    [JsonPropertyName("Asp_Build")]
    public required string AspBuild { get; init; }

    [JsonPropertyName("Sdk_Builds")]
    public required List<string> SdkBuilds { get; init; }

    [JsonPropertyName("Release_Date")]
    public required string ReleaseDate { get; init; }

    public required bool Security { get; init; }

    [JsonPropertyName("Support_Phase")]
    public required string SupportPhase { get; init; }

    public required bool Internal { get; init; }

    public required bool SdkOnly { get; init; }

    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        WriteIndented = true
    };

    public override string ToString() =>
        JsonSerializer.Serialize(this, s_jsonOptions);

    public static ReleaseConfig FromJson(string json) =>
        JsonSerializer.Deserialize<ReleaseConfig>(json)
            ?? throw new InvalidOperationException("Failed to deserialize release config: " + json);
};
