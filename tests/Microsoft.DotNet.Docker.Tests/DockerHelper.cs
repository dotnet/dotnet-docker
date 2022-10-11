// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class DockerHelper
    {
        public static string DockerOS => GetDockerOS();
        public static string DockerArchitecture => GetDockerArch();
        public static string ContainerWorkDir => IsLinuxContainerModeEnabled ? "/sandbox" : "c:\\sandbox";
        public static bool IsLinuxContainerModeEnabled => string.Equals(DockerOS, "linux", StringComparison.OrdinalIgnoreCase);
        public static string TestArtifactsDir { get; } = Path.Combine(Directory.GetCurrentDirectory(), "TestAppArtifacts");

        private ITestOutputHelper OutputHelper { get; set; }

        public DockerHelper(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public void Build(
            string tag,
            string dockerfile = null,
            string target = null,
            string contextDir = ".",
            bool pull = false,
            params string[] buildArgs)
        {
            string buildArgsOption = string.Empty;
            if (buildArgs != null)
            {
                foreach (string arg in buildArgs)
                {
                    buildArgsOption += $" --build-arg {arg}";
                }
            }

            string targetArg = target == null ? string.Empty : $" --target {target}";
            string dockerfileArg = dockerfile == null ? string.Empty : $" -f {dockerfile}";
            string pullArg = pull ? " --pull" : string.Empty;

            ExecuteWithLogging($"build -t {tag}{targetArg}{buildArgsOption}{dockerfileArg}{pullArg} {contextDir}");
        }

        /// <summary>
        /// Builds a helper image intended to test distroless scenarios.
        /// </summary>
        /// <remarks>
        /// Because distroless containers do not contain a shell, and potentially other packages necessary for testing,
        /// this helper image stores the entire root of the distroless filesystem at the specified destination path within
        /// the built container image.
        /// </remarks>
        public string BuildDistrolessHelper(DotNetImageType imageType, ProductImageData imageData, string rootDestination)
        {
            string dockerfile = Path.Combine(TestArtifactsDir, "Dockerfile.distroless");
            string distrolessImageTag = imageData.GetImage(imageType, this);
            string baseImageTag = distrolessImageTag
                .Replace("-distroless", string.Empty)
                .Replace("-chiseled", string.Empty);

            string tag = imageData.GetIdentifier("distroless-helper");
            Build(tag, dockerfile, null, TestArtifactsDir, false,
                $"distroless_image={distrolessImageTag}",
                $"base_image={baseImageTag}",
                $"root_destination={rootDestination}");
            return tag;
        }

        public static bool ContainerExists(string name) => ResourceExists("container", $"-f \"name={name}\"");

        public void Copy(string src, string dest) => ExecuteWithLogging($"cp {src} {dest}");

        public void DeleteContainer(string container, bool captureLogs = false)
        {
            if (ContainerExists(container))
            {
                if (captureLogs)
                {
                    ExecuteWithLogging($"logs {container}", ignoreErrors: true);
                }

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

        private static string Execute(
            string args, bool ignoreErrors = false, bool autoRetry = false, ITestOutputHelper outputHelper = null)
        {
            (Process Process, string StdOut, string StdErr) result;
            if (autoRetry)
            {
                result = ExecuteWithRetry(args, outputHelper, ExecuteProcess);
            }
            else
            {
                result = ExecuteProcess(args, outputHelper);
            }

            if (!ignoreErrors && result.Process.ExitCode != 0)
            {
                ProcessStartInfo startInfo = result.Process.StartInfo;
                string msg = $"Failed to execute {startInfo.FileName} {startInfo.Arguments}" +
                    $"{Environment.NewLine}Exit code: {result.Process.ExitCode}" +
                    $"{Environment.NewLine}Standard Error: {result.StdErr}";
                throw new InvalidOperationException(msg);
            }

            return result.StdOut;
        }

        private static (Process Process, string StdOut, string StdErr) ExecuteProcess(
            string args, ITestOutputHelper outputHelper) => ExecuteHelper.ExecuteProcess("docker", args, outputHelper);

        private string ExecuteWithLogging(string args, bool ignoreErrors = false, bool autoRetry = false)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            OutputHelper.WriteLine($"Executing: docker {args}");
            string result = Execute(args, outputHelper: OutputHelper, ignoreErrors: ignoreErrors, autoRetry: autoRetry);

            stopwatch.Stop();
            OutputHelper.WriteLine($"Execution Elapsed Time: {stopwatch.Elapsed}");

            return result;
        }

        private static (Process Process, string StdOut, string StdErr) ExecuteWithRetry(
            string args,
            ITestOutputHelper outputHelper,
            Func<string, ITestOutputHelper, (Process Process, string StdOut, string StdErr)> executor)
        {
            const int maxRetries = 5;
            const int waitFactor = 5;

            int retryCount = 0;

            (Process Process, string StdOut, string StdErr) result = executor(args, outputHelper);
            while (result.Process.ExitCode != 0)
            {
                retryCount++;
                if (retryCount >= maxRetries)
                {
                    break;
                }

                int waitTime = Convert.ToInt32(Math.Pow(waitFactor, retryCount - 1));
                if (outputHelper != null)
                {
                    outputHelper.WriteLine($"Retry {retryCount}/{maxRetries}, retrying in {waitTime} seconds...");
                }

                Thread.Sleep(waitTime * 1000);
                result = executor(args, outputHelper);
            }

            return result;
        }

        private static string GetDockerOS() => Execute("version -f \"{{ .Server.Os }}\"");
        private static string GetDockerArch() => Execute("version -f \"{{ .Server.Arch }}\"");

        public string GetImageUser(string image) => ExecuteWithLogging($"inspect -f \"{{{{ .Config.User }}}}\" {image}");

        public IDictionary<string, string> GetEnvironmentVariables(string image)
{
            string envVarsStr = ExecuteWithLogging($"inspect -f \"{{{{json .Config.Env }}}}\" {image}");
            JArray envVarsArray = (JArray)JsonConvert.DeserializeObject(envVarsStr);
            return envVarsArray
                .ToDictionary(
                    item => item.ToString().Split('=')[0],
                    item => item.ToString().Split('=')[1]);
        }

        public string GetContainerAddress(string container)
        {
            string containerAddress = ExecuteWithLogging("inspect -f \"{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}\" " + container);
            if (String.IsNullOrWhiteSpace(containerAddress)){
                containerAddress = ExecuteWithLogging("inspect -f \"{{.NetworkSettings.Networks.nat.IPAddress }}\" " + container);
            }

            return containerAddress;
        }

        public string GetContainerHostPort(string container, int containerPort = 80) =>
            ExecuteWithLogging(
                $"inspect -f \"{{{{(index (index .NetworkSettings.Ports \\\"{containerPort}/tcp\\\") 0).HostPort}}}}\" {container}");

        public string GetContainerWorkPath(string relativePath)
        {
            string separator = IsLinuxContainerModeEnabled ? "/" : "\\";
            return $"{ContainerWorkDir}{separator}{relativePath}";
        }

        public static bool ImageExists(string tag) => ResourceExists("image", tag);

        public void Pull(string image) => ExecuteWithLogging($"pull {image}", autoRetry: true);

        public string GetHistory(string image) =>
            ExecuteWithLogging($"history --no-trunc --format \"{{{{ .CreatedBy }}}}\" {image}");

        private static bool ResourceExists(string type, string filterArg)
        {
            string output = Execute($"{type} ls -a -q {filterArg}", true);
            return output != "";
        }

        public string Run(
            string image,
            string name,
            string command = null,
            string workdir = null,
            string optionalRunArgs = null,
            bool detach = false,
            string runAsUser = null,
            bool skipAutoCleanup = false,
            bool useMountedDockerSocket = false)
        {
            string cleanupArg = skipAutoCleanup ? string.Empty : " --rm";
            string detachArg = detach ? " -d -t" : string.Empty;
            string userArg = runAsUser != null ? $" -u {runAsUser}" : string.Empty;
            string workdirArg = workdir == null ? string.Empty : $" -w {workdir}";
            string mountedDockerSocketArg = useMountedDockerSocket ? " -v /var/run/docker.sock:/var/run/docker.sock" : string.Empty;
            return ExecuteWithLogging(
                $"run --name {name}{cleanupArg}{workdirArg}{userArg}{detachArg}{mountedDockerSocketArg} {optionalRunArgs} {image} {command}");
        }

        /// <summary>
        /// Creates a file system volume that is backed by memory instead of disk.
        /// </summary>
        public string CreateTmpfsVolume(string name, bool ownedByDistrolessUser = false)
        {
            // Create volume using the local driver (the default driver),
            // which accepts options similar to the 'mount' command.
            //
            // Additional options are specified to:
            // - make this volume an in-memory file system with a unique device name (type=tmpfs, device={guid}}).
            // - to set the owner of the root of the file system (o=uid=101).
            string optionalArgs = string.Empty;
            if (ownedByDistrolessUser)
            {
                optionalArgs += " --opt o=uid=101";
            }
            string device = Guid.NewGuid().ToString("D");
            return ExecuteWithLogging($"volume create --opt type=tmpfs --opt device={device}{optionalArgs} {name}");
        }

        public string DeleteVolume(string name)
        {
            return ExecuteWithLogging($"volume remove {name}");
        }
    }
}
