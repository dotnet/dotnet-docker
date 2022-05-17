{{
  set headerArgs to [ "top-header": "#" ]
}}{{InsertTemplate("Announcement.md")}}{{
if !IS_PRODUCT_FAMILY:{{InsertTemplate("FeaturedTags.md", headerArgs)}}
}}{{if IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main":# Featured Repos

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
{{InsertTemplate("About.md", headerArgs)}}

{{InsertTemplate("Use.md", headerArgs)}}

{{InsertTemplate("RelatedRepos.md", headerArgs)}}
{{if !IS_PRODUCT_FAMILY:
# Full Tag Listing
{{if SHORT_REPO != "monitor":For tags contained in the old dotnet/core{{if (PARENT_REPO = "nightly"):-nightly}}/{{SHORT_REPO}} repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core{{if (PARENT_REPO = "nightly"):-nightly}}/{{SHORT_REPO}}/tags/list.

}}*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)*
}}
{{InsertTemplate("Support.md", headerArgs)}}
