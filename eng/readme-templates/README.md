{{if match(PARENT_REPO, "nightly") || VARIABLES["branch"] = "nightly"
:The images from the dotnet/{{if IS_PRODUCT_FAMILY:nightly^else:{{PARENT_REPO}}}} repositories include last-known-good (LKG) builds for the next release of [.NET Core](https://github.com/dotnet/core).

See [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/) for images with official releases of [.NET Core](https://github.com/dotnet/core).

}}{{if match(PARENT_REPO, "nightly") || PARENT_REPO = "dotnet"
:As part of the .NET 5.0 release the Docker images are published to different repositories.  The 2.1 and 3.1 images are published to [core branded repositories](https://hub.docker.com/_/microsoft-dotnet-core/) while the 5.0 and higher versions will be published to [non-core branded repositories](https://hub.docker.com/_/microsoft-dotnet/).  See the [related issue](https://github.com/dotnet/dotnet-docker/issues/1765) for more details.

}}{{if !IS_PRODUCT_FAMILY:# Featured Tags

{{if match(SHORT_REPO, "samples")
:* `dotnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/dotnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/core/samples:dotnetapp`
* `aspnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/core/samples:aspnetapp`
^elif match(PARENT_REPO, "core"):* `3.1` (LTS/Current)
  * `docker pull {{FULL_REPO}}:3.1`
^else:* `5.0` (Preview)
  * `docker pull {{FULL_REPO}}:5.0`
}}}}{{if IS_PRODUCT_FAMILY && VARIABLES["branch"] = "master"
:# Featured Repos

## .NET Core 2.1/3.1

* [dotnet/core/sdk](https://hub.docker.com/_/microsoft-dotnet-core-sdk/): .NET Core SDK
* [dotnet/core/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/): ASP.NET Core Runtime
* [dotnet/core/runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/): .NET Core Runtime
* [dotnet/core/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-runtime-deps/): .NET Core Runtime Dependencies
* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples

## .NET 5.0+

* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
^elif IS_PRODUCT_FAMILY && VARIABLES["branch"] = "nightly"
:# Featured Repos

## .NET Core 2.1/3.1

* [dotnet/core-nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-core-nightly-sdk/): .NET Core SDK (Preview)
* [dotnet/core-nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/core-nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime/): .NET Core Runtime (Preview)
* [dotnet/core-nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime-deps/): .NET Core Runtime Dependencies (Preview)

## .NET 5.0+

* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)
* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime/): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor](https://hub.docker.com/_/microsoft-dotnet-nightly-monitor/): .NET Monitor Tool (Preview)
}}
# About {{if IS_PRODUCT_FAMILY:.NET^else:This Image}}

{{InsertTemplate(join(filter(["About", SHORT_REPO, "md"], len), "."))}}
Watch [dotnet/announcements](https://github.com/dotnet/announcements/labels/Docker) for Docker-related .NET announcements.

# How to Use the Image{{if IS_PRODUCT_FAMILY:s}}

The [.NET Core Docker samples](https://github.com/dotnet/dotnet-docker/blob/master/samples/README.md) show various ways to use .NET Core and Docker together. See [Building Docker Images for .NET Core Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

{{InsertTemplate(join(filter(["Use", SHORT_REPO, "md"], len), "."))}}
# Related Repos

.NET Core 2.1/3.1:

{{if !IS_PRODUCT_FAMILY || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "nightly")
    :* [dotnet/core](https://hub.docker.com/_/microsoft-dotnet-core/): .NET Core
}}{{if (PARENT_REPO = "core" && SHORT_REPO != "sdk") || (PARENT_REPO = "dotnet" && SHORT_REPO = "sdk")
    :* [dotnet/core/sdk](https://hub.docker.com/_/microsoft-dotnet-core-sdk/): .NET Core SDK
}}{{if (PARENT_REPO = "core" && SHORT_REPO != "aspnet") || (PARENT_REPO = "dotnet" && SHORT_REPO = "aspnet")
    :* [dotnet/core/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/): ASP.NET Core Runtime
}}{{if (PARENT_REPO = "core" && SHORT_REPO != "runtime") || (PARENT_REPO = "dotnet" && SHORT_REPO = "runtime")
    :* [dotnet/core/runtime](https://hub.docker.com/_/microsoft-dotnet-core-runtime/): .NET Core Runtime
}}{{if (PARENT_REPO = "core" && SHORT_REPO != "runtime-deps") || (PARENT_REPO = "dotnet" && SHORT_REPO = "runtime-deps")
    :* [dotnet/core/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-runtime-deps/): .NET Core Runtime Dependencies
}}{{if REPO != "dotnet/core/samples"
    :* [dotnet/core/samples](https://hub.docker.com/_/microsoft-dotnet-core-samples/): .NET Core Samples
}}{{if !IS_PRODUCT_FAMILY || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "master")
    :* [dotnet/core-nightly](https://hub.docker.com/_/microsoft-dotnet-core-nightly/): .NET Core (Preview)
}}{{if (PARENT_REPO = "core-nightly" && SHORT_REPO != "sdk") || (PARENT_REPO = "nightly" && SHORT_REPO = "sdk")
    :* [dotnet/core-nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-core-nightly-sdk/): .NET Core SDK (Preview)
}}{{if (PARENT_REPO = "core-nightly" && SHORT_REPO != "aspnet") || (PARENT_REPO = "nightly" && SHORT_REPO = "aspnet")
    :* [dotnet/core-nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-core-nightly-aspnet/): ASP.NET Core Runtime (Preview)
}}{{if (PARENT_REPO = "core-nightly" && SHORT_REPO != "runtime") || (PARENT_REPO = "nightly" && SHORT_REPO = "runtime")
    :* [dotnet/core-nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime/): .NET Core Runtime (Preview)
}}{{if (PARENT_REPO = "core-nightly" && SHORT_REPO != "runtime-deps") || (PARENT_REPO = "nightly" && SHORT_REPO = "runtime-deps")
    :* [dotnet/core-nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-core-nightly-runtime-deps/): .NET Core Runtime Dependencies (Preview)
}}
.NET 5.0+:

{{if !IS_PRODUCT_FAMILY || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "nightly")
    :* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "sdk") || (PARENT_REPO = "core" && SHORT_REPO = "sdk")
    :* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "aspnet") || (PARENT_REPO = "core" && SHORT_REPO = "aspnet")
    :* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "runtime") || (PARENT_REPO = "core" && SHORT_REPO = "runtime")
    :* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "runtime-deps") || (PARENT_REPO = "core" && SHORT_REPO = "runtime-deps")
    :* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
}}{{if !IS_PRODUCT_FAMILY || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "master")
    :* [dotnet/nightly](https://hub.docker.com/_/microsoft-dotnet-nightly/): .NET (Preview)
}}{{if (PARENT_REPO = "nightly" && SHORT_REPO != "sdk") || (PARENT_REPO = "core-nightly" && SHORT_REPO = "sdk")
    :* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)
}}{{if (PARENT_REPO = "nightly" && SHORT_REPO != "aspnet") || (PARENT_REPO = "core-nightly" && SHORT_REPO = "aspnet")
    :* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
}}{{if (PARENT_REPO = "nightly" && SHORT_REPO != "runtime") || (PARENT_REPO = "core-nightly" && SHORT_REPO = "runtime")
    :* [dotnet/nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime/): .NET Runtime (Preview)
}}{{if (PARENT_REPO = "nightly" && SHORT_REPO != "runtime-deps") || (PARENT_REPO = "core-nightly" && SHORT_REPO = "runtime-deps")
    :* [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)
}}{{if PARENT_REPO = "nightly" && SHORT_REPO != "monitor"
    :* [dotnet/nightly/monitor](https://hub.docker.com/_/microsoft-dotnet-nightly-monitor/): .NET Monitor Tool (Preview)
}}
.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

{{if !IS_PRODUCT_FAMILY:# Full Tag Listing

}}# Support

See [Microsoft Support for .NET Core](https://github.com/dotnet/core/blob/master/microsoft-support.md) for the support lifecycle.

# Image Update Policy

* We update the supported .NET Core images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:1909, buildpack-deps:bionic-scm, etc.).
* We publish .NET Core images as part of releasing new versions of .NET Core including major/minor and servicing.

# Feedback

* [File a .NET Core Docker issue](https://github.com/dotnet/dotnet-docker/issues)
* [File a .NET Core issue](https://github.com/dotnet/core/issues)
* [File an ASP.NET Core issue](https://github.com/aspnet/home/issues)
* [File an issue for other .NET components](https://github.com/dotnet/core/blob/master/Documentation/core-repos.md)
* [File a Visual Studio Docker Tools issue](https://github.com/microsoft/dockertools/issues)
* [File a Microsoft Container Registry (MCR) issue](https://github.com/microsoft/containerregistry/issues)
* [Ask on Stack Overflow](https://stackoverflow.com/questions/tagged/.net-core)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET Core license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/master/documentation/image-artifact-details.md)
* [Windows Nano Server license](https://hub.docker.com/_/microsoft-windows-nanoserver/) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
