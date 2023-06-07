{{
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
    set isSingleStage to !(isRpmInstall && isInternal) && !(isMariner && dotnetVersion != "6.0" && dotnetVersion != "7.0") ^
    set depsStagingDir to "/staging" ^
    set createUserHome to dotnetVersion != "6.0" ^
    set urlSuffix to when(isInternal, "$SAS_QUERY_STRING", "") ^
    set rpmFilename to "dotnet-runtime-deps.rpm" ^
    set utilPkgs to when(isMariner && dotnetVersion != "6.0" && dotnetVersion != "7.0", ["shadow-utils"], []) ^
    set username to "app" ^
    set uid to 64198 ^
    set gid to uid ^
    set nonRootUserComment to "# Create a non-root user and group"
}}FROM {{baseImageRepo}}:{{baseImageTag}}{{if !isSingleStage: AS base}}

{{InsertTemplate("../Dockerfile.common-dotnet-envs", [
    "uid": uid
])}}
{{if isInternal && isRpmInstall:
RUN {{InsertTemplate("Dockerfile.download-runtime-deps-pkg",
    [
        "url-suffix": urlSuffix,
        "filename": rpmFilename,
        "is-internal": isInternal
    ], "    ")}}}}
RUN {{InsertTemplate("../Dockerfile.linux.install-deps")}}{{if isRpmInstall:

{{InsertTemplate("Dockerfile.install-runtime-deps-pkg",
    [
        "url-suffix": urlSuffix,
        "filename": rpmFilename
    ])}}}}{{if dotnetVersion != "6.0" && dotnetVersion != "7.0":
{{if !isSingleStage:

{{nonRootUserComment}}
FROM base as installer
{{if len(utilPkgs) > 0:
RUN {{InsertTemplate("../Dockerfile.linux.install-pkgs", [ "pkgs": utilPkgs ])}}
}}}}
{{if isSingleStage:{{nonRootUserComment}}
RUN {{InsertTemplate("Dockerfile.linux.non-root-user", [ "name": username ])}}^
else:RUN mkdir -p {{depsStagingDir}}/etc \
    && {{InsertTemplate("Dockerfile.linux.non-root-user-staged",
            [
                "staging-dir": depsStagingDir,
                "exclusive": dotnetVersion != "6.0",
                "name": username,
                "no-create-home": !createUserHome
            ]
        )}}}}}}{{if !isSingleStage:


# Final image
FROM base

COPY --from=installer {{depsStagingDir}}/ /{{if createUserHome:

# Workaround for https://github.com/moby/moby/issues/38710
COPY --from=installer --chown=$APP_UID:$APP_UID {{depsStagingDir}}/home/{{username}} /home/{{username}}}}{{if isRpmInstall:

{{if isInternal:{{InsertTemplate("../Dockerfile.linux.copy-files",
[
    "files": [
        [
            "filename": rpmFilename
        ]
    ],
    "srcStage": "installer",
    "destination": ""
])}}}}
{{InsertTemplate("Dockerfile.install-runtime-deps-pkg",
    [
        "skip-download": isInternal
        "url-suffix": urlSuffix,
        "filename": rpmFilename
    ])}}}}}}
