FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS base

ENV \
    # UID of the non-root user 'app'
    APP_UID=64198 \
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
        krb5 \
        libgcc \
        libstdc++ \
        openssl-libs \
        zlib \
    && tdnf clean all


# Create a non-root user and group
FROM base as installer

RUN tdnf install -y \
        shadow-utils \
    && tdnf clean all

RUN mkdir -p /staging/etc \
    && groupadd \
        --system \
        --gid=$APP_UID \
        app \
    && useradd -l \
        --uid=$APP_UID \
        --gid=$APP_UID \
        --create-home \
        --system \
        app \
    && install -d -m 0755 -o $APP_UID -g $APP_UID "/staging/home/app" \
    && rootOrAppRegex='^\(root\|app\):' \
    && cat /etc/passwd | grep $rootOrAppRegex > "/staging/etc/passwd" \
    && cat /etc/group | grep $rootOrAppRegex > "/staging/etc/group"


# Final image
FROM base

COPY --from=installer /staging/ /

# Workaround for https://github.com/moby/moby/issues/38710
COPY --from=installer --chown=64198:64198 /staging/home/app /home/app
