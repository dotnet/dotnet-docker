// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.DotNet.Docker.Tests
{
    public class ImageData
    {
        private string runtimeDepsVersion;
        private string sdkOsVariant;
        private string sdkVersion;

        public string Architecture { get; set; } = "amd64";
        public string DotNetVersion { get; set; }
        public bool HasNoSdk { get; set; }
        public bool IsWeb { get; set; }
        public bool IsAlpine { get => OsVariant.StartsWith("alpine"); }
        public bool IsArm { get => String.Equals("arm", Architecture, StringComparison.OrdinalIgnoreCase); }
        public string OsVariant { get; set; }

        public string RuntimeDepsVersion
        {
            get { return runtimeDepsVersion ?? DotNetVersion; }
            set { runtimeDepsVersion = value; }
        }

        public string SdkOsVariant
        {
            get { return sdkOsVariant ?? (HasNoSdk ? "" : OsVariant); }
            set { sdkOsVariant = value; }
        }
        
        public string SdkVersion
        {
            get { return sdkVersion ?? DotNetVersion; }
            set { sdkVersion = value; }
        }

        public override string ToString()
        {
            return typeof(ImageData).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(propInfo => $"{propInfo.Name}='{propInfo.GetValue(this) ?? "<null>"}'")
                .Aggregate((working, next) => $"{working}, {next}");
        }
    }
}
