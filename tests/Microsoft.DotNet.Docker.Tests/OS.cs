// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.DotNet.Docker.Tests
{
    public static class OS
    {
        // Alpine
        public const string Alpine = "alpine";
        public const string Alpine321 = $"{Alpine}3.21";
        public const string Alpine322 = $"{Alpine}3.22";
        public const string Alpine323 = $"{Alpine}3.23";

        // AzureLinux
        public const string AzureLinux = "azurelinux";
        public const string AzureLinuxDistroless = $"{AzureLinux}-distroless";
        public const string AzureLinux30 = $"{AzureLinux}3.0";
        public const string AzureLinux30Distroless = $"{AzureLinux30}-distroless";

        // Debian
        public const string Bookworm = "bookworm";
        public const string BookwormSlim = $"{Bookworm}{SlimSuffix}";

        // Mariner
        public const string Mariner = "cbl-mariner";
        public const string MarinerDistroless = $"{Mariner}-distroless";
        public const string Mariner20 = $"{Mariner}2.0";
        public const string Mariner20Distroless = $"{Mariner20}-distroless";

        // Ubuntu
        public const string Jammy = "jammy";
        public const string JammyChiseled = $"{Jammy}{ChiseledSuffix}";
        public const string Noble = "noble";
        public const string NobleChiseled = $"{Noble}{ChiseledSuffix}";
        public const string UbuntuChiseled = $"ubuntu{ChiseledSuffix}";

        // Windows
        public const string NanoServer = "nanoserver";
        public const string NanoServer1809 = $"{NanoServer}-1809";
        public const string NanoServerLtsc2022 = $"{NanoServer}-ltsc2022";
        public const string NanoServerLtsc2025 = $"{NanoServer}-ltsc2025";
        public const string ServerCore = "windowsservercore";
        public const string ServerCoreLtsc2019 = $"{ServerCore}-ltsc2019";
        public const string ServerCoreLtsc2022 = $"{ServerCore}-ltsc2022";
        public const string ServerCoreLtsc2025 = $"{ServerCore}-ltsc2025";

        // Helpers
        public const string DistrolessSuffix = "-distroless";
        public const string ChiseledSuffix = "-chiseled";
        public const string SlimSuffix = "-slim";
    }
}
