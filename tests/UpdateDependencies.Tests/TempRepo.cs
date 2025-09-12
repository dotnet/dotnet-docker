// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Maestro.Common;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.Extensions.Logging;

namespace UpdateDependencies.Tests;

internal sealed class TempRepo : IDisposable
{
    private readonly string _path;

    public TempRepo(string path)
    {
        _path = path;
        Directory.CreateDirectory(path);

        // Create a temp repo that can be used for testing commands
        var logger = Mock.Of<ILogger>();
        var processManager = new ProcessManager(logger, "git");
        var git = new LocalGitClient(
            Mock.Of<IRemoteTokenProvider>(),
            Mock.Of<ITelemetryRecorder>(),
            processManager,
            new FileSystem(),
            logger);
        var localGitRepoFactory = new LocalGitRepoFactory(git, processManager);
        var repo = localGitRepoFactory.Create(new NativePath(path));
        Repo = repo;
    }

    public ILocalGitRepo Repo { get; }

    public ILocalGitRepoFactory CreateLocalGitRepoFactory()
    {
        // Create a mock factory that always hands back the temp repo.
        var mockLocalGitRepoFactory = new Mock<ILocalGitRepoFactory>();
        mockLocalGitRepoFactory.Setup(f => f.Create(It.IsAny<NativePath>())).Returns(Repo);
        return mockLocalGitRepoFactory.Object;
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(_path, true);
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
