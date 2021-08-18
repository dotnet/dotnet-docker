{{if match(PARENT_REPO, "nightly") || VARIABLES["branch"] = "nightly"
:The images from the dotnet/{{if IS_PRODUCT_FAMILY:nightly^else:{{PARENT_REPO}}}} repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).

See [dotnet](https://hub.docker.com/_/microsoft-dotnet/) for images with official releases of [.NET](https://github.com/dotnet/core).

}}{{if !IS_PRODUCT_FAMILY:# Featured Tags

{{if match(SHORT_REPO, "samples")
:* `dotnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:dotnetapp`
* `aspnetapp` [(*Dockerfile*)](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile)
  * `docker pull mcr.microsoft.com/dotnet/samples:aspnetapp`
^elif match(SHORT_REPO, "monitor"):* `5.0` (Preview)
  * `docker pull {{FULL_REPO}}:5.0`
^else:* `5.0` (Current)
  * `docker pull {{FULL_REPO}}:5.0`
* `3.1` (LTS)
  * `docker pull {{FULL_REPO}}:3.1`
}}}}{{if IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main"
:# Featured Repos

* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
* [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/): .NET Monitor Tool
* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
^elif IS_PRODUCT_FAMILY && VARIABLES["branch"] = "nightly"
:# Featured Repos

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

The [.NET Docker samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md) show various ways to use .NET and Docker together. See [Building Docker Images for .NET Applications](https://docs.microsoft.com/dotnet/core/docker/building-net-docker-images) to learn more.

{{InsertTemplate(join(filter(["Use", SHORT_REPO, "md"], len), "."))}}
# Related Repos

.NET:

{{if (!IS_PRODUCT_FAMILY || VARIABLES["branch"] = "nightly")
    :* [dotnet](https://hub.docker.com/_/microsoft-dotnet/): .NET
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "sdk")
    :* [dotnet/sdk](https://hub.docker.com/_/microsoft-dotnet-sdk/): .NET SDK
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "aspnet")
    :* [dotnet/aspnet](https://hub.docker.com/_/microsoft-dotnet-aspnet/): ASP.NET Core Runtime
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "runtime")
    :* [dotnet/runtime](https://hub.docker.com/_/microsoft-dotnet-runtime/): .NET Runtime
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "runtime-deps")
    :* [dotnet/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-runtime-deps/): .NET Runtime Dependencies
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "monitor")
    :* [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/): .NET Monitor Tool
}}{{if ((!IS_PRODUCT_FAMILY || VARIABLES["branch"] = "nightly") && SHORT_REPO != "samples")
    :* [dotnet/samples](https://hub.docker.com/_/microsoft-dotnet-samples/): .NET Samples
}}{{if ((PARENT_REPO = "nightly" && SHORT_REPO != "sdk") || (PARENT_REPO = "dotnet" && SHORT_REPO = "sdk") || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main"))
    :* [dotnet/nightly/sdk](https://hub.docker.com/_/microsoft-dotnet-nightly-sdk/): .NET SDK (Preview)
}}{{if ((PARENT_REPO = "nightly" && SHORT_REPO != "aspnet") || (PARENT_REPO = "dotnet" && SHORT_REPO = "aspnet") || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main"))
    :* [dotnet/nightly/aspnet](https://hub.docker.com/_/microsoft-dotnet-nightly-aspnet/): ASP.NET Core Runtime (Preview)
}}{{if ((PARENT_REPO = "nightly" && SHORT_REPO != "runtime") || (PARENT_REPO = "dotnet" && SHORT_REPO = "runtime") || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main"))
    :* [dotnet/nightly/runtime](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime/): .NET Runtime (Preview)
}}{{if ((PARENT_REPO = "nightly" && SHORT_REPO != "runtime-deps") || (PARENT_REPO = "dotnet" && SHORT_REPO = "runtime-deps") || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main"))
    :* [dotnet/nightly/runtime-deps](https://hub.docker.com/_/microsoft-dotnet-nightly-runtime-deps/): .NET Runtime Dependencies (Preview)
}}{{if ((PARENT_REPO = "nightly" && SHORT_REPO != "monitor") || (PARENT_REPO = "dotnet" && SHORT_REPO = "monitor") || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main"))
    :* [dotnet/nightly/monitor](https://hub.docker.com/_/microsoft-dotnet-nightly-monitor/): .NET Monitor Tool (Preview)
}}
.NET Framework:

* [dotnet/framework](https://hub.docker.com/_/microsoft-dotnet-framework/): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples](https://hub.docker.com/_/microsoft-dotnet-framework-samples/): .NET Framework, ASP.NET and WCF Samples

{{if !IS_PRODUCT_FAMILY:# Full Tag Listing
<!--End of generated tags-->

{{if SHORT_REPO != "monitor":For tags contained in the old dotnet/core{{if (PARENT_REPO = "nightly"):-nightly}}/{{SHORT_REPO}} repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core{{if (PARENT_REPO = "nightly"):-nightly}}/{{SHORT_REPO}}/tags/list.

}}}}# Support

See [Microsoft Support for .NET](https://github.com/dotnet/core/blob/master/microsoft-support.md) for the support lifecycle.

# Image Update Policy

* We update the supported .NET images within 12 hours of any updates to their base images (e.g. debian:buster-slim, windows/nanoserver:ltsc2022, buildpack-deps:bionic-scm, etc.).
* We publish .NET images as part of releasing new versions of .NET including major/minor and servicing.

# Feedback

* [File an issue](https://github.com/dotnet/dotnet-docker/issues/new/choose)
* [Contact Microsoft Support](https://support.microsoft.com/contactus/)

# License

* Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
* [.NET license](https://github.com/dotnet/dotnet-docker/blob/main/LICENSE)
* [Discover licensing for Linux image contents](https://github.com/dotnet/dotnet-docker/blob/main/documentation/image-artifact-details.md)
* [Windows base image license](https://docs.microsoft.com/virtualization/windowscontainers/images-eula) (only applies to Windows containers)
* [Pricing and licensing for Windows Server 2019](https://www.microsoft.com/cloud-platform/windows-server-pricing)
