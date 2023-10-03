// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit.Abstractions;
using Xunit;

namespace Microsoft.DotNet.Docker.Tests;

#nullable enable
public class TempFolderContext : IDisposable
{
    public TempFolderContext()
    {
        do
        {
            Path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                Guid.NewGuid().ToString());
        }
        while (Directory.Exists(Path));

        Directory.CreateDirectory(Path);
    }

    public string Path { get; }

    public void Dispose()
    {
        Directory.Delete(Path, true);
    }
}

public static class FileHelper
{
    public static TempFolderContext UseTempFolder()
    {
        return new TempFolderContext();
    }

    public static void CompareFiles(string expectedFilePath, string actualFilePath, ITestOutputHelper outputHelper, bool warnOnDiffs = false)
    {
        string baselineFileText = File.ReadAllText(expectedFilePath).Trim();
        string actualFileText = File.ReadAllText(actualFilePath).Trim();

        string? message = null;

        if (baselineFileText != actualFileText)
        {
            // Retrieve a diff in order to provide a UX which calls out the diffs.
            string diff = DiffFiles(expectedFilePath, actualFilePath, outputHelper);
            string prefix = warnOnDiffs ? "##vso[task.logissue type=warning;]" : string.Empty;
            message = $"{Environment.NewLine}{prefix}Expected file '{expectedFilePath}' does not match actual file '{actualFilePath}`.  {Environment.NewLine}"
                + $"{diff}{Environment.NewLine}";

            if (warnOnDiffs)
            {
                outputHelper.WriteLine(message);
                outputHelper.WriteLine("##vso[task.complete result=SucceededWithIssues;]");
            }
        }

        if (!warnOnDiffs)
        {
            Assert.Null(message);
        }
    }

    /// <summary>
    /// Runs a git diff of two files.
    /// </summary>
    public static string DiffFiles(string file1Path, string file2Path, ITestOutputHelper outputHelper)
    {
        (Process Process, string StdOut, string StdErr) diffResult =
            ExecuteHelper.ExecuteProcess("git", $"diff --no-index {file1Path} {file2Path}", outputHelper);

        return diffResult.StdOut;
    }
}
