// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

#nullable enable

namespace Microsoft.DotNet.Docker.Tests;

/// <summary>
/// Represents the type of operating system.
/// </summary>
public enum OSType
{
    Linux,
    Windows
}

/// <summary>
/// Represents the family or distribution of an operating system.
/// </summary>
public enum OSFamily
{
    Alpine,
    AzureLinux,
    Debian,
    Mariner,
    Ubuntu,
    NanoServer,
    WindowsServerCore
}

/// <summary>
/// Rich metadata about an operating system used in .NET Docker images.
/// </summary>
public sealed record OSInfo(
    OSType Type,
    OSFamily Family,
    string Version,
    bool IsDistroless)
{
    /// <summary>
    /// Gets the tag name for this OS, used in Docker image tags.
    /// </summary>
    public string TagName
    {
        get
        {
            string baseTag = Family switch
            {
                OSFamily.Alpine => string.IsNullOrEmpty(Version) ? "alpine" : $"alpine{Version}",
                OSFamily.AzureLinux => $"azurelinux{Version}",
                OSFamily.Debian => $"{GetCodename(Family, Version)}-slim",
                OSFamily.Mariner => $"cbl-mariner{Version}",
                OSFamily.Ubuntu => GetCodename(Family, Version),
                OSFamily.NanoServer => $"nanoserver-{Version.ToLowerInvariant().Replace(" ", "")}",
                OSFamily.WindowsServerCore => $"windowsservercore-{Version.ToLowerInvariant().Replace(" ", "")}",
                _ => throw new InvalidOperationException($"Unknown OS family: {Family}")
            };

            return (Family, IsDistroless) switch
            {
                (OSFamily.Ubuntu, true) => $"{baseTag}-chiseled",
                (_, true) => $"{baseTag}-distroless",
                _ => baseTag
            };
        }
    }

    private static string GetCodename(OSFamily family, string version) => (family, version) switch
    {
        (OSFamily.Debian, "12") => "bookworm",
        (OSFamily.Ubuntu, "22.04") => "jammy",
        (OSFamily.Ubuntu, "24.04") => "noble",
        (OSFamily.Ubuntu, "26.04") => "resolute",
        _ => throw new InvalidOperationException($"Unknown {family} version: {version}")
    };

    /// <summary>
    /// Gets the display name for this OS, combining family and version.
    /// </summary>
    public string DisplayName
    {
        get
        {
            string familyName = Family switch
            {
                OSFamily.Alpine => "Alpine",
                OSFamily.AzureLinux => "Azure Linux",
                OSFamily.Debian => "Debian",
                OSFamily.Mariner => "CBL-Mariner",
                OSFamily.Ubuntu => "Ubuntu",
                OSFamily.NanoServer => "Nano Server",
                OSFamily.WindowsServerCore => "Windows Server Core",
                _ => Family.ToString()
            };

            return string.IsNullOrEmpty(Version) ? familyName : $"{familyName} {Version}";
        }
    }

    /// <summary>
    /// Gets a value indicating whether this OS is Windows-based.
    /// </summary>
    public bool IsWindows => Type == OSType.Windows;

    /// <summary>
    /// Checks if the TagName contains the specified value.
    /// Provides backward compatibility with string-based OS checks.
    /// </summary>
    public bool Contains(string value) => TagName.Contains(value);

    /// <summary>
    /// Checks if the TagName starts with the specified value.
    /// Provides backward compatibility with string-based OS checks.
    /// </summary>
    public bool StartsWith(string value) => TagName.StartsWith(value);

    /// <summary>
    /// Implicit conversion to string for backward compatibility with existing code.
    /// </summary>
    public static implicit operator string(OSInfo os) => os.TagName;

    public override string ToString() => TagName;

    // Alpine
    public static OSInfo AlpineFloating { get; } = new(
        OSType.Linux, OSFamily.Alpine, "", IsDistroless: false);

    public static OSInfo Alpine321 { get; } = AlpineFloating with { Version = "3.21" };

    public static OSInfo Alpine322 { get; } = AlpineFloating with { Version = "3.22" };

    public static OSInfo Alpine323 { get; } = AlpineFloating with { Version = "3.23" };

    // Azure Linux
    public static OSInfo AzureLinux30 { get; } = new(
        OSType.Linux, OSFamily.AzureLinux, "3.0", IsDistroless: false);

    public static OSInfo AzureLinux30Distroless { get; } = AzureLinux30 with { IsDistroless = true };

    // Debian
    public static OSInfo BookwormSlim { get; } = new(
        OSType.Linux, OSFamily.Debian, "12", IsDistroless: false);

    // Mariner (CBL-Mariner)
    public static OSInfo Mariner20 { get; } = new(
        OSType.Linux, OSFamily.Mariner, "2.0", IsDistroless: false);

    public static OSInfo Mariner20Distroless { get; } = Mariner20 with { IsDistroless = true };

    // Ubuntu
    public static OSInfo Jammy { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "22.04", IsDistroless: false);

    public static OSInfo JammyChiseled { get; } = Jammy with { IsDistroless = true };

    public static OSInfo Noble { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "24.04", IsDistroless: false);

    public static OSInfo NobleChiseled { get; } = Noble with { IsDistroless = true };

    public static OSInfo Resolute { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "26.04", IsDistroless: false);

    public static OSInfo ResoluteChiseled { get; } = Resolute with { IsDistroless = true };

    // Windows - Nano Server
    public static OSInfo NanoServer1809 { get; } = new(
        OSType.Windows, OSFamily.NanoServer, "1809", IsDistroless: false);

    public static OSInfo NanoServerLtsc2022 { get; } = NanoServer1809 with { Version = "LTSC 2022" };

    public static OSInfo NanoServerLtsc2025 { get; } = NanoServer1809 with { Version = "LTSC 2025" };

    // Windows - Server Core
    public static OSInfo ServerCoreLtsc2019 { get; } = new(
        OSType.Windows, OSFamily.WindowsServerCore, "LTSC 2019", IsDistroless: false);

    public static OSInfo ServerCoreLtsc2022 { get; } = ServerCoreLtsc2019 with { Version = "LTSC 2022" };

    public static OSInfo ServerCoreLtsc2025 { get; } = ServerCoreLtsc2019 with { Version = "LTSC 2025" };
}
