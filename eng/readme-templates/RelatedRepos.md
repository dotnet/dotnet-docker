{{
    _ ARGS:
      top-header: The string to use as the top-level header.
}}{{ARGS["top-header"]}} Related Repos

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
