// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.DotNet.Docker.Tests
{
    public class DefaultImageDataEqualityComparer : IEqualityComparer<ImageData>
    {
        public bool Equals([AllowNull] ImageData x, [AllowNull] ImageData y)
        {
            if (x is null && y is null)
            {
                return true;
            }

            if (x is null && !(y is null))
            {
                return false;
            }

            if (!(x is null) && y is null)
            {
                return false;
            }

            return x.VersionString == y.VersionString &&
                x.OS == y.OS &&
                x.Arch == y.Arch;
        }

        public int GetHashCode([DisallowNull] ImageData obj)
        {
            return $"{obj.VersionString}-{obj.OS}-{obj.Arch}".GetHashCode();
        }
    }
}
