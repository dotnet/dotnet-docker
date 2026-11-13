// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

namespace Microsoft.DotNet.Docker.Tests;

internal static class OS
{
    // String constants for special tagging cases (appliance images)
    public const string Alpine = "alpine";
    public const string AzureLinuxDistroless = "azurelinux-distroless";

    // Alpine
    public static OSInfo AlpineFloating { get; } = new(OSType.Linux, OSFamily.Alpine, "");
    public static OSInfo Alpine324 { get; } = AlpineFloating with { Version = "3.24" };

    // Azure Linux
    public static OSInfo AzureLinux30 { get; } = new(OSType.Linux, OSFamily.AzureLinux, "3.0");
    public static OSInfo AzureLinux30Distroless { get; } = AzureLinux30 with { IsDistroless = true };
    public static OSInfo AzureLinux40 { get; } = new(OSType.Linux, OSFamily.AzureLinux, "4.0", IsUnstable: true);
    public static OSInfo AzureLinux40Distroless { get; } = AzureLinux40 with { IsDistroless = true };

    // Ubuntu
    public static OSInfo Noble { get; } = new(OSType.Linux, OSFamily.Ubuntu, "24.04");
    public static OSInfo NobleChiseled { get; } = Noble with { IsDistroless = true };
    public static OSInfo Resolute { get; } = new(OSType.Linux, OSFamily.Ubuntu, "26.04");
    public static OSInfo ResoluteChiseled { get; } = Resolute with { IsDistroless = true };

    // Windows - Nano Server
    public static OSInfo NanoServerLtsc2022 { get; } = new(OSType.Windows, OSFamily.NanoServer, "LTSC 2022");
    public static OSInfo NanoServerLtsc2025 { get; } = NanoServerLtsc2022 with { Version = "LTSC 2025" };

    // Windows - Server Core
    public static OSInfo ServerCoreLtsc2022 { get; } = new(OSType.Windows, OSFamily.WindowsServerCore, "LTSC 2022");
    public static OSInfo ServerCoreLtsc2025 { get; } = ServerCoreLtsc2022 with { Version = "LTSC 2025" };
}
