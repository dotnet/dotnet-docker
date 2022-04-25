# Installer image
FROM cblmariner.azurecr.io/base/core:2.0 AS installer

RUN tdnf install -y \
        dnf \
    && tdnf clean all

# Install .NET's dependencies into a staging location
RUN mkdir /staging \
    && dnf install -y --releasever=2.0 --installroot /staging \
        prebuilt-ca-certificates \
        \
        # .NET dependencies
        glibc \
        krb5 \
        libgcc \
        libstdc++ \
        openssl-libs \
        zlib \
    && dnf clean all

# Create a non-root user and group
RUN groupadd \
        --system \
        --gid=101 \
        app \
    && adduser \
        --uid 101 \
        --gid app \
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
    && find /staging/var/log -type f -size +0 -delete


# .NET runtime-deps image
FROM cblmariner.azurecr.io/distroless/minimal:2.0

COPY --from=installer /staging/ /

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since ICU package isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

USER app
