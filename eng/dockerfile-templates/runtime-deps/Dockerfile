{{
    _ ARGS:
        is-extra (optional): Whether to include extra packages that are not needed for every app,
            for example globalization support ^

    set dotnetVersion to join(slice(split(PRODUCT_VERSION, "."), 0, 2), ".") ^
    set isAlpine to find(OS_ARCH_HYPHENATED, "Alpine") >= 0 ^
    set isDebian to find(OS_ARCH_HYPHENATED, "Debian") >= 0 ^
    set isUbuntu to find(OS_ARCH_HYPHENATED, "Ubuntu") >= 0 ^
    set isAzureLinux to defined(match(OS_VERSION, "^azurelinux\d+\.\d+$")) ^

    set baseImageRepo to when(isAlpine,
        cat(ARCH_VERSIONED, "/alpine"),
        when(isDebian,
            cat(ARCH_VERSIONED, "/debian"),
            when(isUbuntu,
                "ubuntu.azurecr.io/ubuntu",
                when(isAzureLinux,
                    "mcr.microsoft.com/azurelinux/base/core",
                    "<NOT-IMPLEMENTED>")))) ^
    set baseImageTag to when(isAlpine || isAzureLinux, OS_VERSION_NUMBER, OS_VERSION) ^
    set baseImage to cat(baseImageRepo, ":", baseImageTag) ^

    set useGlobalizationInvariantMode to !ARGS["is-extra"] && (isDistroless || isAlpine) ^
    set username to "app" ^
    set uid to 1654 ^
    set nonRootUserComment to "# Create a non-root user and group"

}}FROM {{baseImage}}

{{InsertTemplate("../Dockerfile.common-dotnet-envs",
    [
        "uid": uid,
        "globalization-invariant": useGlobalizationInvariantMode
    ]
)}}

RUN {{InsertTemplate("../Dockerfile.linux.install-deps", [
    "is-extra": ARGS["is-extra"]
])}}

{{nonRootUserComment}}
RUN {{InsertTemplate("Dockerfile.linux.non-root-user", [ "name": username ])}}
