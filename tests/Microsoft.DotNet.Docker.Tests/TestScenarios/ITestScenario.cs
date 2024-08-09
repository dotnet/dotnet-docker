// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.DotNet.Docker.Tests;

public abstract class TestScenario
{
    public Task ExecuteAsync(bool shouldThrow = false)
    {
        if (shouldThrow)
        {
            return Assert.ThrowsAnyAsync<Exception>(ExecuteInternalAsync);
        }

        return ExecuteInternalAsync();
    }

    protected abstract Task ExecuteInternalAsync();
}
