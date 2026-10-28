// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Maestro.Common.Telemetry;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.DotNet.Internal.Credentials;
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
        ILogger logger = Mock.Of<ILogger>();
        ProcessManager processManager = new ProcessManager(logger, "git");
        LocalGitClient git = new LocalGitClient(
            Mock.Of<IRemoteTokenProvider>(),
            Mock.Of<ITelemetryRecorder>(),
            processManager,
            new FileSystem(),
            logger);
        LocalGitRepoFactory localGitRepoFactory = new LocalGitRepoFactory(git, processManager);

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
