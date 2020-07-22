// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public static class ExecuteHelper
    {
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

            StringBuilder stdOutput = new StringBuilder();
            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) => stdOutput.AppendLine(e.Data));

            StringBuilder stdError = new StringBuilder();
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => stdError.AppendLine(e.Data));

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            string output = stdOutput.ToString().Trim();
            if (outputHelper != null && !string.IsNullOrWhiteSpace(output))
            {
                outputHelper.WriteLine(output);
            }

            string error = stdError.ToString().Trim();
            if (outputHelper != null && !string.IsNullOrWhiteSpace(error))
            {
                outputHelper.WriteLine(error);
            }

            return (process, output, error);
        }
    }
}
