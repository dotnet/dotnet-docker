# Installer image
FROM mcr.microsoft.com/azurelinux-beta/base/core:4.0 AS installer

RUN dnf install -y \
        shadow-utils \
    && dnf clean all

# Install .NET's dependencies into a staging location
RUN mkdir /staging \
    && dnf install -y --use-host-config --installroot /staging \
        # .NET dependencies
        glibc \
        icu \
        libgcc \
        libstdc++ \
        openssl-libs \
        tzdata \
    && dnf clean all --use-host-config --installroot /staging

# Create a non-root user and group
RUN groupadd \
        --gid=1654 \
        app \
    && useradd --no-log-init \
        --uid=1654 \
        --gid=1654 \
        --shell /bin/false \
        --create-home \
        app \
    && install --directory --mode 0755 --owner 1654 --group 1654 "/staging/home/app" \
    && rootOrAppRegex='^\(root\|app\):' \
    && cat /etc/passwd | grep $rootOrAppRegex > "/staging/etc/passwd" \
    && cat /etc/group | grep $rootOrAppRegex > "/staging/etc/group"

# Clean up staging
RUN rm -rf \
        /staging/etc/dnf \
        /staging/run/* \
        /staging/var/cache/dnf \
        /staging/var/lib/rpm \
        /staging/usr/share/doc \
        /staging/usr/share/man \
    && find /staging/var/log -type f -size +0 -delete


# .NET runtime-deps image
FROM mcr.microsoft.com/azurelinux-beta/distroless/minimal:4.0

ENV \
    # UID of the non-root user 'app'
    APP_UID=1654 \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true

COPY --from=installer /staging/ /

# Workaround for https://github.com/moby/moby/issues/38710
COPY --from=installer --chown=1654:1654 /staging/home/app /home/app

USER $APP_UID
