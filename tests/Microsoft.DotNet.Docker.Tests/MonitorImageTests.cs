// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "monitor")]
    public class MonitorImageTests : CommonRuntimeImageTests
    {
        private const int DefaultHttpPort = 80;
        private const int DefaultArtifactsPort = 52323;
        private const int DefaultMetricsPort = 52325;

        private const string UrlPath_Processes = "processes";
        private const string UrlPath_Metrics = "metrics";

        private const string Directory_Diag = "/diag";
        private const string Directory_Tmp = "/tmp";

        private const string File_DiagPort = Directory_Diag + "/port";

        /// <summary>
        /// Command line that is the default command line presented in the image.
        /// When specifying additional command arguments, these must be prepended to
        /// maintain existing behavior.
        /// </summary>
        private const string Switch_DefaultImageCmd = "collect --urls https://+:52323 --metricUrls http://+:52325";

        /// <summary>
        /// Command line switch to disable authentication. By default,
        /// dotnet-monitor requires authentication on the artifacts port.
        /// </summary>
        private const string Switch_NoAuthentication = "--no-auth";

        public MonitorImageTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override DotNetImageRepo ImageRepo => DotNetImageRepo.Monitor;

        public static IEnumerable<object[]> GetMonitorImageData() =>
            TestData.GetMonitorImageData()
                .Select(imageData => new object[] { imageData });

        /// <summary>
        /// Gets each dotnet-monitor image paired with each sample aspnetcore image of the same architecture.
        /// Allows for testing volume mounts and diagnostic port usage among different distros.
        /// </summary>
        public static IEnumerable<object[]> GetScenarioData()
        {
            IList<object[]> data = new List<object[]>();
            foreach (ProductImageData ProductImageData in TestData.GetMonitorImageData())
            {
                foreach (SampleImageData sampleImageData in TestData.GetAllSampleImageData())
                {
                    // Only use published images (do not want to build unpublished images in the tests)
                    if (!sampleImageData.IsPublished)
                        continue;

                    // Only consider the sample image if it has the same architecture.
                    if (ProductImageData.Arch != sampleImageData.Arch)
                        continue;

                    data.Add(new object[] { ProductImageData, sampleImageData });
                }
            }
            return data;
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetMonitorImageData))]
        public void VerifyInsecureFiles(ProductImageData imageData)
        {
            base.VerifyCommonInsecureFiles(imageData);
        }

        [LinuxImageTheory]
        [MemberData(nameof(GetMonitorImageData))]
        public void VerifyShellNotInstalledForDistroless(ProductImageData imageData)
        {
            base.VerifyCommonShellNotInstalledForDistroless(imageData);
        }

        [DotNetTheory]
        [MemberData(nameof(GetMonitorImageData))]
        public void VerifyNoSasToken(ProductImageData imageData)
        {
            base.VerifyCommonNoSasToken(imageData);
        }

        [DotNetTheory]
        [MemberData(nameof(GetMonitorImageData))]
        public void VerifyDefaultUser(ProductImageData imageData)
        {
            VerifyCommonDefaultUser(imageData);
        }

        /// <summary>
        /// Verifies that the environment variables essential to dotnet-monitor are set correctly.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetMonitorImageData))]
        public void VerifyEnvironmentVariables(ProductImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();
            variables.AddRange(ProductImageTests.GetCommonEnvironmentVariables());

            if (imageData.Version.Major <= 7) {
                // ASPNETCORE_URLS has been unset to allow the default URL binding to occur.
                variables.Add(new EnvironmentVariableInfo("ASPNETCORE_URLS", string.Empty));
            } else {
                variables.Add(new EnvironmentVariableInfo("ASPNETCORE_HTTP_PORTS", string.Empty));
            }
            // Diagnostics should be disabled
            variables.Add(new EnvironmentVariableInfo("COMPlus_EnableDiagnostics", "0"));
            // DefaultProcess filter should select a process with a process ID of 1
            variables.Add(new EnvironmentVariableInfo("DefaultProcess__Filters__0__Key", "ProcessId"));
            variables.Add(new EnvironmentVariableInfo("DefaultProcess__Filters__0__Value", "1"));
            // Existing (orphaned) diagnostic port should be delete before starting server
            variables.Add(new EnvironmentVariableInfo("DiagnosticPort__DeleteEndpointOnStartup", "true"));
            if (imageData.Version.Major != 6)
            {
                // GC mode should be set to Server
                variables.Add(new EnvironmentVariableInfo("DOTNET_gcServer", "1"));
            }
            // Console logger format should be JSON and output UTC timestamps without timezone information
            variables.Add(new EnvironmentVariableInfo("Logging__Console__FormatterName", "json"));
            variables.Add(new EnvironmentVariableInfo("Logging__Console__FormatterOptions__TimestampFormat", "yyyy-MM-ddTHH:mm:ss.fffffffZ"));
            variables.Add(new EnvironmentVariableInfo("Logging__Console__FormatterOptions__UseUtcTimestamp", "true"));

            EnvironmentVariableInfo.Validate(
                variables,
                imageData.GetImage(ImageRepo, DockerHelper),
                imageData,
                DockerHelper);
        }

        /// <summary>
        /// Tests that the image can run without additional configuration
        /// and the metrics endpoint is usable without providing authentication.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetMonitorImageData))]
        public Task VerifyMonitorDefault(ProductImageData imageData)
        {
            return VerifyMonitorAsync(imageData, noAuthentication: false);
        }

        /// <summary>
        /// Tests that the image can run without https enabled, that the artifacts ports
        /// respond with Unauthroized, and the metrics endpoint is usable without
        /// providing authentication.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetMonitorImageData))]
        public Task VerifyMonitorNoHttpsUnconfiguredAuth(ProductImageData imageData)
        {
            return VerifyMonitorAsync(
                imageData,
                noAuthentication: false,
                async containerName =>
                {
                    if (!Config.IsHttpVerificationDisabled)
                    {
                        // Verify processes returns 401 (Unauthorized) since authentication was not configured.
                        await ImageScenarioVerifier.VerifyHttpResponseFromContainerAsync(
                            containerName,
                            DockerHelper,
                            OutputHelper,
                            DefaultArtifactsPort,
                            UrlPath_Processes,
                            m => VerifyStatusCode(m, HttpStatusCode.Unauthorized));
                    }
                },
                builder =>
                {
                    // Reset and expose the artifacts port over http (not secure)
                    builder.MonitorUrl(DefaultArtifactsPort);
                });
        }

        /// <summary>
        /// Tests that the image can run without https and authenciation, thus the artifacts
        /// and the metrics ports are usable without providing authentication.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetMonitorImageData))]
        public Task VerifyMonitorNoHttpsNoAuth(ProductImageData imageData)
        {
            return VerifyMonitorAsync(
                imageData,
                noAuthentication: true,
                async containerName =>
                {
                    if (!Config.IsHttpVerificationDisabled)
                    {
                        // Verify metrics endpoint is accessible and produces zero processes
                        using HttpResponseMessage processesMessage =
                            await ImageScenarioVerifier.GetHttpResponseFromContainerAsync(
                                containerName,
                                DockerHelper,
                                OutputHelper,
                                DefaultArtifactsPort,
                                UrlPath_Processes);

                        JsonElement rootElement = GetContentAsJsonElement(processesMessage);

                        // Verify returns an empty array (should not detect any processes)
                        Assert.Equal(JsonValueKind.Array, rootElement.ValueKind);
                        Assert.Equal(0, rootElement.GetArrayLength());
                    }
                },
                builder =>
                {
                    // Reset and expose the artifacts port over http (not secure)
                    builder.MonitorUrl(DefaultArtifactsPort);
                });
        }

        /// <summary>
        /// Tests that the image can run without https enabled and that the artifacts ports
        /// are accessible with valid authorization header.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetMonitorImageData))]
        public Task VerifyMonitorNoHttpsWithAuth(ProductImageData imageData)
        {
            GenerateKeyOutput output = GenerateKey(imageData);
            AuthenticationHeaderValue authorizationHeader = AuthenticationHeaderValue.Parse(output.AuthorizationHeader);

            return VerifyMonitorAsync(
                imageData,
                noAuthentication: false,
                async containerName =>
                {
                    if (!Config.IsHttpVerificationDisabled)
                    {
                        // Verify processes returns 401 (Unauthorized) since authentication was not provided.
                        await ImageScenarioVerifier.VerifyHttpResponseFromContainerAsync(
                            containerName,
                            DockerHelper,
                            OutputHelper,
                            DefaultArtifactsPort,
                            UrlPath_Processes,
                            m => VerifyStatusCode(m, HttpStatusCode.Unauthorized));

                        // Verify processes is accessible using authorization header
                        using HttpResponseMessage processesMessage =
                            await ImageScenarioVerifier.GetHttpResponseFromContainerAsync(
                                containerName,
                                DockerHelper,
                                OutputHelper,
                                DefaultArtifactsPort,
                                UrlPath_Processes,
                                authorizationHeader: authorizationHeader);

                        JsonElement rootElement = GetContentAsJsonElement(processesMessage);

                        // Verify returns an empty array (should not detect any processes)
                        Assert.Equal(JsonValueKind.Array, rootElement.ValueKind);
                        Assert.Equal(0, rootElement.GetArrayLength());
                    }
                },
                builder =>
                {
                    // Reset and expose the artifacts port over http (not secure)
                    builder.MonitorUrl(DefaultArtifactsPort);
                    // Configuration authentication
                    builder.MonitorApiKey(output.Authentication.MonitorApiKey);
                },
                authorizationHeader);
        }

        /// <summary>
        /// Verifies that the image can discover a dotnet process
        /// in another container via mounting the /tmp directory.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetScenarioData))]
        public Task VerifyConnectMode(ProductImageData imageData, SampleImageData sampleData)
        {
            return VerifyScenarioAsync(
                productImageData: imageData,
                sampleImageData: sampleData,
                shareTmpVolume: true,
                listenDiagPortVolume: false,
                noAuthentication: true,
                async (monitorName, sampleName) =>
                {
                    if (!Config.IsHttpVerificationDisabled)
                    {
                        using HttpResponseMessage responseMessage =
                            await ImageScenarioVerifier.GetHttpResponseFromContainerAsync(
                                monitorName,
                                DockerHelper,
                                OutputHelper,
                                DefaultArtifactsPort,
                                UrlPath_Processes);

                        JsonElement rootElement = GetContentAsJsonElement(responseMessage);

                        // Verify returns an array with one element (the sample container process)
                        Assert.Equal(JsonValueKind.Array, rootElement.ValueKind);
                        Assert.Equal(1, rootElement.GetArrayLength());
                    }
                });
        }

        /// <summary>
        /// Verifies that the image can listen for dotnet processes
        /// in other containers by having them connect to the diagnostic port listener.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetScenarioData))]
        public Task VerifyListenMode(ProductImageData imageData, SampleImageData sampleData)
        {
            return VerifyScenarioAsync(
                productImageData: imageData,
                sampleImageData: sampleData,
                shareTmpVolume: false,
                listenDiagPortVolume: true,
                noAuthentication: true,
                async (monitorName, sampleName) =>
                {
                    if (!Config.IsHttpVerificationDisabled)
                    {
                        using HttpResponseMessage responseMessage =
                            await ImageScenarioVerifier.GetHttpResponseFromContainerAsync(
                                monitorName,
                                DockerHelper,
                                OutputHelper,
                                DefaultArtifactsPort,
                                UrlPath_Processes);

                        JsonElement rootElement = GetContentAsJsonElement(responseMessage);

                        // Verify returns an array with one element (the sample container process)
                        Assert.Equal(JsonValueKind.Array, rootElement.ValueKind);
                        Assert.Equal(1, rootElement.GetArrayLength());
                    }
                });
        }

        /// <summary>
        /// Runs a single instance of the dotnet-monitor image.
        /// </summary>
        /// <param name="imageData">The image data of the dotnet-monitor image.</param>
        /// <param name="noAuthentication">Set to true to disable dotnet-monitor authenication.</param>
        /// <param name="verifyContainerAsync">Callback to test some aspect of the container.</param>
        /// <param name="runArgsCallback">Allows for modifying the "docker run" args of the container.</param>
        private async Task VerifyMonitorAsync(
            ProductImageData imageData,
            bool noAuthentication,
            Func<string, Task> verifyContainerAsync = null,
            Action<DockerRunArgsBuilder> runArgsCallback = null,
            AuthenticationHeaderValue authorizationHeader = null
            )
        {
            GetNames(imageData, out string monitorImageName, out string monitorContainerName, out _);
            try
            {
                DockerRunArgsBuilder runArgsBuilder = DockerRunArgsBuilder.Create()
                    .ExposePort(DefaultMetricsPort);

                if (null != runArgsCallback)
                {
                    runArgsCallback(runArgsBuilder);
                }

                DockerHelper.Run(
                    image: monitorImageName,
                    name: monitorContainerName,
                    command: GetMonitorAdditionalArgs(imageData, noAuthentication),
                    detach: true,
                    optionalRunArgs: runArgsBuilder.Build());

                if (!Config.IsHttpVerificationDisabled)
                {
                    // Verify metrics endpoint is accessible
                    using HttpResponseMessage metricsMessage =
                        await ImageScenarioVerifier.GetHttpResponseFromContainerAsync(
                            monitorContainerName,
                            DockerHelper,
                            OutputHelper,
                            DefaultMetricsPort,
                            UrlPath_Metrics,
                            authorizationHeader: authorizationHeader);

                    string metricsContent = await metricsMessage.Content.ReadAsStringAsync();

                    // Metrics should not return any content if
                    // no processes are detected.
                    Assert.Equal(string.Empty, metricsContent);
                }

                if (null != verifyContainerAsync)
                {
                    await verifyContainerAsync(monitorContainerName);
                }
            }
            finally
            {
                DockerHelper.DeleteContainer(monitorContainerName);
            }
        }

        /// <summary>
        /// Runs a single instance of each of the dotnet-monitor and samples images.
        /// </summary>
        /// <param name="productImageData">The image data of the dotnet-monitor image.</param>
        /// <param name="shareTmpVolume">Set to true to mount the /tmp directory in both containers.</param>
        /// <param name="listenDiagPortVolume">
        /// Set to true to have the monitor container listen with a diagnostic port listener
        /// for diagnostic connections from the samples container.
        /// </param>
        /// <param name="noAuthentication">Set to true to disable dotnet-monitor authenication.</param>
        /// <param name="verifyContainerAsync">Callback to test some aspect of the containers.</param>
        /// <param name="monitorRunArgsCallback">Allows for modifying the "docker run" args of the dotnet-monitor container.</param>
        /// <param name="sampleRunArgsCallback">Allows for modifying the "docker run" args of the samples container.</param>
        private async Task VerifyScenarioAsync(
            ProductImageData productImageData,
            SampleImageData sampleImageData,
            bool shareTmpVolume,
            bool listenDiagPortVolume,
            bool noAuthentication,
            Func<string, string, Task> verifyContainerAsync,
            Action<DockerRunArgsBuilder> monitorRunArgsCallback = null,
            Action<DockerRunArgsBuilder> sampleRunArgsCallback = null)
        {
            GetNames(productImageData, out string monitorImageName, out string monitorContainerName, out int monitorUser);
            GetNames(sampleImageData, out string sampleImageName, out string sampleContainerName, out int sampleUser);

            DockerRunArgsBuilder monitorArgsBuilder = DockerRunArgsBuilder.Create()
                .MonitorUrl(DefaultArtifactsPort);

            DockerRunArgsBuilder sampleArgsBuilder = DockerRunArgsBuilder.Create()
                .ExposePort(DefaultHttpPort);

            string diagPortVolumeName = null;
            string tmpVolumeName = null;

            try
            {
                int? volumeUid = AdjustMonitorUserAndCalculateVolumeOwner(
                    listenDiagPortVolume,
                    sampleUser,
                    monitorUser,
                    monitorArgsBuilder);

                // Create a volume for the two containers to share the /tmp directory.
                if (shareTmpVolume)
                {
                    tmpVolumeName = DockerHelper.CreateTmpfsVolume(UniqueName("tmpvol"), volumeUid);

                    monitorArgsBuilder.VolumeMount(tmpVolumeName, Directory_Tmp);

                    sampleArgsBuilder.VolumeMount(tmpVolumeName, Directory_Tmp);
                }

                // Create a volume so that the dotnet-monitor container can provide a
                // diagnostic listening port to the samples container so that the samples
                // process can connect to the dotnet-monitor process.
                if (listenDiagPortVolume)
                {
                    diagPortVolumeName = DockerHelper.CreateTmpfsVolume(UniqueName("diagportvol"), volumeUid);

                    monitorArgsBuilder.VolumeMount(diagPortVolumeName, Directory_Diag);
                    monitorArgsBuilder.MonitorListen(File_DiagPort);

                    sampleArgsBuilder.VolumeMount(diagPortVolumeName, Directory_Diag);
                    sampleArgsBuilder.RuntimeSuspend(File_DiagPort);
                }

                // Allow modification of the "docker run" args of the monitor container
                if (null != monitorRunArgsCallback)
                {
                    monitorRunArgsCallback(monitorArgsBuilder);
                }

                // Allow modification of the "docker run" args of the samples container
                if (null != sampleRunArgsCallback)
                {
                    sampleRunArgsCallback(sampleArgsBuilder);
                }

                // Run the sample container
                DockerHelper.Run(
                    image: sampleImageName,
                    name: sampleContainerName,
                    detach: true,
                    optionalRunArgs: sampleArgsBuilder.Build());

                // Run the dotnet-monitor container
                DockerHelper.Run(
                    image: monitorImageName,
                    name: monitorContainerName,
                    command: GetMonitorAdditionalArgs(productImageData, noAuthentication),
                    detach: true,
                    optionalRunArgs: monitorArgsBuilder.Build());

                await verifyContainerAsync(
                    monitorContainerName,
                    sampleContainerName);
            }
            finally
            {
                DockerHelper.DeleteContainer(monitorContainerName);

                DockerHelper.DeleteContainer(sampleContainerName);

                if (!string.IsNullOrEmpty(diagPortVolumeName))
                {
                    DockerHelper.DeleteVolume(diagPortVolumeName);
                }

                if (!string.IsNullOrEmpty(tmpVolumeName))
                {
                    DockerHelper.DeleteVolume(tmpVolumeName);
                }
            }
        }

        private static int? AdjustMonitorUserAndCalculateVolumeOwner(bool listenMode, int sampleUid, int monitorUid, DockerRunArgsBuilder monitorArgsBuilder)
        {
            // Make sure volume is accessible by sample app without modifying the sample container
            // and ensure monitor app can access it, even if needing to change its user. This is
            // done by making the volume owned by the least privileged user; if they are the same user or
            // are different non-root users, defer to the user of the sample image.

            if (!IsRoot(sampleUid))
            {
                if (sampleUid != monitorUid)
                {
                    // If sample is connecting to monitor OR (monitor is connecting to sample AND is not running as root),
                    // change monitor to run as same user as sample. In general, a client has to either be root or
                    // the same user as the server for UDS connections.
                    if (listenMode || !IsRoot(monitorUid))
                    {
                        monitorArgsBuilder.AsUser(sampleUid);
                    }
                }

                return sampleUid;
            }
            else if (!IsRoot(monitorUid))
            {
                // Sample is root; monitor is non-root

                if (listenMode)
                {
                    // Monitor has UDS to which sample will connect, which it can without changes since
                    // it is running as root; use monitor user as volume owner
                    return monitorUid;
                }
                else
                {
                    // Sample has UDS to which monitor will connect, which requires monitor to run as
                    // root as well; use sample user as volume owner
                    monitorArgsBuilder.AsUser(sampleUid);

                    return sampleUid;
                }
            }

            // Both sample and monitor run as root: no volume ownership change necessary
            Debug.Assert(IsRoot(sampleUid) && IsRoot(monitorUid));
            return null;

            static bool IsRoot(int uid)
            {
                return 0 == uid;
            }
        }

        private static string UniqueName(string name)
        {
            return $"{name}-{DateTime.Now.ToFileTime()}";
        }

        private static SampleImageData GetSampleImageData(ProductImageData imageData)
        {
            return TestData.GetSampleImageData()
                .First(d => d.IsPublished = true && d.Arch == imageData.Arch);
        }

        private static string GetMonitorAdditionalArgs(ProductImageData imageData, bool noAuthentication)
        {
            const char spaceChar = ' ';

            // This determines if we are going to add the default args that are included in the entrypoint in images before 7.0
            // This flag should be thought of as "We want to add anything to the commandline and are 7.0+".
            // This is required for 7.0+ images when command line arguments are being appended because docker
            // will treat the presence of any commandline args as overriding the entire CMD block in the DockerFile.
            bool addDefaultArgs =
                // We are version 7.0+, this will never apply to 6.x images
                imageData.Version.Major >= 7 &&
                // We are adding anything to the command line. When additional flags are added to this method, `noAuthentication`
                // should be replaced something like `(noAuthentication || myNewFlag || mySetting != Setting.Default)`
                noAuthentication;

            // Standard here is to have the built command line always end with a space, so it needs to start with one
            StringBuilder builtCommandline = new StringBuilder(spaceChar);

            if (addDefaultArgs)
            {
                builtCommandline.AppendFormat("{0} ", Switch_DefaultImageCmd);
            }

            if (noAuthentication)
            {
                builtCommandline.AppendFormat("{0} ", Switch_NoAuthentication);
            }

            string cmdsResult = builtCommandline.ToString().Trim(spaceChar);

            return cmdsResult;
        }

        private void GetNames(ProductImageData imageData, out string imageName, out string containerName, out int userIdentifier)
        {
            imageName = imageData.GetImage(ImageRepo, DockerHelper);
            containerName = imageData.GetIdentifier("monitortest");
            // If user is empty, then image is running as same as docker daemon (typically root)
            userIdentifier = string.IsNullOrEmpty(DockerHelper.GetImageUser(imageName)) ? 0 : imageData.NonRootUID.Value;
        }

        private void GetNames(SampleImageData imageData, out string imageName, out string containerName, out int userIdentifier)
        {
            // Need to allow pulling of the sample image since these are not built in the same pipeline
            // as the other images; otherwise, these tests will fail due to lack of sample image.
            string tag = imageData.GetTagNameBase(SampleImageType.Aspnetapp);
            imageName = imageData.GetImage(tag, DockerHelper, allowPull: true);
            containerName = imageData.GetIdentifier("monitortest-sample");
            // If user is empty, then image is running as same as docker daemon (typically root)
            userIdentifier = string.IsNullOrEmpty(DockerHelper.GetImageUser(imageName)) ? 0 : imageData.NonRootUID.Value;
        }

        private void VerifyStatusCode(HttpResponseMessage message, HttpStatusCode statusCode)
        {
            if (message.StatusCode != statusCode)
            {
                throw new HttpRequestException($"Expected status code {statusCode}", null, statusCode);
            }
        }

        private static JsonElement GetContentAsJsonElement(HttpResponseMessage message)
        {
            using (Stream stream = message.Content.ReadAsStream())
            {
                return JsonDocument.Parse(stream).RootElement;
            }
        }

        private GenerateKeyOutput GenerateKey(ProductImageData imageData)
        {
            GetNames(imageData, out string monitorImageName, out string monitorContainerName, out _);
            try
            {
                DockerRunArgsBuilder runArgsBuilder = DockerRunArgsBuilder.Create()
                    .Entrypoint("dotnet-monitor");

                string json = DockerHelper.Run(
                    image: monitorImageName,
                    name: monitorContainerName,
                    command: "generatekey -o machinejson",
                    optionalRunArgs: runArgsBuilder.Build());

                GenerateKeyOutput output = JsonSerializer.Deserialize<GenerateKeyOutput>(json);

                Assert.NotNull(output?.Authentication?.MonitorApiKey?.PublicKey);
                Assert.NotNull(output?.Authentication?.MonitorApiKey?.Subject);
                Assert.NotNull(output?.AuthorizationHeader);

                return output;
            }
            finally
            {
                DockerHelper.DeleteContainer(monitorContainerName);
            }
        }
    }

    internal static class MonitorDockerRunArgsBuilderExtensions
    {
        // dotnet-monitor variables
        internal const string EnvVar_Authentication_MonitorApiKey_PublicKey = "DotnetMonitor_Authentication__MonitorApiKey__PublicKey";
        internal const string EnvVar_Authentication_MonitorApiKey_Subject = "DotnetMonitor_Authentication__MonitorApiKey__Subject";
        internal const string EnvVar_DiagnosticPort_ConnectionMode = "DotnetMonitor_DiagnosticPort__ConnectionMode";
        internal const string EnvVar_DiagnosticPort_EndpointName = "DotnetMonitor_DiagnosticPort__EndpointName";
        internal const string EnvVar_Metrics_Enabled = "DotnetMonitor_Metrics__Enabled";
        internal const string EnvVar_Urls = "DotnetMonitor_Urls";

        // runtime variables
        internal const string EnvVar_DiagnosticPorts = "DOTNET_DiagnosticPorts";

        public static DockerRunArgsBuilder MonitorApiKey(this DockerRunArgsBuilder builder, MonitorApiKeyOptions options)
        {
            return builder
                .EnvironmentVariable(EnvVar_Authentication_MonitorApiKey_PublicKey, options.PublicKey)
                .EnvironmentVariable(EnvVar_Authentication_MonitorApiKey_Subject, options.Subject);
        }

        /// <summary>
        /// Disables the metrics endpoint in dotnet-monitor.
        /// </summary>
        public static DockerRunArgsBuilder MonitorDisableMetrics(this DockerRunArgsBuilder builder)
        {
            return builder.EnvironmentVariable(EnvVar_Metrics_Enabled, "false");
        }

        /// <summary>
        /// Places dotnet-monitor into listen mode, allowing dotnet processes to connect
        /// to its diagnostic port listener.
        /// </summary>
        public static DockerRunArgsBuilder MonitorListen(this DockerRunArgsBuilder builder, string endpointName)
        {
            return builder
                .EnvironmentVariable(EnvVar_DiagnosticPort_ConnectionMode, "Listen")
                .EnvironmentVariable(EnvVar_DiagnosticPort_EndpointName, endpointName);
        }

        /// <summary>
        /// Sets the artifacts url with the port and exposes the port on the container.
        /// </summary>
        public static DockerRunArgsBuilder MonitorUrl(this DockerRunArgsBuilder builder, int port)
        {
            return builder.ExposePort(port)
                .EnvironmentVariable(EnvVar_Urls, WildcardUrl(port));
        }

        /// <summary>
        /// Suspends a dotnet runtime until it can connect to a diagnostic port listener
        /// at the specified endpoint name.
        /// </summary>
        public static DockerRunArgsBuilder RuntimeSuspend(this DockerRunArgsBuilder builder, string endpointName)
        {
            return builder.EnvironmentVariable(EnvVar_DiagnosticPorts, $"{endpointName},suspend");
        }

        private static string WildcardUrl(int port)
        {
            return $"http://*:{port}";
        }
    }

    /// <summary>
    /// Represents the structured output of a "dotnet-monitor generatekey -o machinejson" invocation.
    /// </summary>
    internal sealed class GenerateKeyOutput
    {
        public AuthenticationOptions Authentication { get; set; }

        public string AuthorizationHeader { get; set; }
    }

    internal sealed class AuthenticationOptions
    {
        public MonitorApiKeyOptions MonitorApiKey { get; set; }
    }

    internal sealed class MonitorApiKeyOptions
    {
        public string PublicKey { get; set; }

        public string Subject { get; set; }
    }
}
