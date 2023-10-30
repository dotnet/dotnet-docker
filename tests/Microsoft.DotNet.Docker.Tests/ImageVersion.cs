// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.DotNet.Docker.Tests
{
    public readonly struct ImageVersion
    {
        private readonly Version _version;

        public static readonly ImageVersion V6_0 = new(new Version(6, 0), isPreview: false);
        public static readonly ImageVersion V6_3 = new(new Version(6, 3), isPreview: false);
        public static readonly ImageVersion V7_0 = new(new Version(7, 0), isPreview: false);
        public static readonly ImageVersion V7_3 = new(new Version(7, 3), isPreview: false);
        public static readonly ImageVersion V8_0 = new(new Version(8, 0), isPreview: false);

        public ImageVersion(Version version, bool isPreview)
        {
            _version = version;
            IsPreview = isPreview;
        }

        public int Major => _version.Major;

        public bool IsPreview { get; }

        public override string ToString() => _version.ToString();

        public string GetTagName() => ToString() + (IsPreview ? "-preview" : string.Empty);
    }
}
