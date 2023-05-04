{{
    _ ARGS:
      leading-line-break: Indicates whether to include a leading line break
      trailing-line-break: Indicates whether to include a trailing line break
      readme-host: Moniker of the site that will host the readme ^
    set nonNightlyRepo to when(IS_PRODUCT_FAMILY, "dotnet", join(split(REPO, "/nightly"), "")) ^
    set isNightlyRepo to match(split(REPO, "/")[1], "nightly")
}}{{if isNightlyRepo || VARIABLES["branch"] = "nightly"
:{{if ARGS["leading-line-break"]:
}}**IMPORTANT**

**The images from the dotnet/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).**

**See [dotnet]({{InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": nonNightlyRepo ])}}) for images with official releases of [.NET](https://github.com/dotnet/core).**
{{if ARGS["trailing-line-break"]:
}}}}
