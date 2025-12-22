// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

namespace Microsoft.DotNet.Docker.Tests;

internal static class OS
{
    // String constants for special tagging cases (appliance images)
    public const string Alpine = "alpine";
    public const string AzureLinuxDistroless = "azurelinux-distroless";
    public const string MarinerDistroless = "cbl-mariner-distroless";
    public const string UbuntuChiseled = "ubuntu-chiseled";

    // Alpine
    public static OSInfo AlpineFloating { get; } = new(OSType.Linux, OSFamily.Alpine, "");
    public static OSInfo Alpine322 { get; } = AlpineFloating with { Version = "3.22" };
    public static OSInfo Alpine323 { get; } = AlpineFloating with { Version = "3.23" };

    // Azure Linux
    public static OSInfo AzureLinux30 { get; } = new(OSType.Linux, OSFamily.AzureLinux, "3.0");
    public static OSInfo AzureLinux30Distroless { get; } = AzureLinux30 with { IsDistroless = true };

    // Debian
    public static OSInfo BookwormSlim { get; } = new(OSType.Linux, OSFamily.Debian, "12");

    // Mariner (CBL-Mariner)
    public static OSInfo Mariner20 { get; } = new(OSType.Linux, OSFamily.Mariner, "2.0");
    public static OSInfo Mariner20Distroless { get; } = Mariner20 with { IsDistroless = true };

    // Ubuntu
    public static OSInfo Jammy { get; } = new(OSType.Linux, OSFamily.Ubuntu, "22.04");
    public static OSInfo JammyChiseled { get; } = Jammy with { IsDistroless = true };
    public static OSInfo Noble { get; } = new(OSType.Linux, OSFamily.Ubuntu, "24.04");
    public static OSInfo NobleChiseled { get; } = Noble with { IsDistroless = true };
    public static OSInfo Resolute { get; } = new(OSType.Linux, OSFamily.Ubuntu, "26.04", IsUnstable: true);
    public static OSInfo ResoluteChiseled { get; } = Resolute with { IsDistroless = true };

    // Windows - Nano Server
    public static OSInfo NanoServer1809 { get; } = new(OSType.Windows, OSFamily.NanoServer, "1809");
    public static OSInfo NanoServerLtsc2022 { get; } = NanoServer1809 with { Version = "LTSC 2022" };
    public static OSInfo NanoServerLtsc2025 { get; } = NanoServer1809 with { Version = "LTSC 2025" };

    // Windows - Server Core
    public static OSInfo ServerCoreLtsc2019 { get; } = new(OSType.Windows, OSFamily.WindowsServerCore, "LTSC 2019");
    public static OSInfo ServerCoreLtsc2022 { get; } = ServerCoreLtsc2019 with { Version = "LTSC 2022" };
    public static OSInfo ServerCoreLtsc2025 { get; } = ServerCoreLtsc2019 with { Version = "LTSC 2025" };
}
