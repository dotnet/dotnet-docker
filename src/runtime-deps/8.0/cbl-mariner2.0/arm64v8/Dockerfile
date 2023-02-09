FROM mcr.microsoft.com/cbl-mariner/base/core:2.0

RUN tdnf install -y \
        shadow-utils \
        ca-certificates \
        \
        # .NET dependencies
        glibc \
        icu \
        krb5 \
        libgcc \
        libstdc++ \
        openssl-libs \
        zlib \
    && tdnf clean all \
    # Create a non-root user and group
    && groupadd \
        --system \
        --gid=101 \
        app \
    && adduser \
        --uid 101 \
        --gid 101 \
        --shell /bin/false \
        --create-home \
        --system \
        app \
    && tdnf remove -y \
        shadow-utils \
    && tdnf clean all

ENV \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
