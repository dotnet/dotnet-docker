{{
    _ ARGS:
      readme-host: Moniker of the site that will host the readme
      repos: List of repos to render, in the format [repoName, displayName, isProductFamilyRepo] ^

    set getRepoBulletPoint(repo) to:{{
        set repoName to repo[0] ^
        set displayName to repo[1] ^
        set isProductFamilyRepo to repo[2] ^
        set url to InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": repoName, "is-product-family": isProductFamilyRepo ]) ^
        return join(["* [", repoName, "](", url, "): ", displayName])
    }}

}}{{ARGS["top-header"]}}{{for r in ARGS["repos"]:
{{getRepoBulletPoint(r)}}}}