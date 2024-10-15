{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readmes
      product-repos: List of .NET product repos
      product-family-repos: List of .NET product family repos
      samples-repos: List of .NET samples repos
      framework-repos: List of .NET Framework repos ^

    set repos to ARGS["product-repos"] ^
    set productFamilyRepos to ARGS["product-family-repos"] ^
    set samplesRepos to ARGS["samples-repos"] ^
    set frameworkRepos to ARGS["framework-repos"] ^

    _ Common functions to help with repo rendering ^

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

    _ Create final set of repos to display ^

    set currentRepo to cat(filter(repos, isCurrentRepo)) ^

    _ Exclude monitor/base from repos besides monitor ^
    set repos to filter(repos, filterMonitorRepo) ^

    _ Exclude this repo from its own readme ^
    set repos to filter(repos, isNotCurrentRepo) ^
    set samplesRepos to filter(samplesRepos, isNotCurrentRepo) ^

    set repos to
        when(isNightlyRepo,
            when(IS_PRODUCT_FAMILY,
                cat(productFamilyRepos, repos, samplesRepos),
                cat(productFamilyRepos, currentRepo, map(repos, insertNightly), samplesRepos)),
            when(IS_PRODUCT_FAMILY,
                cat(map(repos, insertNightly)),
                cat(productFamilyRepos, repos, map(currentRepo, insertNightly), samplesRepos)))

}}{{ARGS["top-header"]}} Related Repositories

.NET:
{{InsertTemplate("RepoList.md", [ "readme-host": ARGS["readme-host"], "repos": repos ])}}

.NET Framework:
{{InsertTemplate("RepoList.md", [ "readme-host": ARGS["readme-host"], "repos": frameworkRepos ])}}