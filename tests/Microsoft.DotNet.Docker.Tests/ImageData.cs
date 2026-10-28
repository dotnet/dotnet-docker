// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.DotNet.Docker.Tests;

public abstract record ImageData
{
    private readonly List<string> _pulledImages = [];

    public Arch Arch { get; set; }
    public bool IsArm => Arch is Arch.Arm or Arch.Arm64;
    public OSInfo OS { get; set; }
    public bool IsDistroless => OS.IsDistroless;
    public virtual int DefaultPort => 8080;
    public virtual int? NonRootUID => IsWindows ? null : 1654;

    private static readonly Lazy<JObject> ImageInfoData;

    static ImageData()
    {
        ImageInfoData = new Lazy<JObject>(() =>
        {
            string imageInfoPath = Environment.GetEnvironmentVariable("IMAGE_INFO_PATH");
            if (!string.IsNullOrEmpty(imageInfoPath))
            {
                string imageInfoContents = File.ReadAllText(imageInfoPath);
                return JsonConvert.DeserializeObject<JObject>(imageInfoContents);
            }

            return null;
        });
    }

    public string Platform
    {
        get
        {
            string os = IsWindows ? "windows" : "linux";
            string arch = Arch.ToString().ToLowerInvariant();
            if (ArchVariant.Length > 0)
            {
                arch += $"/{ArchVariant}";
            }

            return $"{os}/{arch}";
        }
    }

    public string ArchVariant =>
        Arch switch
        {
            Arch.Amd64 => string.Empty,
            Arch.Arm => "v7",
            Arch.Arm64 => "v8",
            _ => throw new NotImplementedException()
        };

    public bool IsWindows => OS.IsWindows;

    public string Rid
    {
        get
        {
            string rid;

            if (IsWindows)
            {
                rid = "win-x64";
            }
            else
            {
                string arch = Arch switch
                {
                    Arch.Arm => "arm",
                    Arch.Arm64 => "arm64",
                    Arch.Amd64 => "x64",
                    _ => throw new NotImplementedException()
                };
                string modifier = OS.Family == OSFamily.Alpine ? "musl-" : "";
                rid = $"linux-{modifier}{arch}";
            }

            return rid;
        }
    }

    public string OsVersion
    {
        get
        {
            const string prefixGroup = "Prefix";
            const string versionGroup = "Version";
            const string ltscPrefix = "ltsc";
            string versionNumber = string.Empty;
            Match match = Regex.Match(OS, @$"(-(?<{prefixGroup}>[a-zA-Z_]*))?(?<{versionGroup}>\d+.\d+)");

            if (match.Groups[prefixGroup].Success && match.Groups[prefixGroup].Value == ltscPrefix)
            {
                versionNumber = ltscPrefix;
            }

            versionNumber += match.Groups[versionGroup].Value;
            return versionNumber;
        }
    }

    public virtual string GetIdentifier(string type) => GenerateContainerName(type);

    public static string GenerateContainerName(string prefix) => $"{prefix}-{DateTime.Now.ToFileTime()}";

    protected void PullImageIfNecessary(string imageName, DockerHelper dockerHelper, bool allowPull = false)
    {
        if ((Config.PullImages || allowPull) && !_pulledImages.Contains(imageName))
        {
            dockerHelper.Pull(imageName);
            _pulledImages.Add(imageName);
        }
        else
        {
            Assert.True(DockerHelper.ImageExists(imageName), $"`{imageName}` could not be found on disk.");
        }
    }

    public static string GetRepoNameModifier() => $"{(Config.IsNightlyRepo ? "/nightly" : string.Empty)}";

    public static string GetImageName(string tag, string repoName, string repoNameModifier = null)
    {
        string repo = GetRepoName(repoName, repoNameModifier);
        string registry = GetRegistryName(repo, tag);

        return $"{registry}{repo}:{tag}";
    }

    public static string GetRepoName(string repoName, string repoNameModifier = null) =>
        repoName == "aspire-dashboard"
            ? $"aspire{repoNameModifier ?? GetRepoNameModifier()}/dashboard"
            : $"dotnet{repoNameModifier ?? GetRepoNameModifier()}/{repoName}";

    protected string GetTagName(string tagPrefix, string os, string tagPostfix = null)
    {
        IEnumerable<string> tagParts = [tagPrefix, os, tagPostfix, GetArchTagSuffix()];
        tagParts = tagParts.Where(s => !string.IsNullOrEmpty(s));
        return string.Join('-', tagParts);
    }

    protected virtual string GetArchTagSuffix() => (Arch == Arch.Amd64 && IsWindows)
        ? string.Empty
        : GetArchLabel();

    protected string GetArchLabel() =>
        Arch switch
        {
            Arch.Amd64 => "amd64",
            Arch.Arm => "arm32v7",
            Arch.Arm64 => "arm64v8",
            _ => throw new NotSupportedException($"Unsupported architecture '{Arch}'")
        };

    private static string GetRegistryName(string repo, string tag)
    {
        bool imageExistsInStaging = true;

        // In the case of running this in a local development environment, there would likely be no image info file
        // provided. In that case, the assumption is that the images exist in the staging location.

        if (ImageInfoData.Value != null)
        {
            JObject repoInfo = (JObject)ImageInfoData.Value
                .Value<JArray>("repos")
                .FirstOrDefault(imageInfoRepo => imageInfoRepo["repo"].ToString() == repo);

            imageExistsInStaging = repoInfo?["images"] != null && repoInfo.Value<JArray>("images")
                    .SelectMany(imageInfo => imageInfo.Value<JArray>("platforms"))
                    .Cast<JObject>()
                    .Any(platformInfo => platformInfo.Value<JArray>("simpleTags")?.Any(imageTag => imageTag.ToString() == tag) == true);
        }

        return imageExistsInStaging ? $"{Config.Registry}/{Config.RepoPrefix}" : "mcr.microsoft.com/";
    }

    public override string ToString()
    {
        return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Select(propInfo => $"{propInfo.Name}='{propInfo.GetValue(this) ?? "<null>"}'")
            .Aggregate((working, next) => $"{working}, {next}");
    }
}
