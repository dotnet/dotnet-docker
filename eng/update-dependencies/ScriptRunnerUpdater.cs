// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.DotNet.VersionTools.Dependencies;

namespace Dotnet.Docker
{
    /// <summary>
    /// An IDependencyUpdater that executes a PowerShell script to perform the update.
    /// </summary>
    public class ScriptRunnerUpdater : IDependencyUpdater
    {
        private string _scriptPath;

        private ScriptRunnerUpdater()
        {
        }

        public static IDependencyUpdater GetDockerfileUpdater(string repoRoot)
        {
            return new ScriptRunnerUpdater()
            {
                _scriptPath = Path.Combine(repoRoot, "eng", "dockerfile-templates", "Get-GeneratedDockerfiles.ps1")
            };
        }

        public static IDependencyUpdater GetReadMeUpdater(string repoRoot)
        {
            return new ScriptRunnerUpdater()
            {
                _scriptPath = Path.Combine(repoRoot, "eng", "readme-templates", "Get-GeneratedReadmes.ps1")
            };
        }

        public IEnumerable<DependencyUpdateTask> GetUpdateTasks(IEnumerable<IDependencyInfo> dependencyInfos)
        {
            return new DependencyUpdateTask[] {
                new DependencyUpdateTask(
                    () => ExecuteScript(),
                    Enumerable.Empty<IDependencyInfo>(),
                    Enumerable.Empty<string>()
                )
            };
        }

        private void ExecuteScript()
        {
            Trace.TraceInformation($"Executing '{_scriptPath}'");

            // Support both execution within Windows 10, Nano Server and Linux environments.
            Process process;
            try
            {
                process = Process.Start("pwsh", _scriptPath);
                process.WaitForExit();
            }
            catch (Win32Exception)
            {
                process = Process.Start("powershell", _scriptPath);
                process.WaitForExit();
            }

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"Unable to successfully execute '{_scriptPath}'");
            }
        }
    }
}
