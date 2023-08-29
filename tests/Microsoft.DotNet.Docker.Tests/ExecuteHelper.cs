// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
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

            StringBuilder stdOutput = new StringBuilder();
            StringBuilder stdError = new StringBuilder();

            using (AutoResetEvent outputWaitHandle = new(false))
            using (AutoResetEvent errorWaitHandle = new(false))
            {
                process.OutputDataReceived += (sender, e) => {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        stdOutput.Append(e.Data);
                    }
                };

                process.ErrorDataReceived += (sender, e) => {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        stdError.Append(e.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                int timeoutInMs = Convert.ToInt32(Timeout.TotalMilliseconds);

                if (process.WaitForExit(timeoutInMs)
                    && outputWaitHandle.WaitOne(timeoutInMs)
                    && errorWaitHandle.WaitOne(timeoutInMs))
                {
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
                else
                {
                    throw new TimeoutException();
                }
            }
        }
    }
}
