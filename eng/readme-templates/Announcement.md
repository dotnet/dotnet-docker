{{
    _ ARGS:
      leading-line-break: Indicates whether to include a leading line break
      trailing-line-break: Indicates whether to include a trailing line break
      readme-host: Moniker of the site that will host the readme ^
    set nonNightlyRepo to join(split(REPO, "/nightly"), "")
}}{{if match(PARENT_REPO, "nightly") || VARIABLES["branch"] = "nightly"
:{{if ARGS["leading-line-break"]:
}}**IMPORTANT**

**The images from the dotnet/{{if IS_PRODUCT_FAMILY:nightly^else:{{PARENT_REPO}}}} repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).**

**See [dotnet]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": nonNightlyRepo ])}}) for images with official releases of [.NET](https://github.com/dotnet/core).**
{{if ARGS["trailing-line-break"]:
}}}}
