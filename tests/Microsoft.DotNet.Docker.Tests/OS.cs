// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.DotNet.Docker.Tests
{
    public class OS
    {
        // Alpine
        public const string Alpine = "alpine";
        public const string Alpine313 = "alpine3.13";

        // Debian
        public const string BullseyeSlim = "bullseye-slim";
        public const string Buster = "buster";
        public const string BusterSlim = "buster-slim";
        public const string Stretch = "stretch";
        public const string StretchSlim = "stretch-slim";

        // Mariner
        public const string Mariner10 = "cbl-mariner1.0";

        // Ubuntu
        public const string Bionic = "bionic";
        public const string Focal = "focal";

        // Windows
        public const string NanoServer1809 = "nanoserver-1809";
        public const string NanoServer2004 = "nanoserver-2004";
        public const string NanoServer20H2 = "nanoserver-20H2";
        public const string NanoServerLtsc2022 = "nanoserver-ltsc2022";
        public const string ServerCoreLtsc2019 = "windowsservercore-ltsc2019";
        public const string ServerCoreLtsc2022 = "windowsservercore-ltsc2022";

        // Helpers
        public const string AlpinePrefix = "alpine";
        public const string SlimSuffix = "-slim";
    }
}
