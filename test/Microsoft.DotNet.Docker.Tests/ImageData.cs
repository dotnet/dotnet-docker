// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.DotNet.Docker.Tests
{
    public class ImageData
    {
        private string runtimeDepsVersion;
        private string sdkVersion;

        public string Architecture { get; set; } = "amd64";
        public string DotNetVersion { get; set; }
        public bool HasNoSdk { get; set; }
        public bool IsAlpine { get => String.Equals(OS.Alpine, OsVariant, StringComparison.OrdinalIgnoreCase); }
        public bool IsArm { get => String.Equals("arm", Architecture, StringComparison.OrdinalIgnoreCase); }
        public string OsVariant { get; set; }
        public string PlatformOS { get; set; }

        public string RuntimeDepsVersion
        {
            get { return runtimeDepsVersion ?? DotNetVersion; }
            set { runtimeDepsVersion = value; }
        }

        public string SdkVersion
        {
            get { return sdkVersion ?? DotNetVersion; }
            set { sdkVersion = value; }
        }

        public string SdkOsVariant
        {
            get { return HasNoSdk ? "" : OsVariant; }
        }
    }
}
