{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme
}}{{ARGS["top-header"]}} Related Repos

.NET:

{{if (!IS_PRODUCT_FAMILY || VARIABLES["branch"] = "nightly") && ARGS["readme-host"] = "dockerhub"
    :* [dotnet]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet", "is-product-family": "true" ])}}): .NET
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "sdk")
    :* [dotnet/sdk]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/sdk" ])}}): .NET SDK
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "aspnet")
    :* [dotnet/aspnet]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/aspnet" ])}}): ASP.NET Core Runtime
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "runtime")
    :* [dotnet/runtime]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/runtime" ])}}): .NET Runtime
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "runtime-deps")
    :* [dotnet/runtime-deps]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/runtime-deps" ])}}): .NET Runtime Dependencies
}}{{if (PARENT_REPO = "dotnet" && SHORT_REPO != "monitor")
    :* [dotnet/monitor]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/monitor" ])}}): .NET Monitor Tool
}}{{if ((!IS_PRODUCT_FAMILY || VARIABLES["branch"] = "nightly") && SHORT_REPO != "samples")
    :* [dotnet/samples]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/samples" ])}}): .NET Samples
}}{{if ((PARENT_REPO = "nightly" && SHORT_REPO != "sdk") || (PARENT_REPO = "dotnet" && SHORT_REPO = "sdk") || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main"))
    :* [dotnet/nightly/sdk]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/nightly/sdk" ])}}): .NET SDK (Preview)
}}{{if ((PARENT_REPO = "nightly" && SHORT_REPO != "aspnet") || (PARENT_REPO = "dotnet" && SHORT_REPO = "aspnet") || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main"))
    :* [dotnet/nightly/aspnet]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/nightly/aspnet" ])}}): ASP.NET Core Runtime (Preview)
}}{{if ((PARENT_REPO = "nightly" && SHORT_REPO != "runtime") || (PARENT_REPO = "dotnet" && SHORT_REPO = "runtime") || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main"))
    :* [dotnet/nightly/runtime]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/nightly/runtime" ])}}): .NET Runtime (Preview)
}}{{if ((PARENT_REPO = "nightly" && SHORT_REPO != "runtime-deps") || (PARENT_REPO = "dotnet" && SHORT_REPO = "runtime-deps") || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main"))
    :* [dotnet/nightly/runtime-deps]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/nightly/runtime-deps" ])}}): .NET Runtime Dependencies (Preview)
}}{{if ((PARENT_REPO = "nightly" && SHORT_REPO != "monitor") || (PARENT_REPO = "dotnet" && SHORT_REPO = "monitor") || (IS_PRODUCT_FAMILY && VARIABLES["branch"] = "main"))
    :* [dotnet/nightly/monitor]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/nightly/monitor" ])}}): .NET Monitor Tool (Preview)
}}
.NET Framework:

* [dotnet/framework]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/framework", "is-product-family": "true" ])}}): .NET Framework, ASP.NET and WCF
* [dotnet/framework/samples]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": "dotnet/framework/samples" ])}}): .NET Framework, ASP.NET and WCF Samples
