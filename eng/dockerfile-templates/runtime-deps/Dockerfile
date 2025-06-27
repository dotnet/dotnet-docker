{{
    _ ARGS:
        is-extra (optional): Whether to include extra packages that are not needed for every app,
            for example globalization support ^

    set dotnetVersion to join(slice(split(PRODUCT_VERSION, "."), 0, 2), ".") ^
    set isAlpine to find(OS_ARCH_HYPHENATED, "Alpine") >= 0 ^
    set isDebian to find(OS_ARCH_HYPHENATED, "Debian") >= 0 ^
    set isUbuntu to find(OS_ARCH_HYPHENATED, "Ubuntu") >= 0 ^
    set isCblMariner to defined(match(OS_VERSION, "^cbl-mariner\d+\.\d+$")) ^
    set isAzureLinux to isCblMariner || defined(match(OS_VERSION, "^azurelinux\d+\.\d+$")) ^

    set baseImageRepo to when(isAlpine,
        cat(ARCH_VERSIONED, "/alpine"),
        when(isDebian,
            cat(ARCH_VERSIONED, "/debian"),
            when(isUbuntu,
                "ubuntu.azurecr.io/ubuntu",
                when(isAzureLinux,
                    when(find(OS_VERSION_NUMBER, "3.0") >= 0,
                        "mcr.microsoft.com/azurelinux/base/core",
                        "mcr.microsoft.com/cbl-mariner/base/core"),
                    "<NOT-IMPLEMENTED>")))) ^
    set baseImageTag to when(isAlpine || isAzureLinux, OS_VERSION_NUMBER, OS_VERSION) ^
    set baseImage to cat(baseImageRepo, ":", baseImageTag) ^

    set isMultiStage to isCblMariner ^
    set firstStageName to "base" ^
    set secondStageName to when(isMultiStage, "installer") ^

    set stagingDir to "/staging" ^

    set utilPkgs to when(isAzureLinux, ["shadow-utils"], []) ^
    set useGlobalizationInvariantMode to !ARGS["is-extra"] && (isDistroless || isAlpine) ^
    set username to "app" ^
    set uid to 1654 ^
    set gid to uid ^
    set nonRootUserComment to "# Create a non-root user and group"

}}FROM {{baseImage}}{{if isMultiStage: AS base}}

{{InsertTemplate("../Dockerfile.common-dotnet-envs",
    [
        "uid": uid,
        "globalization-invariant": useGlobalizationInvariantMode
    ]
)}}

RUN {{InsertTemplate("../Dockerfile.linux.install-deps", [
    "is-extra": ARGS["is-extra"]
])}}{{
if isMultiStage:


FROM base AS installer

RUN {{InsertTemplate("../Dockerfile.linux.install-pkgs", [ "pkgs": utilPkgs ])}}

{{nonRootUserComment}}
RUN {{InsertTemplate("Dockerfile.linux.non-root-user-staged",
    [
        "staging-dir": stagingDir,
        "name": username,
        "no-create-home": "true",
        "create-dir": "true"
    ]
)}}


# Final image
FROM base

COPY --from=installer {{stagingDir}}/ /

RUN install -d -m 0755 -o $APP_UID -g $APP_UID "/home/{{username}}"^else:

{{nonRootUserComment}}
RUN {{InsertTemplate("Dockerfile.linux.non-root-user", [ "name": username ])}}}}
