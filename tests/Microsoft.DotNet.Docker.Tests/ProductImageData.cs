// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Docker.Tests
{
    public class ProductImageData : ImageData
    {
        private string _sdkOS;
        private string _osTag;
        private ImageVersion? _versionFamily;

        public bool HasCustomSdk => _sdkOS != null;

        public bool GlobalizationInvariantMode => (!ImageVariant.HasFlag(DotNetImageVariant.Extra)
                    || Version.Major == 6
                    || Version.Major == 7)
                && (IsDistroless || OS.Contains(Tests.OS.Alpine));

        public string SdkOS
        {
            get => HasCustomSdk ? _sdkOS : OS;
            init => _sdkOS = value;
        }

        public DotNetImageVariant SdkImageVariant { get; init; } = DotNetImageVariant.None;

        public string OSTag
        {
            get => _osTag != null ? _osTag : OS;
            init => _osTag = value;
        }

        public ImageVersion Version { get; init; }

        public ImageVersion VersionFamily
        {
            get { return _versionFamily.GetValueOrDefault(Version); }
            init { _versionFamily = value; }
        }

        public DotNetImageVariant ImageVariant { get; init;}

        public DotNetImageRepo SupportedImageRepos { get; init; } = DotNetImageRepo.Runtime_Deps
                                                                    | DotNetImageRepo.Runtime
                                                                    | DotNetImageRepo.Aspnet
                                                                    | DotNetImageRepo.SDK;

        public string VersionString => Version.ToString();

        public override int DefaultPort => (IsDistroless || (Version.Major != 6 && Version.Major != 7)) ? 8080 : 80;

        public override int? NonRootUID =>
            OS == Tests.OS.Mariner20Distroless && (Version.Major == 6 || Version.Major == 7) ? 101 : base.NonRootUID;

        public string GetDockerfilePath(DotNetImageRepo imageRepo) =>
            $"src/{GetImageRepoName(imageRepo)}{GetVariantSuffix()}/{Version}/{OSTag}/{GetArchLabel()}";

        private string GetVariantSuffix() =>
            ImageVariant == DotNetImageVariant.None ? "" : $"-{GetImageVariantName(ImageVariant)}";

        public override string GetIdentifier(string type) => $"{VersionString}-{base.GetIdentifier(type)}";

        public static string GetImageRepoName(DotNetImageRepo imageRepo) =>
            Enum.GetName(typeof(DotNetImageRepo), imageRepo).ToLowerInvariant().Replace('_', '-');

        public static string GetImageVariantName(DotNetImageVariant imageVariant)
        {
            IEnumerable<string> imageVariants = [];
            foreach (DotNetImageVariant enumValue in Enum.GetValues(typeof(DotNetImageVariant)))
            {
                if (enumValue != DotNetImageVariant.None && imageVariant.HasFlag(enumValue))
                {
                    imageVariants = imageVariants.Append(Enum.GetName(typeof(DotNetImageVariant), enumValue).ToLowerInvariant());
                }
            }
            return string.Join('-', imageVariants);
        }

        public string GetImage(DotNetImageRepo imageRepo, DockerHelper dockerHelper)
        {
            // ASP.NET composite includes its own runtime that we want to test.
            if (ImageVariant.HasFlag(DotNetImageVariant.Composite) && imageRepo == DotNetImageRepo.Runtime)
            {
                imageRepo = DotNetImageRepo.Aspnet;
            }

            if (imageRepo != DotNetImageRepo.SDK && !SupportedImageRepos.HasFlag(imageRepo))
            {
                throw new ArgumentOutOfRangeException(nameof(imageRepo),
                    $"Unsupported image type '{imageRepo}' for Image Variant '{ImageVariant}'");
            }

            string tag = GetTagName(imageRepo);
            string imageName = GetImageName(tag, GetImageRepoName(imageRepo));

            PullImageIfNecessary(imageName, dockerHelper);

            return imageName;
        }

        public string GetProductVersion(DotNetImageRepo imageRepoToUse, DotNetImageRepo productVersionRepo, DockerHelper dockerHelper)
        {
            string imageName = GetImage(imageRepoToUse, dockerHelper);
            return GetProductVersion(imageName, productVersionRepo, dockerHelper);
        }

        public string GetProductVersion(string imageName, DotNetImageRepo productVersionRepo, DockerHelper dockerHelper)
        {
            string containerName = GetIdentifier($"GetProductVersion-{productVersionRepo}");

            return productVersionRepo switch
            {
                DotNetImageRepo.SDK => dockerHelper.Run(imageName, containerName, "dotnet --version"),
                DotNetImageRepo.Runtime => GetRuntimeVersion(imageName, containerName, "Microsoft.NETCore.App", dockerHelper),
                DotNetImageRepo.Aspnet => GetRuntimeVersion(imageName, containerName, "Microsoft.AspNetCore.App", dockerHelper),
                _ => throw new NotSupportedException($"Unsupported image type '{productVersionRepo}'"),
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
            string executable = !IsDistroless || (OS.Contains(Tests.OS.Mariner) && Version.Major == 6)
                ? "dotnet "
                : string.Empty;
            return executable + command;
        }

        private string GetTagName(DotNetImageRepo imageRepo)
        {
            ImageVersion imageVersion;
            string os;
            string variant = ImageRepoIsSupported(imageRepo) && ImageVariant != DotNetImageVariant.None
                ? GetImageVariantName(ImageVariant)
                : "";

            switch (imageRepo)
            {
                case DotNetImageRepo.Runtime:
                case DotNetImageRepo.Aspnet:
                case DotNetImageRepo.Runtime_Deps:
                case DotNetImageRepo.Monitor:
                    imageVersion = Version;
                    os = OSTag;
                    break;
                case DotNetImageRepo.SDK:
                    imageVersion = Version;
                    os = SdkOS;
                    variant = GetImageVariantName(SdkImageVariant);
                    break;
                default:
                    throw new NotSupportedException($"Unsupported image type '{imageRepo}'");
            }

            return GetTagName(imageVersion.GetTagName(), os, variant);
        }

        public bool ImageRepoIsSupported(DotNetImageRepo imageRepo) => SupportedImageRepos.HasFlag(imageRepo);
    }
}
