// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.DotNet.Docker.Tests
{
    /// <summary>
    /// The possible variants of a .NET container image.
    /// </summary>
    /// <remarks>
    /// The order of values in this enum enforces the order that the variants must appear in tags if there are multiple.
    /// </remarks>
    [Flags]
    public enum DotNetImageVariant
    {
        None        = 0,
        AOT         = 1 << 0,
        Composite   = 1 << 1,
        Extra       = 1 << 2,
    }
}
