// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    string TagName,
    string DisplayName,
    bool IsDistroless)
{
    /// <summary>
    /// Gets a value indicating whether this OS is Windows-based.
    /// </summary>
    public bool IsWindows => Type == OSType.Windows;

    /// <summary>
    /// Gets a value indicating whether this OS uses Ubuntu Chiseled images.
    /// </summary>
    public bool IsChiseled => TagName.Contains(OS.ChiseledSuffix);

    /// <summary>
    /// Gets a value indicating whether this OS uses slim images.
    /// </summary>
    public bool IsSlim => TagName.Contains(OS.SlimSuffix);

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
        OSType.Linux, OSFamily.Alpine, "", OS.Alpine, "Alpine", IsDistroless: false);

    public static OSInfo Alpine321 { get; } = AlpineFloating with
        { Version = "3.21", TagName = OS.Alpine321, DisplayName = "Alpine 3.21" };

    public static OSInfo Alpine322 { get; } = AlpineFloating with
        { Version = "3.22", TagName = OS.Alpine322, DisplayName = "Alpine 3.22" };

    public static OSInfo Alpine323 { get; } = AlpineFloating with
        { Version = "3.23", TagName = OS.Alpine323, DisplayName = "Alpine 3.23" };

    // Azure Linux
    public static OSInfo AzureLinux30 { get; } = new(
        OSType.Linux, OSFamily.AzureLinux, "3.0", OS.AzureLinux30, "Azure Linux 3.0", IsDistroless: false);

    public static OSInfo AzureLinux30Distroless { get; } = AzureLinux30 with
        { TagName = OS.AzureLinux30Distroless, DisplayName = "Azure Linux 3.0 (Distroless)", IsDistroless = true };

    // Debian
    public static OSInfo Bookworm { get; } = new(
        OSType.Linux, OSFamily.Debian, "12", OS.Bookworm, "Debian 12 (Bookworm)", IsDistroless: false);

    public static OSInfo BookwormSlim { get; } = Bookworm with
        { TagName = OS.BookwormSlim, DisplayName = "Debian 12 (Bookworm Slim)" };

    // Mariner (CBL-Mariner)
    public static OSInfo Mariner20 { get; } = new(
        OSType.Linux, OSFamily.Mariner, "2.0", OS.Mariner20, "CBL-Mariner 2.0", IsDistroless: false);

    public static OSInfo Mariner20Distroless { get; } = Mariner20 with
        { TagName = OS.Mariner20Distroless, DisplayName = "CBL-Mariner 2.0 (Distroless)", IsDistroless = true };

    // Ubuntu
    public static OSInfo Jammy { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "22.04", OS.Jammy, "Ubuntu 22.04 (Jammy)", IsDistroless: false);

    public static OSInfo JammyChiseled { get; } = Jammy with
        { TagName = OS.JammyChiseled, DisplayName = "Ubuntu 22.04 (Jammy Chiseled)", IsDistroless = true };

    public static OSInfo Noble { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "24.04", OS.Noble, "Ubuntu 24.04 (Noble)", IsDistroless: false);

    public static OSInfo NobleChiseled { get; } = Noble with
        { TagName = OS.NobleChiseled, DisplayName = "Ubuntu 24.04 (Noble Chiseled)", IsDistroless = true };

    public static OSInfo Resolute { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "26.04", OS.Resolute, "Ubuntu 26.04 (Resolute)", IsDistroless: false);

    public static OSInfo ResoluteChiseled { get; } = Resolute with
        { TagName = OS.ResoluteChiseled, DisplayName = "Ubuntu 26.04 (Resolute Chiseled)", IsDistroless = true };

    // Windows - Nano Server
    public static OSInfo NanoServer1809 { get; } = new(
        OSType.Windows, OSFamily.NanoServer, "1809", OS.NanoServer1809, "Nano Server 1809", IsDistroless: false);

    public static OSInfo NanoServerLtsc2022 { get; } = NanoServer1809 with
        { Version = "LTSC 2022", TagName = OS.NanoServerLtsc2022, DisplayName = "Nano Server LTSC 2022" };

    public static OSInfo NanoServerLtsc2025 { get; } = NanoServer1809 with
        { Version = "LTSC 2025", TagName = OS.NanoServerLtsc2025, DisplayName = "Nano Server LTSC 2025" };

    // Windows - Server Core
    public static OSInfo ServerCoreLtsc2019 { get; } = new(
        OSType.Windows, OSFamily.WindowsServerCore, "LTSC 2019", OS.ServerCoreLtsc2019, "Windows Server Core LTSC 2019", IsDistroless: false);

    public static OSInfo ServerCoreLtsc2022 { get; } = ServerCoreLtsc2019 with
        { Version = "LTSC 2022", TagName = OS.ServerCoreLtsc2022, DisplayName = "Windows Server Core LTSC 2022" };

    public static OSInfo ServerCoreLtsc2025 { get; } = ServerCoreLtsc2019 with
        { Version = "LTSC 2025", TagName = OS.ServerCoreLtsc2025, DisplayName = "Windows Server Core LTSC 2025" };
}
