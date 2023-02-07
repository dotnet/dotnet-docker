FROM arm64v8/alpine:3.17

RUN apk add --no-cache \
        ca-certificates \
        \
        # .NET dependencies
        krb5-libs \
        libgcc \
        libintl \
        libssl3 \
        libstdc++ \
        zlib

# Create a non-root user and group
RUN addgroup \
        --system \
        --gid 101 \
        app \
    && adduser \
        --uid 101 \
        --ingroup app \
        --shell /bin/false \
        --system \
        app

ENV \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since ICU package isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
