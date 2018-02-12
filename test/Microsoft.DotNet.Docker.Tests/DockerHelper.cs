// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class DockerHelper
    {
        public static string DockerOS => GetDockerOS();
        public static string ContainerWorkDir => IsLinuxContainerModeEnabled ? "/sandbox" : "c:\\sandbox";
        public static bool IsLinuxContainerModeEnabled => string.Equals(DockerOS, "linux", StringComparison.OrdinalIgnoreCase);
        private ITestOutputHelper OutputHelper { get; set; }

        public DockerHelper(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
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

            ExecuteWithLogging($"build -t {tag} {buildArgsOption} -f {dockerfile} .");
        }

        public void DeleteImage(string tag)
        {
            if (ImageExists(tag))
            {
                ExecuteWithLogging($"image rm -f {tag}");
            }
        }

        public void DeleteVolume(string name)
        {
            if (VolumeExists(name))
            {
                ExecuteWithLogging($"volume rm -f {name}");
            }
        }

        private void ExecuteWithLogging(string args)
        {
            OutputHelper.WriteLine($"Executing : docker {args}");
            Execute(args, outputHelper:OutputHelper);
        }

        private static string Execute(string args, bool ignoreErrors = false, ITestOutputHelper outputHelper = null)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("docker", args);
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            Process process = Process.Start(startInfo);

            StringBuilder stdOutput = new StringBuilder();
            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) => stdOutput.AppendLine(e.Data));

            StringBuilder stdError = new StringBuilder();
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => stdError.AppendLine(e.Data));

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            string output = stdOutput.ToString().Trim();
            if (outputHelper != null && !string.IsNullOrWhiteSpace(output))
            {
                outputHelper.WriteLine(output);
            }

            if (!ignoreErrors && process.ExitCode != 0)
            {
                string msg = $"Failed to execute {startInfo.FileName} {startInfo.Arguments}{Environment.NewLine}{stdError}";
                throw new InvalidOperationException(msg);
            }

            return output;
        }

        private static string GetDockerOS()
        {
            return Execute("version -f \"{{ .Server.Os }}\"");
        }

        public string GetContainerWorkPath(string relativePath)
        {
            string separator = IsLinuxContainerModeEnabled ? "/" : "\\";
            return $"{ContainerWorkDir}{separator}{relativePath}";
        }

        public static bool ImageExists(string tag)
        {
            return ResourceExists("image", tag);
        }

        private static bool ResourceExists(string type, string filterArg)
        {
            string output = Execute($"{type} ls -q {filterArg}", true);
            return output != "";
        }

        public void Run(
            string image,
            string command,
            string containerName,
            string volumeName = null,
            bool runAsContainerAdministrator = false)
        {
            string volumeArg = volumeName == null ? string.Empty : $" -v {volumeName}:{ContainerWorkDir}";
            string userArg = runAsContainerAdministrator ? " -u ContainerAdministrator" : string.Empty;
            ExecuteWithLogging($"run --rm --name {containerName}{volumeArg}{userArg} {image} {command}");
        }

        public static bool VolumeExists(string name)
        {
            return ResourceExists("volume", $"-f \"name={name}\"");
        }
    }
}
