// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public static class ExecuteHelper
    {
        public static TimeSpan Timeout = TimeSpan.FromMinutes(5);

        public static (Process Process, string StdOut, string StdErr) ExecuteProcess(
            string fileName, string args, ITestOutputHelper outputHelper)
        {
            Process process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo =
                {
                    FileName = fileName,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            output = output.Trim();
            error = error.Trim();

            outputHelper?.WriteLine(output);
            outputHelper?.WriteLine(error);

            return (process, output, error);
        }
    }
}
