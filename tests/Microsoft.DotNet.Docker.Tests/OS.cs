// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.DotNet.Docker.Tests
{
    public class OS
    {
        // Alpine
        public const string Alpine = "alpine";
        public const string Alpine319 = $"{Alpine}3.19";
        public const string Alpine320 = $"{Alpine}3.20";

        // AzureLinux
        public const string AzureLinux = "azurelinux";

        public const string AzureLinuxDistroless = $"{AzureLinux}-distroless";
        public const string AzureLinux30 = $"{AzureLinux}3.0";
        public const string AzureLinux30Distroless = $"{AzureLinux30}-distroless";

        // Debian
        public const string Bookworm = "bookworm";
        public const string BookwormSlim = $"{Bookworm}{SlimSuffix}";
        public const string Bullseye = "bullseye";
        public const string BullseyeSlim = $"{Bullseye}{SlimSuffix}";

        // Mariner
        public const string Mariner = "cbl-mariner";

        public const string MarinerDistroless = $"{Mariner}-distroless";
        public const string Mariner20 = $"{Mariner}2.0";
        public const string Mariner20Distroless = $"{Mariner20}-distroless";

        // Ubuntu
        public const string Bionic = "bionic";
        public const string Focal = "focal";
        public const string Jammy = "jammy";
        public const string JammyChiseled = $"{Jammy}{ChiseledSuffix}";
        public const string Noble = "noble";
        public const string NobleChiseled = $"{Noble}{ChiseledSuffix}";
        public const string UbuntuChiseled = $"ubuntu{ChiseledSuffix}";

        // Windows
        public const string NanoServer = "nanoserver";
        public const string NanoServer1809 = $"{NanoServer}-1809";
        public const string NanoServerLtsc2022 = $"{NanoServer}-ltsc2022";
        public const string ServerCore = "windowsservercore";
        public const string ServerCoreLtsc2019 = $"{ServerCore}-ltsc2019";
        public const string ServerCoreLtsc2022 = $"{ServerCore}-ltsc2022";

        // Helpers
        public const string ChiseledSuffix = "-chiseled";
        public const string SlimSuffix = "-slim";
    }
}
