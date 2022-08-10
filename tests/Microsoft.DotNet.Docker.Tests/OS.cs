// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.DotNet.Docker.Tests
{
    public class OS
    {
        // Alpine
        public const string Alpine = "alpine";
        public const string Alpine315 = $"{Alpine}3.15";
        public const string Alpine316 = $"{Alpine}3.16";

        // Debian
        public const string Bullseye = "bullseye";
        public const string BullseyeSlim = $"{Bullseye}{SlimSuffix}";
        public const string Buster = "buster";
        public const string BusterSlim = $"{Buster}{SlimSuffix}";

        // Mariner
        public const string Mariner = "cbl-mariner";

        public const string MarinerDistroless = $"{Mariner}-distroless";
        public const string Mariner10 = $"{Mariner}1.0";
        public const string Mariner10Distroless = $"{Mariner10}-distroless";
        public const string Mariner20 = $"{Mariner}2.0";
        public const string Mariner20Distroless = $"{Mariner20}-distroless";

        // Ubuntu
        public const string Bionic = "bionic";
        public const string Focal = "focal";
        public const string Jammy = "jammy";
        public const string JammyChiseled = "jammy-chiseled";

        // Windows
        public const string NanoServer = "nanoserver";
        public const string NanoServer1809 = $"{NanoServer}-1809";
        public const string NanoServerLtsc2022 = $"{NanoServer}-ltsc2022";
        public const string ServerCore = "windowsservercore";
        public const string ServerCoreLtsc2019 = $"{ServerCore}-ltsc2019";
        public const string ServerCoreLtsc2022 = $"{ServerCore}-ltsc2022";

        // Helpers
        public const string SlimSuffix = "-slim";
    }
}
