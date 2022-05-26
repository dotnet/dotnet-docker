{{
    _ Generates a URL formatted for the site that will host the readme
    ARGS:
      readme-host: Moniker of the site that will host the readme
      repo: Repo path of the URL to be generated
      is-product-family: Indicates whether the URL refers to a product family page
}}{{
when(ARGS["readme-host"] = "mar",
    when(ARGS["is-product-family"],
        cat("https://mcr.microsoft.com/catalog?search=", ARGS["repo"]),
        cat("https://mcr.microsoft.com/product/", ARGS["repo"], "/about")),
    when(ARGS["readme-host"] = "dockerhub",
        cat("https://hub.docker.com/_/microsoft-", join(split(ARGS["repo"], "/"), "-"), "/"),
        cat("UNKNOWN HOST: ", ARGS["readme-host"])))}}
