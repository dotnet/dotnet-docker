# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

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
    && tdnf clean all

# Create a non-root user and group
RUN tdnf install -y shadow-utils \
    && tdnf clean all \
    && groupadd \
        --system \
        --gid=101 \
        app \
    && adduser \
        --uid 101 \
        --gid 101 \
        --shell /bin/false \
        --no-create-home \
        --system \
        app \
    && tail -1 < /etc/passwd > "/staging/etc/passwd" \
    && tail -1 < /etc/group > "/staging/etc/group"

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

COPY --from=installer /staging/ /

ENV \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_URLS=http://+:8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since ICU package isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

USER app
