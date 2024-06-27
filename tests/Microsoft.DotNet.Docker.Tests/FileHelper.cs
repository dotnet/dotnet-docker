// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Diagnostics;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public static class FileHelper
    {
        public static TempFolderContext UseTempFolder()
        {
            return new TempFolderContext();
        }

        public static TempFileContext UseTempFile()
        {
            return new TempFileContext();
        }

        /// <summary>
        /// Returns a value indicating whether the two files are equal.
        /// </summary>
        public static bool CompareFiles(string expectedFilePath, string actualFilePath, ITestOutputHelper outputHelper)
        {
            if (!File.Exists(expectedFilePath))
            {
                throw new FileNotFoundException($"Expected file not found: {expectedFilePath}");
            }

            if (!File.Exists(actualFilePath))
            {
                throw new FileNotFoundException($"Actual file not found: {actualFilePath}");
            }

            string diffOutput = DiffFiles(expectedFilePath, actualFilePath, outputHelper);

            if (!string.IsNullOrEmpty(diffOutput))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Runs a git diff of two files and returns the output.
        /// </summary>
        public static string DiffFiles(string file1Path, string file2Path, ITestOutputHelper outputHelper)
        {
            (Process Process, string StdOut, string StdErr) diffResult =
                ExecuteHelper.ExecuteProcess("git", $"diff --no-index {file1Path} {file2Path}", outputHelper);

            return diffResult.StdOut;
        }
    }

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

    public class TempFileContext : IDisposable
    {
        public TempFileContext()
        {
            do
            {
                Path = System.IO.Path.Combine(
                    System.IO.Path.GetTempPath(),
                    Guid.NewGuid().ToString());
            }
            while (File.Exists(Path));

            File.Create(Path).Dispose();
        }

        public string Path { get; }

        public void Dispose()
        {
            File.Delete(Path);
        }
    }
}
