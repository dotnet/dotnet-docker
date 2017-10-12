// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class DockerHelper
    {
        public static string DockerOS => GetDockerOS();
        public static string ContainerWorkDir => IsLinuxContainerModeEnabled ? "/sandbox" : "c:\\sandbox";
        public static bool IsLinuxContainerModeEnabled => string.Equals(DockerOS, "linux", StringComparison.OrdinalIgnoreCase);
        private ITestOutputHelper Output { get; set; }

        public DockerHelper(ITestOutputHelper output)
        {
            Output = output;
        }

        public void Build(string dockerfile, string tag, string fromImage, params string[] buildArgs)
        {
            string buildArgsOption = $"--build-arg base_image={fromImage}";
            if (buildArgs != null)
            {
                foreach (string arg in buildArgs)
                {
                    buildArgsOption += $" --build-arg {arg}";
                }
            }

            Execute($"build -t {tag} {buildArgsOption} -f {dockerfile} .");
        }

        public void DeleteImage(string tag)
        {
            if (ImageExists(tag))
            {
                Execute($"image rm -f {tag}");
            }
        }

        public void DeleteVolume(string name)
        {
            if (ResourceExists("volume", name))
            {
                Execute($"volume rm -f {name}");
            }
        }

        private void Execute(string args)
        {
            Output.WriteLine($"Executing : docker {args}");
            ProcessStartInfo info = new ProcessStartInfo("docker", args);
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            Process process = Process.Start(info);
            process.WaitForExit();
            Output.WriteLine(process.StandardOutput.ReadToEnd());

            if (process.ExitCode != 0)
            {
                string stdErr = process.StandardError.ReadToEnd();
                string msg = $"Failed to execute {info.FileName} {info.Arguments}{Environment.NewLine}{stdErr}";
                throw new InvalidOperationException(msg);
            }
        }

        private static string GetDockerOS()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("docker", "version -f \"{{ .Server.Os }}\"");
            startInfo.RedirectStandardOutput = true;
            Process process = Process.Start(startInfo);
            process.WaitForExit();
            return process.StandardOutput.ReadToEnd().Trim();
        }

        public string GetContainerWorkPath(string relativePath)
        {
            string separator = IsLinuxContainerModeEnabled ? "/" : "\\";
            return $"{ContainerWorkDir}{separator}{relativePath}";
        }

        private static bool ResourceExists(string type, string id)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("docker", $"{type} ls -q {id}");
            startInfo.RedirectStandardOutput = true;
            Process process = Process.Start(startInfo);
            process.WaitForExit();
            return process.ExitCode == 0 && process.StandardOutput.ReadToEnd().Trim() != "";
        }

        public void Run(string image, string command, string containerName, string volumeName = null)
        {
            string volumeArg = volumeName == null ? string.Empty : $" -v {volumeName}:{ContainerWorkDir}";
            Execute($"run --rm --name {containerName}{volumeArg} {image} {command}");
        }

        public static bool ImageExists(string tag)
        {
            return ResourceExists("image", tag);
        }
    }
}
