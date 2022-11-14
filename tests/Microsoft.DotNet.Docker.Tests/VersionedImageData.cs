// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.DotNet.Docker.Tests
{
    public abstract class VersionedImageData : ImageData
    {
        public virtual ImageVersion RuntimeVersion
        {
            get => throw new NotImplementedException();
            set => throw new NotSupportedException();
        }

        public string RuntimeVersionString => RuntimeVersion.ToString();
        public ImageVersion Version { get; set; }
        public string VersionString => Version.ToString();
    }
}
