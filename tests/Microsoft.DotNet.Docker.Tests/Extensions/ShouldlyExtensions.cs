// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;

namespace Microsoft.DotNet.Docker.Tests.Extensions;

internal static class ShouldlyExtensions
{
    public static void ShouldAllSatisfy<T>(this IEnumerable<T> collection, Action<T> condition)
    {
        collection.ShouldSatisfyAllConditions(
            collection.Select(
                item => new Action(() => condition(item))).ToArray());
    }
}
