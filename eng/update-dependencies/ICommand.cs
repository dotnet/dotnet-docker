// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Dotnet.Docker;

/// <summary>
/// A generic CLI command that takes an options object.
/// </summary>
/// <typeparam name="TOptions">
/// The type of options that the command accepts.
/// </typeparam>
/// <seealso cref="BaseCommand"/>
public interface ICommand<TOptions> where TOptions : IOptions
{
    Task<int> ExecuteAsync(TOptions options);
}
