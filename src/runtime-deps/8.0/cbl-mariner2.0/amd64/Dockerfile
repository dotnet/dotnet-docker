FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS base

ENV \
    # UID of the non-root user 'app'
    APP_UID=1654 \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true

RUN tdnf install -y \
        ca-certificates \
        \
        # .NET dependencies
        glibc \
        icu \
        libgcc \
        libstdc++ \
        openssl-libs \
        tzdata \
        zlib \
    && tdnf clean all


FROM base AS installer

RUN tdnf install -y \
        shadow-utils \
    && tdnf clean all

# Create a non-root user and group
RUN groupadd \
        --gid=$APP_UID \
        app \
    && useradd -l \
        --uid=$APP_UID \
        --gid=$APP_UID \
        --no-create-home \
        app \
    && mkdir -p "/staging/etc" \
    # Copy user/group info to staging
    && cp /etc/passwd /staging/etc/passwd \
    && cp /etc/group /staging/etc/group


# Final image
FROM base

COPY --from=installer /staging/ /

RUN install -d -m 0755 -o $APP_UID -g $APP_UID "/home/app"
