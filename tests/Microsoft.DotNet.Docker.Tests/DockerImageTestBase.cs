// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests;

/// <summary>
/// Base class for tests that build Docker images. Tracks all images created
/// during test execution and automatically cleans them up after each test
/// completes. Each individual test method will have its images cleaned up
/// after test completion.
/// </summary>
public abstract class DockerImageTestBase : IAsyncLifetime
{
    private readonly List<string> _createdImageTags = [];

    protected ITestOutputHelper OutputHelper { get; }
    protected DockerHelper DockerHelper { get; }

    protected DockerImageTestBase(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
        DockerHelper = new DockerHelper(outputHelper);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        if (_createdImageTags.Count > 0)
        {
            OutputHelper.WriteLine($"Cleaning up {_createdImageTags.Count} image(s) created during test:");
            foreach (string tag in _createdImageTags)
            {
                OutputHelper.WriteLine($"  - {tag}");
            }

            foreach (string tag in _createdImageTags)
            {
                try
                {
                    DockerHelper.DeleteImage(tag);
                }
                catch
                {
                    // Best effort cleanup - don't fail the test if image deletion fails
                    OutputHelper.WriteLine($"Warning: Failed to delete image '{tag}'");
                }
            }

            _createdImageTags.Clear();
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Builds a Docker image and registers it for cleanup after the test completes.
    /// </summary>
    protected void Build(
        string tag,
        string dockerfile = "",
        string target = "",
        string contextDir = ".",
        bool pull = false,
        string platform = "",
        string output = "",
        params string[] buildArgs)
    {
        DockerHelper.Build(tag, dockerfile, target, contextDir, pull, platform, output, buildArgs);
        if (!string.IsNullOrEmpty(tag))
        {
            _createdImageTags.Add(tag);
        }
    }

    /// <summary>
    /// Builds a helper image intended to test distroless scenarios.
    /// </summary>
    /// <remarks>
    /// Because distroless containers do not contain a shell, and potentially other packages necessary for testing,
    /// this helper image stores the entire root of the distroless filesystem at the specified destination path within
    /// the built container image.
    /// </remarks>
    protected string BuildDistrolessHelper(DotNetImageRepo imageRepo, ProductImageData imageData, string copyDestination, string copyOrigin = "/")
    {
        string tag = DockerHelper.BuildDistrolessHelper(imageRepo, imageData, copyDestination, copyOrigin);
        _createdImageTags.Add(tag);
        return tag;
    }
}
