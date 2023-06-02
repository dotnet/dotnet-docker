FROM arm32v7/alpine:3.17

ENV \
    # UID of the non-root user 'app'
    APP_UID=64198 \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since ICU package isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

RUN apk add --no-cache \
        ca-certificates \
        \
        # .NET dependencies
        krb5-libs \
        libgcc \
        libssl3 \
        libstdc++ \
        zlib

# Create a non-root user and group
RUN addgroup \
        --system \
        --gid=$APP_UID \
        app \
    && adduser \
        --uid=$APP_UID \
        --ingroup=app \
        --system \
        app
