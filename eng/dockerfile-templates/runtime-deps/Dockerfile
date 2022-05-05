{{
    set dotnetVersion to join(slice(split(PRODUCT_VERSION, "."), 0, 2), ".") ^
    set isAlpine to find(OS_ARCH_HYPHENATED, "Alpine") >= 0 ^
    set isDebian to find(OS_ARCH_HYPHENATED, "Debian") >= 0 ^
    set isUbuntu to find(OS_ARCH_HYPHENATED, "Ubuntu") >= 0 ^
    set isFullMariner to defined(match(OS_VERSION, "^cbl-mariner\d+\.\d+$")) ^
    set isDistrolessMariner to defined(match(OS_VERSION, "^cbl-mariner\d+\.\d+-distroless$")) ^
    set isMariner to isFullMariner || isDistrolessMariner ^
    set baseUrl to VARIABLES[cat("base-url|", dotnetVersion, "|", VARIABLES["branch"])] ^
    set isInternal to find(baseUrl, "msrc") >= 0 || find(baseUrl, "internal") >= 0 ^
    set distrolessStagingDir to "/staging" ^
    set marinerRepo to "mcr.microsoft.com/cbl-mariner" ^
    set baseImageRepo to when(isAlpine,
        cat(ARCH_VERSIONED, "/alpine"),
        when(isDebian,
            cat(ARCH_VERSIONED, "/debian"),
            when(isUbuntu,
                cat(ARCH_VERSIONED, "/ubuntu"),
                when(isMariner,
                    cat(marinerRepo, "/base/core"),
                    "<NOT-IMPLEMENTED>")))) ^
    set baseImageTag to when(isAlpine || isMariner, OS_VERSION_NUMBER, OS_VERSION) ^
    set isSingleStage to !isDistrolessMariner && !(isFullMariner && isInternal && ARCH_SHORT != "arm64") ^
    set urlSuffix to when(isInternal, "SAS_QUERY_STRING", "") ^
    set rpmFilename to "dotnet-runtime-deps.rpm"
}}{{
    if !isSingleStage:# Installer image
}}FROM {{baseImageRepo}}:{{baseImageTag}}{{if !isSingleStage: AS installer}}{{ if isInternal && isFullMariner && ARCH_SHORT != "arm64":

RUN {{InsertTemplate("Dockerfile.download-runtime-deps-pkg",
    [
        "url-suffix": urlSuffix,
        "filename": rpmFilename
    ], "    ")}}}}
{{ if isDistrolessMariner && find(OS_VERSION, "1.0") >= 0:
RUN {{InsertTemplate("../Dockerfile.linux.install-pkgs",
    [
        "pkgs": ["dnf"]
    ])}}
}}
{{if isDistrolessMariner:# Install .NET's dependencies into a staging location
}}RUN {{if isDistrolessMariner:mkdir {{distrolessStagingDir}} \
    && }}{{InsertTemplate("../Dockerfile.linux.install-deps", ["distroless-staging-dir": distrolessStagingDir])}}
{{ if isDistrolessMariner:
# Create a non-root user and group
RUN {{if find(OS_VERSION, "1.0") < 0:tdnf install -y shadow-utils \
    && tdnf clean all \
    && }}groupadd \
        --system \
        --gid={{if find(OS_VERSION, "1.0") >= 0:1000^else:101}} \
        app \
    && adduser \
        --uid {{if find(OS_VERSION, "1.0") >= 0:1000^else:101}} \
        --gid app \
        --shell /bin/false \
        --no-create-home \
        --system \
        app \
    # Copy user/group info to staging
    && cp /etc/passwd {{distrolessStagingDir}}/etc/passwd \
    && cp /etc/group {{distrolessStagingDir}}/etc/group

# Clean up staging
RUN rm -rf {{distrolessStagingDir}}/etc/dnf \
    && rm -rf {{distrolessStagingDir}}/run/* \
    && rm -rf {{distrolessStagingDir}}/var/cache/dnf \
    && find {{distrolessStagingDir}}/var/log -type f -size +0 -delete


# .NET runtime-deps image
FROM {{marinerRepo}}/distroless/minimal:{{OS_VERSION_NUMBER}}

COPY --from=installer {{distrolessStagingDir}}/ /
^elif isFullMariner && ARCH_SHORT != "arm64":
{{if isInternal:{{InsertTemplate("../Dockerfile.linux.copy-files",
[
    "files": [
        [
            "filename": rpmFilename
        ]
    ],
    "srcStage": "installer",
    "destination": ""
])}}
}}{{InsertTemplate("Dockerfile.install-runtime-deps-pkg",
    [
        "skip-download": isInternal
        "url-suffix": urlSuffix,
        "filename": rpmFilename
    ])}}
}}
{{InsertTemplate("../Dockerfile.common-dotnet-envs") ^
if isDistrolessMariner:

USER app}}
