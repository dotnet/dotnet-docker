// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Maestro.Common;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

/// <summary>
/// A temporary Git repository for testing purposes.
/// </summary>
internal sealed class TempRepo : IDisposable
{
    public TempRepo()
    {
        Directory.CreateDirectory(LocalPath);

        // Create a temp repo that can be used for testing commands using a real git process
        var logger = Mock.Of<ILogger>();
        var processManager = new ProcessManager(logger, "git");
        var git = new LocalGitClient(
            Mock.Of<IRemoteTokenProvider>(),
            Mock.Of<ITelemetryRecorder>(),
            processManager,
            new FileSystem(),
            logger);
        var localGitRepoFactory = new LocalGitRepoFactory(git, processManager);

        LocalGitRepo = localGitRepoFactory.Create(new NativePath(LocalPath));
    }

    public string LocalPath { get; } = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    public ILocalGitRepo LocalGitRepo { get; }

    public void Dispose()
    {
        try
        {
            Directory.Delete(LocalPath, true);
        }
        catch (DirectoryNotFoundException)
        {
            // Good, it's already gone
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
