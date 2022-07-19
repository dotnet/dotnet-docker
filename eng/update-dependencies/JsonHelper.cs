// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Newtonsoft.Json.Linq;

#nullable enable
namespace Dotnet.Docker;

public static class JsonHelper
{
    public static T GetRequiredToken<T>(this JToken token, string name)
        where T : JToken =>
        (T)(token[name] ?? throw new InvalidOperationException($"Missing '{name}' property"));
}
#nullable disable
