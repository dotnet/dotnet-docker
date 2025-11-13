{{
    _ Generates a URL formatted for the site that will host the readme
    ARGS:
      readme-host: Moniker of the site that will host the readme
      repo: Repo path of the URL to be generated
      is-product-family: Indicates whether the URL refers to a product family page
      use-relative-link: Indicates whether the URL should be relative or absolute ^

    set isProductFamily to ARGS["is-product-family"] ^
    set useRelativeLink to ARGS["use-relative-link"] ^
    set readmeHost to ARGS["readme-host"] ^
    set repo to ARGS["repo"] ^
    set repoParts to split(repo, "/") ^
    set isNightlyRepo to match(repoParts[1], "nightly") ^
    set isFrameworkRepo to match(repoParts[1], "framework") ^
    set readmeFileName to cat(
        "README.",
        when(isProductFamily, "", cat(join(slice(repoParts, when(isNightlyRepo || isFrameworkRepo, 2, 1)), "-"), ".")),
        "md")

}}{{
when(readmeHost = "mar",
    when(isProductFamily,
        cat("https://mcr.microsoft.com/catalog?search=", repo),
        cat("https://mcr.microsoft.com/artifact/mar/", repo, "/about")),
    when(readmeHost = "github",
        when(useRelativeLink,
            cat("./", readmeFileName),
            cat("https://github.com/",
                when(isFrameworkRepo, "microsoft", "dotnet"),
                "/dotnet",
                when(isFrameworkRepo, "-framework", ""),
                "-docker/blob/",
                when(isNightlyRepo, "nightly", "main"),
                "/",
                readmeFileName)),
        when(readmeHost = "dockerhub",
            cat("https://hub.docker.com/r/microsoft/", join(repoParts, "-"), "/"),
            cat("UNKNOWN HOST: ", readmeHost))))}}
