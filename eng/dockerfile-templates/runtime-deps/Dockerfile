{{
    _ ARGS:
        is-extra (optional): Whether to include extra packages that are not needed for every app,
            for example globalization support ^

    set dotnetVersion to join(slice(split(PRODUCT_VERSION, "."), 0, 2), ".") ^
    set isAlpine to find(OS_ARCH_HYPHENATED, "Alpine") >= 0 ^
    set isDebian to find(OS_ARCH_HYPHENATED, "Debian") >= 0 ^
    set isUbuntu to find(OS_ARCH_HYPHENATED, "Ubuntu") >= 0 ^
    set isMariner to defined(match(OS_VERSION, "^cbl-mariner\d+\.\d+$")) ^

    set baseUrl to VARIABLES[cat("base-url|", dotnetVersion, "|", VARIABLES["branch"])] ^
    set isInternal to find(baseUrl, "msrc") >= 0 || find(baseUrl, "internal") >= 0 ^
    set baseImageRepo to when(isAlpine,
        cat(ARCH_VERSIONED, "/alpine"),
        when(isDebian,
            cat(ARCH_VERSIONED, "/debian"),
            when(isUbuntu,
                "ubuntu.azurecr.io/ubuntu",
                when(isMariner,
                    "mcr.microsoft.com/cbl-mariner/base/core",
                    "<NOT-IMPLEMENTED>")))) ^
    set baseImageTag to when(isAlpine || isMariner, OS_VERSION_NUMBER, OS_VERSION) ^
    set isRpmInstall to isMariner && dotnetVersion = "6.0" ^
    set nonRootUserSupported to dotnetVersion != "6.0" && dotnetVersion != "7.0" ^
    set isMultiStage to (isRpmInstall && isInternal) || (isMariner && nonRootUserSupported) ^
    set firstStageName to when(isRpmInstall, "installer", "base") ^
    set secondStageName to when(isMultiStage && nonRootUserSupported, "installer") ^
    set stagingDir to "/staging" ^
    set createUserHome to is ^
    set urlSuffix to when(isInternal, "$SAS_QUERY_STRING", "") ^
    set rpmFilename to "dotnet-runtime-deps.rpm" ^
    set utilPkgs to when(isMariner && nonRootUserSupported, ["shadow-utils"], []) ^
    set useGlobalizationInvariantMode to !ARGS["is-extra"] && (isDistroless || isAlpine) ^
    set username to "app" ^
    set uid to 1654 ^
    set gid to uid ^
    set nonRootUserComment to "# Create a non-root user and group"
}}FROM {{baseImageRepo}}:{{baseImageTag}}{{if isMultiStage: AS {{firstStageName}}}}{{if isRpmInstall && isInternal:

ARG SAS_QUERY_STRING

RUN {{InsertTemplate("Dockerfile.download-runtime-deps-pkg",
    [
        "url-suffix": urlSuffix,
        "filename": rpmFilename,
        "is-internal": isInternal
    ],
    "    "
)}}


FROM {{baseImageRepo}}:{{baseImageTag}}}}

{{InsertTemplate("../Dockerfile.common-dotnet-envs",
    [
        "uid": uid,
        "globalization-invariant": useGlobalizationInvariantMode
    ]
)}}

RUN {{InsertTemplate("../Dockerfile.linux.install-deps",
    [
        "is-extra": ARGS["is-extra"]
    ]
)}}{{if isRpmInstall:{{if isMultiStage:

{{InsertTemplate("../Dockerfile.linux.copy-files",
    [
        "files": [[ "filename": rpmFilename ]],
        "srcStage": firstStageName,
        "destination": ""
    ]
)}}}}

{{InsertTemplate("Dockerfile.install-runtime-deps-pkg",
    [
        "skip-download": isInternal
        "url-suffix": urlSuffix,
        "filename": rpmFilename
    ]
)}}}}{{if isMultiStage && nonRootUserSupported:


FROM {{firstStageName}} AS {{secondStageName}}

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
FROM {{firstStageName}}

COPY --from={{secondStageName}} {{stagingDir}}/ /

RUN install -d -m 0755 -o $APP_UID -g $APP_UID "/home/{{username}}"^else:{{if nonRootUserSupported:

{{nonRootUserComment}}
RUN {{InsertTemplate("Dockerfile.linux.non-root-user", [ "name": username ])}}}}}}
