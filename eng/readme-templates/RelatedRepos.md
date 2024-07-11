{{
    _ ARGS:
      top-header: The string to use as the top-level header.
      readme-host: Moniker of the site that will host the readme ^

    _ Common functions to help with repo rendering ^

    set isCurrentRepo(repo) to:{{
        set repoNameParts to split(repo[0], "/") ^
        set shortRepo to repoNameParts[len(repoNameParts) - 1] ^
        return shortRepo = SHORT_REPO
    }} ^

    set isNotCurrentRepo(repo) to:{{
        return not(isCurrentRepo(repo))
    }} ^

    set isNightlyRepo to match(split(REPO, "/")[1], "nightly") ^

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

    set getRepoBulletPoint(repo) to:{{
        set repoName to repo[0] ^
        set displayName to repo[1] ^
        set isProductFamilyRepo to repo[2] ^
        set url to InsertTemplate("Url.md", [ "readme-host": ARGS["readme-host"], "repo": repoName, "is-product-family": isProductFamilyRepo ]) ^
        return join(["* [", repoName, "](", url, "): ", displayName])
    }} ^

    set filterMonitorRepo(repo) to:{{
        return when(SHORT_REPO != "monitor", find(repo[0], "base") < 0, 1)
    }} ^

    set monitorRepo to when(SHORT_REPO = "monitor",
            ["dotnet/monitor/base", ".NET Monitor Base"],
            ["dotnet/monitor", ".NET Monitor Tool"]) ^

    _ Lists of repos to render
      Format: [repoName, displayName, isProductFamilyRepo] ^

    set repos to [
        ["dotnet/sdk", ".NET SDK"],
        ["dotnet/aspnet", "ASP.NET Core Runtime"],
        ["dotnet/runtime", ".NET Runtime"],
        ["dotnet/runtime-deps", ".NET Runtime Dependencies"],
        ["dotnet/monitor", ".NET Monitor Tool"],
        ["dotnet/monitor/base", ".NET Monitor Base"],
        ["dotnet/aspire-dashboard", ".NET Aspire Dashboard"]
    ] ^
    set commonRepos to [
        ["dotnet", ".NET", 1],
        ["dotnet/samples", ".NET Samples"]
    ] ^
    set frameworkRepos to [
        ["dotnet/framework", ".NET Framework, ASP.NET and WCF", 1],
        ["dotnet/framework/samples", ".NET Framework, ASP.NET and WCF Samples"]
    ] ^

    set currentRepo to cat(filter(repos, isCurrentRepo)) ^
    set commonRepos to filter(commonRepos, isNotCurrentRepo) ^

    _ Exclude monitor/base from repos besides monitor ^
    set repos to filter(repos, filterMonitorRepo) ^
    _ Exclude this repo from its own readme ^
    set repos to filter(repos, isNotCurrentRepo) ^

    set repos to when(
        (IS_PRODUCT_FAMILY && !isNightlyRepo) || !IS_PRODUCT_FAMILY,
        map(repos, insertNightly), repos) ^

    _ For non-nightly product repos, show the nightly version
    set repos to when(!isNightlyRepo && !IS_PRODUCT_FAMILY,
        cat(repos, map(currentRepo, insertNightly)),
        repos) ^

    set repos to when(not(IS_PRODUCT_FAMILY), cat(commonRepos, repos), repos)

}}{{ARGS["top-header"]}} Related Repositories

.NET:
{{for r in repos:
{{getRepoBulletPoint(r)}}}}

.NET Framework:
{{for i, r in frameworkRepos:
{{getRepoBulletPoint(r)}}}}