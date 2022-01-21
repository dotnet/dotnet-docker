{{
    set dotnetVersion to join(slice(split(PRODUCT_VERSION, "."), 0, 2), ".") ^
    set isAlpine to find(OS_ARCH_HYPHENATED, "Alpine") >= 0 ^
    set isDebian to find(OS_ARCH_HYPHENATED, "Debian") >= 0 ^
    set isUbuntu to find(OS_ARCH_HYPHENATED, "Ubuntu") >= 0 ^
    set isFullMariner to defined(match(OS_VERSION, "^cbl-mariner\d+\.\d+$")) ^
    set isDistrolessMariner to defined(match(OS_VERSION, "^cbl-mariner\d+\.\d+-distroless$")) ^
    set isMariner to isFullMariner || isDistrolessMariner ^
    set distrolessStagingDir to "/staging" ^
    set baseImageRepo to when(isAlpine,
        cat(ARCH_VERSIONED, "/alpine"),
        when(isDebian,
            cat(ARCH_VERSIONED, "/debian"),
            when(isUbuntu,
                cat(ARCH_VERSIONED, "/ubuntu"),
                when(isMariner,
                    "cblmariner.azurecr.io/base/core",
                    "<NOT-IMPLEMENTED>")))) ^
    set baseImageTag to when(isAlpine || isMariner, OS_VERSION_NUMBER, OS_VERSION) ^
    set dotnetDepsComment to cat("# .NET", when(dotnetVersion = "3.1", " Core", ""), " dependencies") ^
    set pkgs to when(isAlpine,
        [
            "ca-certificates",
            "",
            dotnetDepsComment,
            "krb5-libs",
            "libgcc",
            "libintl",
            "libssl1.1",
            "libstdc++",
            "zlib"
        ],
        when(isMariner,
            [
                when(dotnetVersion = "3.1" || dotnetVersion = "5.0", "ca-certificates", "prebuilt-ca-certificates"),
                "",
                dotnetDepsComment,
                "glibc",
                "icu",
                "krb5",
                "libgcc",
                "libstdc++",
                "openssl-libs",
                "zlib"
            ],
            [
                "ca-certificates",
                "",
                dotnetDepsComment,
                "libc6",
                "libgcc1",
                "libgssapi-krb5-2",
                cat("libicu", VARIABLES[cat("libicu|", OS_VERSION)]),
                "libssl1.1",
                "libstdc++6",
                "zlib1g"
            ]))
}}{{
    if isDistrolessMariner:# Installer image
}}FROM {{baseImageRepo}}:{{baseImageTag}}{{if isDistrolessMariner: AS installer}}
{{ if isDistrolessMariner:
RUN {{InsertTemplate("../Dockerfile.linux.install-pkgs",
    [
        "pkgs": ["dnf"]
    ])}}
}}
{{if isDistrolessMariner:# Install .NET's dependencies into a staging location
}}RUN {{if isDistrolessMariner:mkdir /staging \
    && }}{{InsertTemplate("../Dockerfile.linux.install-pkgs",
    [
        "pkgs": pkgs,
        "noninteractive": (OS_VERSION = "focal"),
        "pkg-mgr": when(isDistrolessMariner, "dnf", ""),
        "pkg-mgr-opts": when(isDistrolessMariner,
            cat(" --releasever=", OS_VERSION_NUMBER, " --installroot ", distrolessStagingDir),
            ""),
    ])}}
{{ if isDistrolessMariner:
# Create a non-root user and group
RUN groupadd \
        --system \
        --gid=1000 \
        app \
    && adduser \
        --uid 1000 \
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
FROM cblmariner.azurecr.io/distroless/minimal:{{OS_VERSION_NUMBER}}

COPY --from=installer {{distrolessStagingDir}}/ /
^elif isFullMariner:
# Install dotnet-runtime-deps package
RUN dotnet_version={{VARIABLES[cat("runtime|", dotnetVersion, "|build-version")]}} \
    && curl -fSL --output dotnet-runtime-deps.rpm {{VARIABLES[cat("base-url|", dotnetVersion, "|", VARIABLES["branch"])]}}/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.1-{{ARCH_SHORT}}.rpm \
    && dotnet_sha512='{{VARIABLES[cat("runtime-deps-cm.1|", dotnetVersion, "|linux-rpm|", ARCH_SHORT, "|sha")]}}' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && rpm --install dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm
}}
ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true{{if isAlpine: \
    # Set the invariant mode since icu-libs isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true}}{{ if isDistrolessMariner:

USER app}}
