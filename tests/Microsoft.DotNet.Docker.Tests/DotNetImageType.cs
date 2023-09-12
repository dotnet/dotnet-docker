// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.DotNet.Docker.Tests
{
    [Flags]
    public enum DotNetImageRepo
    {
        SDK             = 1 << 0,
        Runtime         = 1 << 1,
        Runtime_Deps    = 1 << 2,
        Aspnet          = 1 << 3,
        Monitor         = 1 << 4
    }
}
