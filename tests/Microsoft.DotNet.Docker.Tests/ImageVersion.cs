// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.DotNet.Docker.Tests
{
    public readonly record struct ImageVersion
    {
        private readonly Version _version;

        public static readonly ImageVersion V8_0 = new(new Version(8, 0), isPreview: false);
        public static readonly ImageVersion V8_1 = new(new Version(8, 1), isPreview: false);
        public static readonly ImageVersion V9_0 = new(new Version(9, 0), isPreview: false);
        public static readonly ImageVersion V9_1 = new(new Version(9, 1), isPreview: false);
        public static readonly ImageVersion V13_1 = new(new Version(13, 1), isPreview: false);
        public static readonly ImageVersion V9_2_Preview = new(new Version(9, 2), isPreview: true);
        public static readonly ImageVersion V10_0 = new(new Version(10, 0), isPreview: false);

        public ImageVersion(Version version, bool isPreview)
        {
            _version = version;
            IsPreview = isPreview;
        }

        public int Major => _version.Major;

        public bool IsPreview { get; }

        public override string ToString() => _version.ToString();

        public string GetTagName() => ToString() + (IsPreview ? "-preview" : string.Empty);

        public static string TrimBuildVersionForRelease(string buildVersion)
        {
            int servicingIndex = buildVersion.IndexOf("-servicing.");
            if (servicingIndex != -1)
            {
                buildVersion = buildVersion.Substring(0, servicingIndex);
            }

            int rtmIndex = buildVersion.IndexOf("-rtm.");
            if (rtmIndex != -1)
            {
                buildVersion = buildVersion.Substring(0, rtmIndex);
            }

            return buildVersion;
        }
    }
}
