﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class ProductImageData : ImageData
    {
        private string _sdkOS;
        private string _osTag;
        private ImageVersion? _versionFamily;

        public bool HasCustomSdk => _sdkOS != null;

        public string SdkOS
        {
            get
            {
                if (_sdkOS != null)
                {
                    return _sdkOS;
                }

                return OS;
            }
            set { _sdkOS = value; }
        }

        public string OSTag
        {
            get
            {
                if (_osTag is not null)
                {
                    return _osTag;
                }

                return OS;
            }
            set { _osTag = value; }
        }

        public ImageVersion Version { get; set; }

        public ImageVersion VersionFamily
        {
            get { return _versionFamily.GetValueOrDefault(Version); }
            set { _versionFamily = value; }
        }

        public string VersionString => Version.ToString();

        public override int DefaultPort => (IsDistroless | Version.Major >= 8) ? 8080 : 80;

        public override int? NonRootUID {
            get {
                if (OS == Tests.OS.Mariner10Distroless)
                {
                    return 1000;
                }
                else if (OS == Tests.OS.Mariner20Distroless && (Version.Major == 6 || Version.Major == 7))
                {
                    return 101;
                }
                else
                {
                    return base.NonRootUID;
                }
            }
        }

        public string GetDockerfilePath(DotNetImageType imageType) =>
            $"src/{GetVariantName(imageType)}/{Version}/{OSTag}/{GetArchLabel()}";

        public override string GetIdentifier(string type) => $"{VersionString}-{base.GetIdentifier(type)}";

        public static string GetVariantName(DotNetImageType imageType) =>
            Enum.GetName(typeof(DotNetImageType), imageType).ToLowerInvariant().Replace('_', '-');

        public string GetImage(DotNetImageType imageType, DockerHelper dockerHelper)
        {
            string tag = GetTagName(imageType);
            string imageName = GetImageName(tag, GetVariantName(imageType));

            PullImageIfNecessary(imageName, dockerHelper);

            return imageName;
        }

        public string GetProductVersion(DotNetImageType imageType, DockerHelper dockerHelper)
        {
            string imageName = GetImage(imageType, dockerHelper);
            string containerName = GetIdentifier($"GetProductVersion-{imageType}");

            return imageType switch
            {
                DotNetImageType.SDK => dockerHelper.Run(imageName, containerName, "dotnet --version"),
                DotNetImageType.Runtime => GetRuntimeVersion(imageName, containerName, "Microsoft.NETCore.App", dockerHelper),
                DotNetImageType.Aspnet => GetRuntimeVersion(imageName, containerName, "Microsoft.AspNetCore.App", dockerHelper),
                _ => throw new NotSupportedException($"Unsupported image type '{imageType}'"),
            };
        }

        private string GetRuntimeVersion(string imageName, string containerName, string runtimeName, DockerHelper dockerHelper)
        {
            const string versionGroupName = "Version";

            string runtimeListing = dockerHelper.Run(imageName, containerName, FormatDotnetCommand("--list-runtimes"));
            Regex versionRegex = new Regex($"{runtimeName} (?<{versionGroupName}>[^\\s]+) ");
            Match match = versionRegex.Match(runtimeListing);
            return match.Success ? match.Groups[versionGroupName].Value : string.Empty;
        }

        private string FormatDotnetCommand(string command)
        {
            // For distroless, dotnet will be the default entrypoint so we don't need to specify "dotnet" in the command.
            // See https://github.com/dotnet/dotnet-docker/issues/3866
            string executable = !IsDistroless ||
                (OS.Contains(Tests.OS.Mariner) && (OS == Tests.OS.Mariner10Distroless || Version.Major == 6)) ?
                    "dotnet " :
                    string.Empty;
            return executable + command;
        }

        private string GetTagName(DotNetImageType imageType)
        {
            ImageVersion imageVersion;
            string os;
            switch (imageType)
            {
                case DotNetImageType.Runtime:
                case DotNetImageType.Aspnet:
                case DotNetImageType.Runtime_Deps:
                case DotNetImageType.Monitor:
                    imageVersion = Version;
                    os = OSTag;
                    break;
                case DotNetImageType.SDK:
                    imageVersion = Version;
                    os = SdkOS;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported image type '{imageType}'");
            }

            return GetTagName(imageVersion.GetTagName(), os);
        }
    }
}
