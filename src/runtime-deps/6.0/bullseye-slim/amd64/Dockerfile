FROM amd64/debian:bullseye-slim

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        \
        # .NET Core dependencies
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu67 \
        libssl1.1 \
        libstdc++6 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/* \
    \
    # Non-root user and group
    && addgroup \
        --group dotnet \
        --gid 3000 \
    && adduser \
        --uid 1000 \
        --gid 3000 \
        --disabled-login \
        --no-create-home \
        --system \
        "dotnet"

ENV \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_URLS=http://+:8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true

# Run as non-root
USER 1000:3000
