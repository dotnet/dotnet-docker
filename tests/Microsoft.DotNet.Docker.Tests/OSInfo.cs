// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

namespace Microsoft.DotNet.Docker.Tests;

/// <summary>
/// Represents the type of operating system.
/// </summary>
internal enum OSType
{
    Linux,
    Windows
}

/// <summary>
/// Represents the family or distribution of an operating system.
/// </summary>
internal enum OSFamily
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
internal sealed record OSInfo(
    OSType Type,
    OSFamily Family,
    string? Version,
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
    /// Implicit conversion to string for backward compatibility with existing code.
    /// </summary>
    public static implicit operator string(OSInfo os) => os.TagName;

    public override string ToString() => TagName;

    // Alpine
    public static OSInfo Alpine { get; } = new(
        OSType.Linux, OSFamily.Alpine, null, OS.Alpine, "Alpine", IsDistroless: false);

    public static OSInfo Alpine321 { get; } = new(
        OSType.Linux, OSFamily.Alpine, "3.21", OS.Alpine321, "Alpine 3.21", IsDistroless: false);

    public static OSInfo Alpine322 { get; } = new(
        OSType.Linux, OSFamily.Alpine, "3.22", OS.Alpine322, "Alpine 3.22", IsDistroless: false);

    public static OSInfo Alpine323 { get; } = new(
        OSType.Linux, OSFamily.Alpine, "3.23", OS.Alpine323, "Alpine 3.23", IsDistroless: false);

    // Azure Linux
    public static OSInfo AzureLinux { get; } = new(
        OSType.Linux, OSFamily.AzureLinux, null, OS.AzureLinux, "Azure Linux", IsDistroless: false);

    public static OSInfo AzureLinuxDistroless { get; } = new(
        OSType.Linux, OSFamily.AzureLinux, null, OS.AzureLinuxDistroless, "Azure Linux (Distroless)", IsDistroless: true);

    public static OSInfo AzureLinux30 { get; } = new(
        OSType.Linux, OSFamily.AzureLinux, "3.0", OS.AzureLinux30, "Azure Linux 3.0", IsDistroless: false);

    public static OSInfo AzureLinux30Distroless { get; } = new(
        OSType.Linux, OSFamily.AzureLinux, "3.0", OS.AzureLinux30Distroless, "Azure Linux 3.0 (Distroless)", IsDistroless: true);

    // Debian
    public static OSInfo Bookworm { get; } = new(
        OSType.Linux, OSFamily.Debian, "12", OS.Bookworm, "Debian 12 (Bookworm)", IsDistroless: false);

    public static OSInfo BookwormSlim { get; } = new(
        OSType.Linux, OSFamily.Debian, "12", OS.BookwormSlim, "Debian 12 (Bookworm Slim)", IsDistroless: false);

    // Mariner (CBL-Mariner)
    public static OSInfo Mariner { get; } = new(
        OSType.Linux, OSFamily.Mariner, null, OS.Mariner, "CBL-Mariner", IsDistroless: false);

    public static OSInfo MarinerDistroless { get; } = new(
        OSType.Linux, OSFamily.Mariner, null, OS.MarinerDistroless, "CBL-Mariner (Distroless)", IsDistroless: true);

    public static OSInfo Mariner20 { get; } = new(
        OSType.Linux, OSFamily.Mariner, "2.0", OS.Mariner20, "CBL-Mariner 2.0", IsDistroless: false);

    public static OSInfo Mariner20Distroless { get; } = new(
        OSType.Linux, OSFamily.Mariner, "2.0", OS.Mariner20Distroless, "CBL-Mariner 2.0 (Distroless)", IsDistroless: true);

    // Ubuntu
    public static OSInfo Jammy { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "22.04", OS.Jammy, "Ubuntu 22.04 (Jammy)", IsDistroless: false);

    public static OSInfo JammyChiseled { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "22.04", OS.JammyChiseled, "Ubuntu 22.04 (Jammy Chiseled)", IsDistroless: true);

    public static OSInfo Noble { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "24.04", OS.Noble, "Ubuntu 24.04 (Noble)", IsDistroless: false);

    public static OSInfo NobleChiseled { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "24.04", OS.NobleChiseled, "Ubuntu 24.04 (Noble Chiseled)", IsDistroless: true);

    public static OSInfo Resolute { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "26.04", OS.Resolute, "Ubuntu 26.04 (Resolute)", IsDistroless: false);

    public static OSInfo ResoluteChiseled { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, "26.04", OS.ResoluteChiseled, "Ubuntu 26.04 (Resolute Chiseled)", IsDistroless: true);

    public static OSInfo UbuntuChiseled { get; } = new(
        OSType.Linux, OSFamily.Ubuntu, null, OS.UbuntuChiseled, "Ubuntu (Chiseled)", IsDistroless: true);

    // Windows - Nano Server
    public static OSInfo NanoServer { get; } = new(
        OSType.Windows, OSFamily.NanoServer, null, OS.NanoServer, "Nano Server", IsDistroless: false);

    public static OSInfo NanoServer1809 { get; } = new(
        OSType.Windows, OSFamily.NanoServer, "1809", OS.NanoServer1809, "Nano Server 1809", IsDistroless: false);

    public static OSInfo NanoServerLtsc2022 { get; } = new(
        OSType.Windows, OSFamily.NanoServer, "LTSC 2022", OS.NanoServerLtsc2022, "Nano Server LTSC 2022", IsDistroless: false);

    public static OSInfo NanoServerLtsc2025 { get; } = new(
        OSType.Windows, OSFamily.NanoServer, "LTSC 2025", OS.NanoServerLtsc2025, "Nano Server LTSC 2025", IsDistroless: false);

    // Windows - Server Core
    public static OSInfo ServerCore { get; } = new(
        OSType.Windows, OSFamily.WindowsServerCore, null, OS.ServerCore, "Windows Server Core", IsDistroless: false);

    public static OSInfo ServerCoreLtsc2019 { get; } = new(
        OSType.Windows, OSFamily.WindowsServerCore, "LTSC 2019", OS.ServerCoreLtsc2019, "Windows Server Core LTSC 2019", IsDistroless: false);

    public static OSInfo ServerCoreLtsc2022 { get; } = new(
        OSType.Windows, OSFamily.WindowsServerCore, "LTSC 2022", OS.ServerCoreLtsc2022, "Windows Server Core LTSC 2022", IsDistroless: false);

    public static OSInfo ServerCoreLtsc2025 { get; } = new(
        OSType.Windows, OSFamily.WindowsServerCore, "LTSC 2025", OS.ServerCoreLtsc2025, "Windows Server Core LTSC 2025", IsDistroless: false);
}
