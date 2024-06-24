{{if IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main":# Featured Repos

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
