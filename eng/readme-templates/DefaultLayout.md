{{
  set commonArgs to [
    "top-header": "#"
    "readme-host": ARGS["readme-host"]
  ] ^
  set isNightlyRepo to match(split(REPO, "/")[1], "nightly") ^
  set isMonitor to find(split(REPO, "/"), "monitor") >= 0 ^
  set isAspireDashboard to find(split(REPO, "/"), "aspire-dashboard") >= 0
}}{{InsertTemplate("Announcement.md", union(commonArgs, [ "trailing-line-break": "true" ]))}}{{
if !IS_PRODUCT_FAMILY:{{InsertTemplate("FeaturedTags.md", commonArgs)}}
}}{{if IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main":# Featured Repos

* [dotnet/sdk]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/sdk" ])}}): .NET SDK
* [dotnet/aspnet]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/aspnet" ])}}): ASP.NET Core Runtime
* [dotnet/runtime]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/runtime" ])}}): .NET Runtime
* [dotnet/runtime-deps]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/runtime-deps" ])}}): .NET Runtime Dependencies
* [dotnet/monitor]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/monitor" ])}}): .NET Monitor Tool
* [dotnet/samples]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/samples" ])}}): .NET Samples
^elif IS_PRODUCT_FAMILY && VARIABLES["branch"] = "nightly"
:# Featured Repos

* [dotnet/nightly/sdk]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/nightly/sdk" ])}}): .NET SDK (Preview)
* [dotnet/nightly/aspnet]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/nightly/aspnet" ])}}): ASP.NET Core Runtime (Preview)
* [dotnet/nightly/runtime]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/nightly/runtime" ])}}): .NET Runtime (Preview)
* [dotnet/nightly/runtime-deps]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/nightly/runtime-deps" ])}}): .NET Runtime Dependencies (Preview)
* [dotnet/nightly/monitor]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/nightly/monitor" ])}}): .NET Monitor Tool (Preview)
* [dotnet/nightly/aspire-dashboard]({{InsertTemplate("Url.md", [ "readme-host": "github", "repo": "dotnet/nightly/aspire-dashboard" ])}}): .NET Aspire Dashboard (Preview)
}}
{{InsertTemplate("About.md", commonArgs)}}

{{InsertTemplate("Use.md", commonArgs)}}{{if (find(REPO, "monitor") < 0 && find(REPO, "aspire") < 0):

{{InsertTemplate("About.variants.md", commonArgs)}}}}

{{InsertTemplate("RelatedRepos.md", commonArgs)}}
{{if !IS_PRODUCT_FAMILY:
# Full Tag Listing
{{if ARGS["readme-host"] = "github":<!--End of generated tags-->
{{if !(isMonitor || isAspireDashboard):For tags contained in the old dotnet/core{{if isNightlyRepo:-nightly}}/{{SHORT_REPO}} repository, you can retrieve a list of those tags at https://mcr.microsoft.com/v2/dotnet/core{{if isNightlyRepo:-nightly}}/{{SHORT_REPO}}/tags/list.

}}*Tags not listed in the table above are not supported. See the [Supported Tags Policy](https://github.com/dotnet/dotnet-docker/blob/main/documentation/supported-tags.md)*
^elif ARGS["readme-host"] = "dockerhub":
View the current tags at the [Microsoft Artifact Registry portal](https://mcr.microsoft.com/product/{{REPO}}/tags) or on [GitHub](https://github.com/dotnet/dotnet-docker/blob/{{if isNightlyRepo:nightly^else:main}}/README.{{SHORT_REPO}}.md#full-tag-listing).
}}}}
{{InsertTemplate("Support.md", commonArgs)}}
