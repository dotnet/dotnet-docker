{{
    _ ARGS:
      nightly-only-repos: List of nightly-only .NET product repos
      product-family-repos: List of .NET product family repos
      leading-line-break: Indicates whether to include a leading line break
      trailing-line-break: Indicates whether to include a trailing line break
      readme-host: Moniker of the site that will host the readme ^

    set getRepoName(repo) to:{{
        return repo[0]
    }} ^

    set nonNightlyRepo to when(IS_PRODUCT_FAMILY, "dotnet", join(split(REPO, "/nightly"), "")) ^
    set announcementFamily to split(nonNightlyRepo, "/")[0] ^
    set isNightlyRepo to match(split(REPO, "/")[1], "nightly") ^
    set productFamilyRepo to ARGS["product-family-repos"][0][0] ^
    set isNightlyOnly to find(map(ARGS["nightly-only-repos"],getRepoName), nonNightlyRepo) >= 0 ^

    set url to when(isNightlyOnly,
        InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": productFamilyRepo, "is-product-family": 1 ]),
        InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": nonNightlyRepo, "is-product-family": IS_PRODUCT_FAMILY ]))

}}{{if isNightlyRepo || VARIABLES["branch"] = "nightly"
:{{if ARGS["leading-line-break"]:
}}{{if announcementFamily = "aspire":> **Important**: The {{REPO}} image is a preview build of the Aspire Dashboard and is not signed. See [{{nonNightlyRepo}}]({{url}}) for stable releases.
^else:> **Important**: The images from the {{announcementFamily}}/nightly repositories include last-known-good (LKG) builds for the next release of [.NET](https://github.com/dotnet/core).
>
> See [{{announcementFamily}}]({{url}}) for images with official releases of [.NET](https://github.com/dotnet/core).
}}{{if ARGS["trailing-line-break"]:
}}}}
