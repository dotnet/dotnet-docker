// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.DotNet.Docker.Tests;

public class TestDockerfile(IEnumerable<string> args, IEnumerable<string> layers)
{
    public const string BuildStageName = "build";
    public const string PublishStageName = "publish";
    public const string AppStageName = "app";

    public string Content { get; } = GetContent(args, layers);

    private static string GetContent(IEnumerable<string> args, IEnumerable<string> layers)
    {
        IEnumerable<string> argsLines = args.Select(arg => "ARG " + arg);
        string argsContent = string.Join(Environment.NewLine, argsLines);

        IEnumerable<string> layerContents = layers.Select(layerContent =>
            string.Join(Environment.NewLine, layerContent));

        string dockerfileContent = string.Join(
            Environment.NewLine + Environment.NewLine,
            [ argsContent, ..layerContents ]);

        return dockerfileContent;
    }
}

public static class TestDockerfileBuilder
{
    private const string CopyNuGetConfigCommands = 
        """
        WORKDIR /source
        COPY NuGet.config .
        """;

    private static DockerOS s_os = DockerHelper.IsLinuxContainerModeEnabled
        ? DockerOS.Linux
        : DockerOS.Windows;

    private static bool s_useNuGetConfig = Config.IsNightlyRepo;

    private static string[] s_commonArgs = [
        "sdk_image",
        "runtime_image",
        "runtime_deps_image",
    ];

    public static TestDockerfile GetDefaultDockerfile(PublishConfig publishConfig)
    {
        string[] publishAndAppLayers = publishConfig switch
        {
            PublishConfig.Aot =>
                [
                    $"""
                    FROM {TestDockerfile.BuildStageName} AS {TestDockerfile.PublishStageName}
                    RUN dotnet publish -r {FormatArg("rid")} --no-restore -o /app
                    """,
                    $"""
                    FROM $runtime_deps_image AS {TestDockerfile.AppStageName}
                    WORKDIR /app
                    COPY --from=publish /app .
                    USER $APP_UID
                    ENTRYPOINT ["./app"]
                    """
                ],
            PublishConfig.FxDependent =>
                [
                    $"""
                    FROM {TestDockerfile.BuildStageName} AS {TestDockerfile.PublishStageName}
                    RUN dotnet publish --no-restore -c Release -o out
                    """,
                    $"""
                    FROM $runtime_image AS {TestDockerfile.AppStageName}
                    ARG port
                    EXPOSE $port
                    WORKDIR /app
                    COPY --from=publish /source/app/out ./
                    ENTRYPOINT ["dotnet", "app.dll"]
                    """,
                ],
            PublishConfig.SelfContained =>
                [
                    $"""
                    FROM {TestDockerfile.BuildStageName} AS {TestDockerfile.PublishStageName}
                    ARG rid
                    RUN dotnet publish -r {FormatArg("rid")} -c Release --self-contained true -o out
                    """,
                    $"""
                    FROM $runtime_deps_image AS {TestDockerfile.AppStageName}
                    ARG port
                    EXPOSE $port
                    WORKDIR /app
                    COPY --from=publish /source/app/out ./
                    ENTRYPOINT ["./app"]
                    """,
                ],
            _ => throw new NotImplementedException($"Unknown publish configuration {publishConfig}"),
        };

        return new TestDockerfile(
            args: s_commonArgs,
            layers: [ GetDefaultBuildLayer(), ..publishAndAppLayers ]);
    }

    public static TestDockerfile GetTestProjectDockerfile()
    {
        string testLayer =
            $"""
            FROM {TestDockerfile.BuildStageName} AS {TestDockerfile.AppStageName}
            ARG rid
            ARG NuGetFeedPassword
            WORKDIR /source/tests
            COPY tests/*.csproj .
            RUN dotnet restore -r {FormatArg("rid")}
            COPY tests/ .
            ENTRYPOINT ["dotnet", "test", "--logger:trx", "--no-restore"]
            """;

        return new TestDockerfile(
            args: s_commonArgs,
            layers: [ GetDefaultBuildLayer(), testLayer ]);
    }

    public static TestDockerfile GetBlazorWasmDockerfile(bool useWasmTools)
    {
        string nugetConfigFileOption = s_useNuGetConfig
            ? "--configfile NuGet.config"
            : string.Empty;

        StringBuilder buildLayerBuilder = new(
            $"""
            FROM $sdk_image AS {TestDockerfile.BuildStageName}
            ARG port
            EXPOSE $port
            """);
        
        if (s_useNuGetConfig)
        {
            buildLayerBuilder.AppendLine();
            buildLayerBuilder.AppendLine(CopyNuGetConfigCommands);
        }

        if (useWasmTools)
        {
            buildLayerBuilder.AppendLine();
            buildLayerBuilder.AppendLine(
                $"""
                RUN dotnet workload install {nugetConfigFileOption} --skip-manifest-update wasm-tools \
                    && . /etc/os-release \
                    && case $ID in \
                        alpine) apk add --no-cache python3 ;; \
                        debian | ubuntu) apt-get update \
                            && apt-get install -y --no-install-recommends python3 \
                            && rm -rf /var/lib/apt/lists/* ;; \
                        mariner | azurelinux) tdnf install -y python3 \
                            && tdnf clean all ;; \
                    esac
                """);
        }

        buildLayerBuilder.AppendLine();
        buildLayerBuilder.AppendLine(
            """
            WORKDIR /source/app
            COPY app/*.csproj .
            RUN dotnet restore
            COPY app/ .
            RUN dotnet build --no-restore
            """);

        string buildLayer = buildLayerBuilder.ToString();

        StringBuilder publishLayerBuilder = new(
            $"""
            FROM {TestDockerfile.BuildStageName} AS {TestDockerfile.PublishStageName}
            ARG rid
            """);
        
        publishLayerBuilder.AppendLine();
        publishLayerBuilder.AppendLine(useWasmTools
            ? "RUN dotnet publish -r browser-wasm -c Release --self-contained true -o out"
            : "RUN dotnet publish --no-restore -c Release -o out");
        
        string publishLayer = publishLayerBuilder.ToString();

        // Blazor WASM output is a static site - there are no runtime executables to be ran in the app stage.
        // Endpoint access is verified in the build stage in the SDK dockerfile.
        // App stage can remain empty in order to test publish functionality.
        string appLayer = $"""FROM $runtime_deps_image AS {TestDockerfile.AppStageName}""";
        
        return new TestDockerfile(s_commonArgs, layers: [ buildLayer, publishLayer, appLayer ]);
    }

    private static string GetDefaultBuildLayer()
    {
        StringBuilder buildLayerBuilder = new(
            $"""
            FROM $sdk_image AS {TestDockerfile.BuildStageName}
            ARG rid
            ARG NuGetFeedPassword
            ARG port
            EXPOSE $port
            """);        

        if (s_useNuGetConfig)
        {
            buildLayerBuilder.AppendLine();
            buildLayerBuilder.AppendLine(CopyNuGetConfigCommands);
        }

        buildLayerBuilder.AppendLine();
        buildLayerBuilder.AppendLine(
            $"""
            WORKDIR /source/app
            COPY app/*.csproj .
            RUN dotnet restore -r {FormatArg("rid")}
            COPY app/ .
            RUN dotnet build --no-restore
            """);

        return buildLayerBuilder.ToString();
    }

    private static string FormatArg(string arg) => s_os == DockerOS.Windows ? $"%{arg}%" : $"${arg}";
}

public enum DockerOS {
    Linux,
    Windows,
}

public enum PublishConfig {
    Aot,
    FxDependent,
    SelfContained,
}
