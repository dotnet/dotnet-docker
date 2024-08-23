// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

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
    private static DockerOS s_os = DockerHelper.IsLinuxContainerModeEnabled
        ? DockerOS.Linux
        : DockerOS.Windows;

    private static bool s_useNuGetConfig = Config.IsNightlyRepo;

    private static string[] s_args = [
        "sdk_image",
        "runtime_image",
        "runtime_deps_image",
    ];

    public static TestDockerfile GetDefaultDockerfile(PublishConfig publishConfig)
    {
        string publishLayerFromLine = $"FROM {TestDockerfile.BuildStageName} AS {TestDockerfile.PublishStageName}";
        string[] publishAndAppLayers = publishConfig switch
        {
            PublishConfig.Aot =>
                [
                    $"""
                    {publishLayerFromLine}
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
                    {publishLayerFromLine}
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
                    {publishLayerFromLine}
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
            args: s_args,
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
            args: s_args,
            layers: [ GetDefaultBuildLayer(), testLayer ]);
    }

    private static string GetDefaultBuildLayer() =>
        $"""
        FROM $sdk_image AS {TestDockerfile.BuildStageName}
        ARG rid
        ARG NuGetFeedPassword
        ARG port
        EXPOSE $port
        WORKDIR /source
        COPY NuGet.config .
        WORKDIR /source/app
        COPY app/*.csproj .
        RUN dotnet restore -r {FormatArg("rid")}
        COPY app/ .
        RUN dotnet build --no-restore
        """;

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
