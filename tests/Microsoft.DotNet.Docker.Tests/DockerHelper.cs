// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

        public static bool ContainerExists(string name)
        {
            return ResourceExists("container", $"-f \"name={name}\"");
        }

        public void DeleteContainer(string container)
        {
            if (ContainerExists(container))
            {
                ExecuteWithLogging($"logs {container}", ignoreErrors: true);
                ExecuteWithLogging($"container rm -f {container}");
            }
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

        private string ExecuteWithLogging(string args, bool ignoreErrors = false)
        {
            OutputHelper.WriteLine($"Executing : docker {args}");
            return Execute(args, outputHelper: OutputHelper, ignoreErrors: ignoreErrors);
        }

        private static string Execute(string args, bool ignoreErrors = false, ITestOutputHelper outputHelper = null)
        {
            Process process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo =
                {
                    FileName = "docker",
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

            if (!ignoreErrors && process.ExitCode != 0)
            {
                string msg = $"Failed to execute {process.StartInfo.FileName} {process.StartInfo.Arguments}{Environment.NewLine}{stdError}";
                throw new InvalidOperationException(msg);
            }

            return output;
        }

        private static string GetDockerOS()
        {
            return Execute("version -f \"{{ .Server.Os }}\"");
        }

        public string GetContainerAddress(string container)
        {
            return ExecuteWithLogging("inspect -f \"{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}\" " + container);
        }

        public string GetContainerHostPort(string container, int containerPort = 80)
        {
            return ExecuteWithLogging(
                $"inspect -f \"{{{{(index (index .NetworkSettings.Ports \\\"{containerPort}/tcp\\\") 0).HostPort}}}}\" {container}");
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

        public static void Pull(string image)
        {
            Execute($"pull {image}");
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
            string portPublishArgs = "-p 80",
            bool detach = false,
            bool runAsContainerAdministrator = false)
        {
            string volumeArg = volumeName == null ? string.Empty : $" -v {volumeName}:{ContainerWorkDir}";
            string userArg = runAsContainerAdministrator ? " -u ContainerAdministrator" : string.Empty;
            string detachArg = detach ? " -d  -t " : string.Empty;
            ExecuteWithLogging($"run --rm --name {containerName}{volumeArg}{userArg}{detachArg} {portPublishArgs} {image} {command}");
        }

        public static bool VolumeExists(string name)
        {
            return ResourceExists("volume", $"-f \"name={name}\"");
        }
    }
}
