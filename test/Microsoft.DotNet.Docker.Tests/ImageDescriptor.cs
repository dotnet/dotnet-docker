// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.DotNet.Docker.Tests
{
    public class ImageDescriptor
    {
        private string runtimeDepsVersion;
        private string sdkVersion;
        private string sdkOsVariant;

        public string Architecture { get; set; } = "amd64";
        public string DotNetCoreVersion { get; set; }
        public bool IsArm { get => String.Equals("arm", Architecture, StringComparison.OrdinalIgnoreCase); }
        public string OsVariant { get; set; }
        public string PlatformOS { get; set; }

        public string RuntimeDepsVersion
        {
            get { return runtimeDepsVersion ?? DotNetCoreVersion; }
            set { runtimeDepsVersion = value; }
        }

        public string SdkVersion
        {
            get { return sdkVersion ?? DotNetCoreVersion; }
            set { sdkVersion = value; }
        }

        public string SdkOsVariant
        {
            get { return sdkOsVariant ?? OsVariant; }
            set { sdkOsVariant = value; }
        }
    }
}
