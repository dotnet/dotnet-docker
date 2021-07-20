// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.Docker.Tests
{
    [Trait("Category", "monitor")]
    public class MonitorImageTests
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
        /// Command line switch to disable authentication. By default,
        /// dotnet-monitor requires authentication on the artifacts port.
        /// </summary>
        private const string Switch_NoAuthentication = "--no-auth";

        public MonitorImageTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
            DockerHelper = new DockerHelper(outputHelper);
        }

        protected DockerHelper DockerHelper { get; }

        protected ITestOutputHelper OutputHelper { get; }

        public static IEnumerable<object[]> GetImageData()
        {
            return TestData.GetMonitorImageData()
                .Select(imageData => new object[] { imageData });
        }

        /// <summary>
        /// Gets each dotnet-monitor image paired with each sample aspnetcore image of the same architecture.
        /// Allows for testing volume mounts and diagnostic port usage among different distros.
        /// </summary>
        public static IEnumerable<object[]> GetScenarioData()
        {
            IList<object[]> data = new List<object[]>();
            foreach (MonitorImageData monitorImageData in TestData.GetMonitorImageData())
            {
                foreach (SampleImageData sampleImageData in TestData.GetAllSampleImageData())
                {
                    // Only use published images (do not want to build unpublished images in the tests)
                    if (!sampleImageData.IsPublished)
                        continue;

                    // Only consider the sample image if it has the same architecture.
                    if (monitorImageData.Arch != sampleImageData.Arch)
                        continue;

                    data.Add(new object[] { monitorImageData, sampleImageData });
                }
            }
            return data;
        }

        /// <summary>
        /// Verifies that the environment variables essential to dotnet-monitor are set correctly.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public void VerifyEnvironmentVariables(MonitorImageData imageData)
        {
            List<EnvironmentVariableInfo> variables = new List<EnvironmentVariableInfo>();
            variables.AddRange(ProductImageTests.GetCommonEnvironmentVariables());

            // ASPNETCORE_URLS has been unset to allow the default URL binding to occur.
            variables.Add(new EnvironmentVariableInfo("ASPNETCORE_URLS", string.Empty));
            // Diagnostics should be disabled
            variables.Add(new EnvironmentVariableInfo("COMPlus_EnableDiagnostics", "0"));
            // DefaultProcess filter should select a process with a process ID of 1
            variables.Add(new EnvironmentVariableInfo("DefaultProcess__Filters__0__Key", "ProcessId"));
            variables.Add(new EnvironmentVariableInfo("DefaultProcess__Filters__0__Value", "1"));
            // Console logger format should be JSON and output UTC timestamps without timezone information
            variables.Add(new EnvironmentVariableInfo("Logging__Console__FormatterName", "json"));
            variables.Add(new EnvironmentVariableInfo("Logging__Console__FormatterOptions__TimestampFormat", "yyyy-MM-ddTHH:mm:ss.fffffffZ"));
            variables.Add(new EnvironmentVariableInfo("Logging__Console__FormatterOptions__UseUtcTimestamp", "true"));

            EnvironmentVariableInfo.Validate(
                variables,
                imageData.GetImage(DockerHelper),
                imageData,
                DockerHelper);
        }

        /// <summary>
        /// Tests that the image can run without additional configuration
        /// and the metrics endpoint is usable without providing authentication.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public Task VerifyMonitorDefault(MonitorImageData imageData)
        {
            return VerifyMonitorAsync(imageData, noAuthentication: false);
        }

        /// <summary>
        /// Tests that the image can run without https enabled, that the artifacts ports
        /// respond with Unauthroized, and the metrics endpoint is usable without
        /// providing authentication.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetImageData))]
        public Task VerifyMonitorNoHttps(MonitorImageData imageData)
        {
            return VerifyMonitorAsync(
                imageData,
                noAuthentication: false,
                async containerName =>
                {
                    if (!Config.IsHttpVerificationDisabled)
                    {
                        // Verify processes returns 401 (Unauthorized) since authentication was not provided.
                        using HttpResponseMessage processesMessage =
                        await ImageScenarioVerifier.GetHttpResponseFromContainerAsync(
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
        [MemberData(nameof(GetImageData))]
        public Task VerifyMonitorNoHttpsNoAuth(MonitorImageData imageData)
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
        /// Verifies that the image can discover a dotnet process
        /// in another container via mounting the /tmp directory.
        /// </summary>
        [LinuxImageTheory]
        [MemberData(nameof(GetScenarioData))]
        public Task VerifyConnectMode(MonitorImageData imageData, SampleImageData sampleData)
        {
            return VerifyScenarioAsync(
                monitorImageData: imageData,
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
        public Task VerifyListenMode(MonitorImageData imageData, SampleImageData sampleData)
        {
            return VerifyScenarioAsync(
                monitorImageData: imageData,
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
            MonitorImageData imageData,
            bool noAuthentication,
            Func<string, Task> verifyContainerAsync = null,
            Action<DockerRunArgsBuilder> runArgsCallback = null)
        {
            GetNames(imageData, out string monitorImageName, out string monitorContainerName);
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
                    command: GetMonitorAdditionalArgs(noAuthentication),
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
                            UrlPath_Metrics);

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
        /// <param name="monitorImageData">The image data of the dotnet-monitor image.</param>
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
            MonitorImageData monitorImageData,
            SampleImageData sampleImageData,
            bool shareTmpVolume,
            bool listenDiagPortVolume,
            bool noAuthentication,
            Func<string, string, Task> verifyContainerAsync,
            Action<DockerRunArgsBuilder> monitorRunArgsCallback = null,
            Action<DockerRunArgsBuilder> sampleRunArgsCallback = null)
        {
            GetNames(monitorImageData, out string monitorImageName, out string monitorContainerName);
            GetNames(sampleImageData, out string sampleImageName, out string sampleContainerName);

            DockerRunArgsBuilder monitorArgsBuilder = DockerRunArgsBuilder.Create()
                .MonitorUrl(DefaultArtifactsPort);

            DockerRunArgsBuilder sampleArgsBuilder = DockerRunArgsBuilder.Create()
                .ExposePort(DefaultHttpPort);

            string diagPortVolumeName = null;
            string tmpVolumeName = null;

            try
            {
                // Create a volume for the two containers to share the /tmp directory.
                if (shareTmpVolume)
                {
                    tmpVolumeName = DockerHelper.CreateVolume(UniqueName("tmpvol"));

                    monitorArgsBuilder.VolumeMount(tmpVolumeName, Directory_Tmp);

                    sampleArgsBuilder.VolumeMount(tmpVolumeName, Directory_Tmp);
                }

                // Create a volume so that the dotnet-monitor container can provide a
                // diagnostic listening port to the samples container so that the samples
                // process can connect to the dotnet-monitor process.
                if (listenDiagPortVolume)
                {
                    diagPortVolumeName = DockerHelper.CreateVolume(UniqueName("diagportvol"));

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
                    command: GetMonitorAdditionalArgs(noAuthentication),
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

        private static string UniqueName(string name)
        {
            return $"{name}-{DateTime.Now.ToFileTime()}";
        }

        private static SampleImageData GetSampleImageData(MonitorImageData imageData)
        {
            return TestData.GetSampleImageData()
                .First(d => d.IsPublished = true && d.Arch == imageData.Arch);
        }

        private static string GetMonitorAdditionalArgs(bool noAuthentication)
        {
            return noAuthentication ? Switch_NoAuthentication : null;
        }

        private void GetNames(MonitorImageData imageData, out string imageName, out string containerName)
        {
            imageName = imageData.GetImage(DockerHelper);
            containerName = imageData.GetIdentifier("monitortest");
        }

        private void GetNames(SampleImageData imageData, out string imageName, out string containerName)
        {
            // Need to allow pulling of the sample image since these are not built in the same pipeline
            // as the other images; otherwise, these tests will fail due to lack of sample image.
            imageName = imageData.GetImage(SampleImageType.Aspnetapp, DockerHelper, allowPull: true);
            containerName = imageData.GetIdentifier("monitortest-sample");
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
    }

    internal static class MonitorDockerRunArgsBuilderExtensions
    {
        // dotnet-monitor variables
        internal const string EnvVar_DiagnosticPort_ConnectionMode = "DotnetMonitor_DiagnosticPort__ConnectionMode";
        internal const string EnvVar_DiagnosticPort_EndpointName = "DotnetMonitor_DiagnosticPort__EndpointName";
        internal const string EnvVar_Metrics_Enabled = "DotnetMonitor_Metrics__Enabled";
        internal const string EnvVar_Urls = "DotnetMonitor_Urls";

        // runtime variables
        internal const string EnvVar_DiagnosticPorts = "DOTNET_DiagnosticPorts";

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
}
