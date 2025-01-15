// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using EmptyFiles;
using VerifyTests.DiffPlex;

namespace Microsoft.DotNet.Docker.Tests;

#nullable enable
[Trait("Category", "pre-build")]
public class VerifyChecksTests
{
    /// <summary>
    /// This test verifies that Verify conventions (.gitignore, .editorconfig, etc.) are properly set up.
    /// Source: https://github.com/VerifyTests/Verify/blob/508f901e76bb241191e6136a5a0ca558b33be268/src/Verify/ConventionCheck/InnerVerifyChecks.cs#L20-L21
    /// Documentation: https://github.com/VerifyTests/Verify/blob/main/docs/wiz/Linux_Other_Cli_Xunit_AzureDevOps.md#conventions-check
    /// </summary>
    /// <returns></returns>
    [Fact]
    public Task Run() =>
        VerifyChecks.Run();
}

public static class ModuleInitializer
{
    /// <summary>
    /// Enable DiffPlex output for line-by-line diffs in test output.
    /// https://github.com/VerifyTests/Verify.DiffPlex/blob/main/readme.md
    /// </summary>
    [ModuleInitializer]
    public static void Initialize() =>
        VerifyDiffPlex.Initialize(OutputType.Compact);

    /// <summary>
    /// Enable comparison of Dockerfiles as text files, since they don't have a file extension.
    /// https://github.com/VerifyTests/Verify/blob/main/docs/verify-directory.md#files-with-no-extension
    /// </summary>
    [ModuleInitializer]
    public static void InitTextFileConvention() =>
        FileExtensions.AddTextFileConvention(path =>
            Path.GetFileName(path).Equals("Dockerfile", StringComparison.InvariantCulture));
}
