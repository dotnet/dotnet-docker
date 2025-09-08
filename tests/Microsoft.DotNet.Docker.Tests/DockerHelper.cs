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
        private static readonly Lazy<string> s_dockerOS = new(GetDockerOS);
        public static string DockerOS => s_dockerOS.Value;

        public static string ContainerWorkDir => IsLinuxContainerModeEnabled ? "/sandbox" : "c:\\sandbox";
        public static bool IsLinuxContainerModeEnabled => string.Equals(DockerOS, "linux", StringComparison.OrdinalIgnoreCase);
        public static string TestArtifactsDir { get; } = Path.Combine(Directory.GetCurrentDirectory(), "TestAppArtifacts");

        private ITestOutputHelper OutputHelper { get; set; }

        public DockerHelper(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        #nullable enable
        public void Build(
            string tag = "",
            string dockerfile = "",
            string target = "",
            string contextDir = ".",
            bool pull = false,
            string platform = "",
            string output = "",
            params string[] buildArgs
        )
        {
            var args = new List<string>();

            // Optional basic flags
            if (!string.IsNullOrWhiteSpace(tag))
            {
                args.Add("-t");
                args.Add(tag);
            }

            if (!string.IsNullOrWhiteSpace(dockerfile))
            {
                args.Add("-f");
                args.Add(dockerfile);
            }

            if (!string.IsNullOrWhiteSpace(target))
            {
                args.Add("--target");
                args.Add(target);
            }

            // Build args
            if (buildArgs is not null)
            {
                foreach (string buildArg in buildArgs)
                {
                    if (!string.IsNullOrWhiteSpace(buildArg))
                    {
                        args.Add("--build-arg");
                        args.Add(buildArg);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(platform))
            {
                args.Add("--platform");
                args.Add(platform);
            }

            if (!string.IsNullOrWhiteSpace(output))
            {
                args.Add("--output");
                args.Add(output);
            }

            if (pull)
            {
                args.Add("--pull");
            }

            args.Add(contextDir);

            ExecuteWithLogging($"build {string.Join(' ', args)}");
        }
        #nullable disable

        /// <summary>
        /// Builds a helper image intended to test distroless scenarios.
        /// </summary>
        /// <remarks>
        /// Because distroless containers do not contain a shell, and potentially other packages necessary for testing,
        /// this helper image stores the entire root of the distroless filesystem at the specified destination path within
        /// the built container image.
        /// </remarks>
        public string BuildDistrolessHelper(DotNetImageRepo imageRepo, ProductImageData imageData, string copyDestination, string copyOrigin = "/")
        {
            string dockerfile = Path.Combine(TestArtifactsDir, "Dockerfile.copy");
            string distrolessImageTag = imageData.GetImage(imageRepo, this);

            // Use the runtime-deps image as the target of the filesystem copy.
            // Not all images are versioned the same as the mainline .NET products.
            // Use the version family (e.g. the .NET product family version) as the
            // version of the runtime-deps image get the correct image.
            ProductImageData runtimeDepsImageData = new()
            {
                Version = imageData.VersionFamily,
                OS = imageData.OS,
                Arch = imageData.Arch,
            };

            // Special case for Aspire Dashboard 9.0 images:
            // Aspire Dashboard 9.0 is based on .NET 8 since Azure Linux 3.0 does not yet have FedRAMP certification.
            // Remove workaround once https://github.com/dotnet/dotnet-docker/issues/5375 is fixed.
            if (imageRepo == DotNetImageRepo.Aspire_Dashboard && imageData.VersionFamily == ImageVersion.V9_0)
            {
                runtimeDepsImageData = runtimeDepsImageData with
                {
                    Version = ImageVersion.V8_0
                };
            }

            // Make sure we don't try to get an image that we don't need before we specify that we want the distro-full
            // version. The image might not be on disk. The correct, distro-full versino will be pulled in the helper
            // image build.
            string baseImageTag = runtimeDepsImageData
                .GetImage(DotNetImageRepo.Runtime_Deps, this, skipPull: true)
                .Replace("-distroless", string.Empty)
                .Replace("-chiseled", string.Empty);

            string tag = imageData.GetIdentifier("distroless-helper");

            Build(tag, dockerfile, null, TestArtifactsDir, false,
                platform: imageData.Platform,
                buildArgs:
                [
                    $"copy_image={distrolessImageTag}",
                    $"base_image={baseImageTag}",
                    $"copy_origin={copyOrigin}",
                    $"copy_destination={copyDestination}"
                ]);

            return tag;
        }

        public static bool ContainerExists(string name) => ResourceExists("container", $"-f \"name={name}\"");

        public static bool ContainerIsRunning(string name) => Execute($"inspect --format=\"{{{{.State.Running}}}}\" {name}") == "true";

        public void Copy(string src, string dest) => ExecuteWithLogging($"cp {src} {dest}");

        public void DeleteContainer(string container, bool captureLogs = false)
        {
            if (ContainerExists(container))
            {
                if (captureLogs)
                {
                    ExecuteWithLogging($"logs {container}", ignoreErrors: true);
                }

                // If a container is already stopped, running `docker stop` again has no adverse effects.
                // This prevents some issues where containers could fail to be forcibly removed while they're running.
                // e.g. https://github.com/dotnet/dotnet-docker/issues/5127
                StopContainer(container);

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

        private void StopContainer(string container)
        {
            if (ContainerExists(container))
            {
                ExecuteWithLogging($"stop {container}", autoRetry: true);
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
            if (string.IsNullOrWhiteSpace(containerAddress)){
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

        /// <summary>
        /// Pulls an image from DockerHub, optionally redirecting it through a
        /// cache registry.
        /// </summary>
        /// <param name="image">
        /// The image to pull, in the format "repo:tag". Since the image is
        /// assumed to be from DockerHub, do not include a registry.
        /// </param>
        /// <returns>
        /// A tag for the image that was pulled. Use this value to refer to the
        /// image in subsequent operations. Do not use the original value of
        /// <paramref name="image"/>.
        /// </returns>
        public string PullDockerHubImage(string image)
        {
            if (!string.IsNullOrEmpty(Config.CacheRegistry))
            {
                image = $"{Config.CacheRegistry}/{image}";
            }

            Pull(image);
            return image;
        }

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
            bool useMountedDockerSocket = false,
            bool silenceOutput = false,
            bool tty = true)
        {
            string cleanupArg = skipAutoCleanup ? string.Empty : " --rm";
            string detachArg = detach ? " -d" : string.Empty;
            string ttyArg = detach && tty ? " -t" : string.Empty;
            string userArg = runAsUser != null ? $" -u {runAsUser}" : string.Empty;
            string workdirArg = workdir == null ? string.Empty : $" -w {workdir}";
            string mountedDockerSocketArg = useMountedDockerSocket ? " -v /var/run/docker.sock:/var/run/docker.sock" : string.Empty;
            if (silenceOutput)
            {
                return Execute(
                    $"run --name {name}{cleanupArg}{workdirArg}{userArg}{detachArg}{ttyArg}{mountedDockerSocketArg} {optionalRunArgs} {image} {command}");
            }
            return ExecuteWithLogging(
                $"run --name {name}{cleanupArg}{workdirArg}{userArg}{detachArg}{ttyArg}{mountedDockerSocketArg} {optionalRunArgs} {image} {command}");
        }

        /// <summary>
        /// Creates a file system volume that is backed by memory instead of disk.
        /// </summary>
        public string CreateTmpfsVolume(string name, int? ownerUid = null)
        {
            // Create volume using the local driver (the default driver),
            // which accepts options similar to the 'mount' command.
            //
            // Additional options are specified to:
            // - make this volume an in-memory file system with a unique device name (type=tmpfs, device={guid}}).
            // - to set the owner of the root of the file system (o=uid=101).
            string optionalArgs = string.Empty;
            if (ownerUid.HasValue)
            {
                optionalArgs += $" --opt o=uid={ownerUid.Value}";
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
