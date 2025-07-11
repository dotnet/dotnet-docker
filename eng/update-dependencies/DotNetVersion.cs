// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

internal partial class DotNetVersion
{
    private readonly string _versionString;

    public DotNetVersion(string versionString)
    {
        _versionString = versionString;
    }

    // Allow a string to be implicitly converted to DotNetVersion
    public static implicit operator DotNetVersion(string versionString) => new(versionString);

    // Allow implicit conversion to string
    public override string ToString() => _versionString;
}
