# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        gawk \
        shadow-utils \
    && tdnf clean all

# Install .NET's dependencies into a staging location
RUN mkdir /staging \
    && tdnf install -y --releasever=2.0 --installroot /staging \
        prebuilt-ca-certificates \
        \
        # .NET dependencies
        glibc \
        icu \
        libgcc \
        libstdc++ \
        openssl-libs \
        tzdata \
        zlib \
    && tdnf clean all --releasever=2.0 --installroot /staging

# Generate RPM manifest file by appending to the original manifest file from base distroless image
COPY --from=mcr.microsoft.com/cbl-mariner/distroless/minimal:2.0 /var/lib/rpmmanifest/container-manifest-2 /tmp/rpmmanifest
RUN tmpManifestPath="/tmp/rpmmanifest" \
    && rpm --query --all --queryformat "%{NAME}\t%{VERSION}-%{RELEASE}\t%{INSTALLTIME}\t%{BUILDTIME}\t%{VENDOR}\t%{EPOCH}\t%{SIZE}\t%{ARCH}\t%{EPOCHNUM}\t%{SOURCERPM}\n" --root /staging | grep -v gpg-pubkey >> $tmpManifestPath \
    && mkdir -p /staging/var/lib/rpmmanifest \
    # Remove duplicates that match on the first field (package name)
    && tac $tmpManifestPath | gawk '!x[$1]++' | sort > /staging/var/lib/rpmmanifest/container-manifest-2

# Create a non-root user and group
RUN groupadd \
        --gid=1654 \
        app \
    && useradd -l \
        --uid=1654 \
        --gid=1654 \
        --shell /bin/false \
        --create-home \
        app \
    && install -d -m 0755 -o 1654 -g 1654 "/staging/home/app" \
    && rootOrAppRegex='^\(root\|app\):' \
    && cat /etc/passwd | grep $rootOrAppRegex > "/staging/etc/passwd" \
    && cat /etc/group | grep $rootOrAppRegex > "/staging/etc/group"

# Clean up staging
RUN rm -rf /staging/etc/tdnf \
    && rm -rf /staging/run/* \
    && rm -rf /staging/var/cache/tdnf \
    && rm -rf /staging/var/lib/rpm \
    && rm -rf /staging/usr/share/doc \
    && rm -rf /staging/usr/share/man \
    && find /staging/var/log -type f -size +0 -delete


# .NET runtime-deps image
FROM mcr.microsoft.com/cbl-mariner/distroless/minimal:2.0

ENV \
    # UID of the non-root user 'app'
    APP_UID=1654 \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true

COPY --from=installer /staging/ /

# Workaround for https://github.com/moby/moby/issues/38710
COPY --from=installer --chown=1654:1654 /staging/home/app /home/app

USER app
