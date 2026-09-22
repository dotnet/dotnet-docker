// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;

namespace Microsoft.DotNet.Docker.UpdateDependencies.Commands;

internal sealed class PositiveIdArgument : Argument<int>
{
    public PositiveIdArgument(string description) : base("id")
    {
        Description = description;
        Validators.Add(result =>
        {
            if (result.GetValueOrDefault<int>() <= 0)
            {
                result.AddError("The ID must be positive.");
            }
        });
    }
}
