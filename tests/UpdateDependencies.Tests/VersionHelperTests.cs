// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Docker.UpdateDependencies;

namespace UpdateDependencies.Tests;

public sealed class VersionHelperTests
{
    [Theory]
    [InlineData("9.0.100", "10.0.100", "10.0.100")]
    [InlineData("10.0.100", "9.0.100", "10.0.100")]
    [InlineData("11.0.100", "11.0.99", "11.0.100")]
    [InlineData("11.0.99-servicing.12345.1", "11.0.100-servicing.12345.1", "11.0.100-servicing.12345.1")]
    public void GetHighestSdkVersion_UsesNumericOrder(string first, string second, string expected)
    {
        VersionHelper.GetHighestSdkVersion([first, second]).ShouldBe(expected);
    }
}
