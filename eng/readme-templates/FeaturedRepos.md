{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readmes
      repos: List of normal .NET product repos
      common-repos: List of other non-product repos (e.g. samples)
      framework-repos: List of .NET Framework repos ^

    set isCurrentRepo(repo) to:{{
        set repoNameParts to split(repo[0], "/") ^
        set shortRepo to repoNameParts[len(repoNameParts) - 1] ^
        return shortRepo = SHORT_REPO
    }} ^

    set isNotCurrentRepo(repo) to:{{
        return not(isCurrentRepo(repo))
    }} ^

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

    set repos to cat(ARGS["repos"], ARGS["common-repos"]) ^
    set repos to filter(repos, isNotCurrentRepo) ^
    set repos to filter(repos, filterMonitorRepo) ^
    set repos to when(isNightlyRepo, map(repos, insertNightly), repos)

}}{{ARGS["top-header"]}} Featured Repos
{{InsertTemplate("RepoList.md", [ "readme-host": ARGS["readme-host"], "repos": repos ])}}