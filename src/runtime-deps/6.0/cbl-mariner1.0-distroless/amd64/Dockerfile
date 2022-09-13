# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:1.0 AS installer

RUN tdnf install -y \
        dnf \
    && tdnf clean all

# Install .NET's dependencies into a staging location
RUN mkdir /staging \
    && dnf install -y --releasever=1.0 --installroot /staging \
        prebuilt-ca-certificates \
        \
        # .NET dependencies
        glibc \
        icu \
        krb5 \
        libgcc \
        libstdc++ \
        openssl-libs \
        zlib \
    && dnf clean all

# Create a non-root user and group
RUN groupadd \
        --system \
        --gid=1000 \
        app \
    && adduser \
        --uid 1000 \
        --gid 1000 \
        --shell /bin/false \
        --no-create-home \
        --system \
        app \
    # Copy user/group info to staging
    && cp /etc/passwd /staging/etc/passwd \
    && cp /etc/group /staging/etc/group

# Clean up staging
RUN rm -rf /staging/etc/dnf \
    && rm -rf /staging/run/* \
    && rm -rf /staging/var/cache/dnf \
    && rm -rf /staging/var/lib/rpm \
    && rm -rf /staging/usr/share/doc \
    && rm -rf /staging/usr/share/man \
    && find /staging/var/log -type f -size +0 -delete


# .NET runtime-deps image
FROM mcr.microsoft.com/cbl-mariner/distroless/minimal:1.0

COPY --from=installer /staging/ /

ENV \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_URLS=http://+:8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true

USER app
