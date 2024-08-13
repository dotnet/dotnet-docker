{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readmes
      product-repos: List of .NET product repos
      product-family-repos: List of .NET product family repos
      samples-repos: List of .NET samples repos
      framework-repos: List of .NET Framework repos ^

    set isNightlyRepo to VARIABLES["branch"] = "nightly" ^

    set insertNightlyRepoName(repoName) to:{{
        return token(repoName, "/", 0, "dotnet/nightly")
    }} ^

    set insertNightlyRepoDisplayName(displayName) to:{{
        return join([displayName, "(Preview)"], " ")
    }} ^

    set insertNightly(repo) to:{{
        return [
            insertNightlyRepoName(repo[0]),
            insertNightlyRepoDisplayName(repo[1])
        ]
    }} ^

    set filterMonitorRepo(repo) to:{{
        return when(SHORT_REPO != "monitor", find(repo[0], "base") < 0, 1)
    }} ^

    set repos to filter(ARGS["product-repos"], filterMonitorRepo) ^
    set repos to when(isNightlyRepo, map(repos, insertNightly), repos) ^
    set repos to when(!IS_PRODUCT_FAMILY || !isNightlyRepo, cat(repos, ARGS["samples-repos"]), repos)

}}{{ARGS["top-header"]}} Featured Repos
{{InsertTemplate("RepoList.md", [ "readme-host": ARGS["readme-host"], "repos": repos ])}}