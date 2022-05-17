{{
    _ ARGS:
      leading-line-break: Indicates whether to include a leading line break
      trailing-line-break: Indicates whether to include a trailing line break
}}{{if match(PARENT_REPO, "nightly") || VARIABLES["branch"] = "nightly"
:{{if ARGS["leading-line-break"]:
}}**IMPORTANT**
**The images from the dotnet/{{if IS_PRODUCT_FAMILY:nightly^else:{{PARENT_REPO}}}} repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).**

**See [dotnet](https://hub.docker.com/_/microsoft-dotnet/) for images with official releases of [.NET](https://github.com/dotnet/core).**
{{if ARGS["trailing-line-break"]:
}}}}
