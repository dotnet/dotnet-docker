{{
    _ ARGS:
      product-repos: List of .NET product repos
      product-family-repos: List of .NET product family repos
      samples-repos: List of .NET samples repos ^

    set repos to cat(ARGS["product-repos"], ARGS["samples-repos"]) ^

    set isCurrentRepo(repo) to:{{
        set repoNameParts to split(repo[0], "/") ^
        set shortRepo to repoNameParts[len(repoNameParts) - 1] ^
        return shortRepo = SHORT_REPO
    }} ^

    set currentRepo to when(IS_PRODUCT_FAMILY, ARGS["product-family-repos"], cat(filter(repos, isCurrentRepo)))

}}# {{currentRepo[0][1]}}
