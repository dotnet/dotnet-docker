// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.VersionTools.Dependencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Dotnet.Docker
{
    /// <summary>
    /// An IDependencyUpdater that will update the tags section of the readme with the latest tags for the repo.
    /// </summary>
    public class ReadMeUpdater : IDependencyUpdater
    {
        private string _repoRoot;

        public ReadMeUpdater(string repoRoot)
        {
            _repoRoot = repoRoot;
        }

        public IEnumerable<DependencyUpdateTask> GetUpdateTasks(IEnumerable<IDependencyInfo> dependencyInfos)
        {
            return new DependencyUpdateTask[] {
                new DependencyUpdateTask(
                    () => InvokeGetTagsDocumentationScript(),
                    Enumerable.Empty<IDependencyInfo>(),
                    Enumerable.Empty<string>()
                )
            };
        }

        private void InvokeGetTagsDocumentationScript()
        {
            Trace.TraceInformation($"InvokeGetTagsDocumentationScript");

            Process process = Process.Start("powershell", Path.Combine(_repoRoot, "scripts", "Get-TagsDocumentation.ps1"));
            process.WaitForExit();
        }
    }
}
