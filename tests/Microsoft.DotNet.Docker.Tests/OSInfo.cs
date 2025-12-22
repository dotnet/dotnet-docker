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
/// Operating system used in .NET container images.
/// </summary>
/// <param name="Type">The type of operating system (Linux or Windows).</param>
/// <param name="Family">The family or distribution of the operating system.</param>
/// <param name="Version">
/// The version of the operating system. Do not use codenames here. Use the
/// version exactly as it appears in the OS's own documentation. Examples:
/// "3.23" for Alpine, "26.04" for Ubuntu, "LTSC 2025" for Windows Server Core.
/// </param>
/// <param name="IsDistroless">Whether the OS is distroless.</param>
public sealed record OSInfo(
    OSType Type,
    OSFamily Family,
    string Version,
    bool IsDistroless = false)
{
    /// <summary>
    /// Gets the tag name for this OS, used in Docker image tags.
    /// </summary>
    public string TagName
    {
        get
        {
            static string GetCodename(OSFamily family, string version) => (family, version) switch
            {
                (OSFamily.Debian, "12") => "bookworm",
                (OSFamily.Ubuntu, "22.04") => "jammy",
                (OSFamily.Ubuntu, "24.04") => "noble",
                (OSFamily.Ubuntu, "26.04") => "resolute",
                _ => throw new InvalidOperationException($"Unknown {family} version: {version}")
            };

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
}
