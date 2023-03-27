FROM arm32v7/debian:bookworm-slim

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        \
        # .NET dependencies
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu72 \
        libssl3 \
        libstdc++6 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Create a non-root user and group
RUN groupadd \
        --system \
        --gid=64198 \
        app \
    && useradd \
        --uid 64198 \
        --gid 64198 \
        --create-home \
        --system \
        app

ENV \
    # UID of the non-root user 'app'
    APP_UID=64198 \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
