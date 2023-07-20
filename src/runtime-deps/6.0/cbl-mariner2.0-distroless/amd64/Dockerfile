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
        krb5 \
        libgcc \
        libstdc++ \
        openssl-libs \
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
        --system \
        --gid=101 \
        app \
    && useradd -l \
        --uid=101 \
        --gid=101 \
        --shell /bin/false \
        --no-create-home \
        --system \
        app \
    # Copy user/group info to staging
    && cp /etc/passwd /staging/etc/passwd \
    && cp /etc/group /staging/etc/group

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
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_URLS=http://+:8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since ICU package isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

COPY --from=installer /staging/ /

USER app
