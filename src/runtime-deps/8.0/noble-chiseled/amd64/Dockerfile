FROM amd64/buildpack-deps:noble-curl AS chisel

# Find the latest chisel releases: https://github.com/canonical/chisel/releases
ARG CHISEL_VERSION="v1.3.0"

# Find the latest chisel-wrapper releases: https://github.com/canonical/rocks-toolbox/releases
ARG ROCKS_TOOLBOX_VERSION="v1.2.0"

RUN apt-get update && apt-get install -y file

RUN chisel_url="https://github.com/canonical/chisel/releases/download/${CHISEL_VERSION}/chisel_${CHISEL_VERSION}_linux_amd64.tar.gz" \
    && curl --fail --show-error --location --remote-name ${chisel_url} \
    && curl --fail --show-error --location ${chisel_url}.sha384 | sha384sum -c - \
    && tar --gzip --extract --no-same-owner --file chisel_${CHISEL_VERSION}_linux_amd64.tar.gz --directory /usr/bin/ \
    && rm chisel_${CHISEL_VERSION}_linux_amd64.tar.gz \
    && curl --fail --show-error --location --output /usr/bin/chisel-wrapper https://raw.githubusercontent.com/canonical/rocks-toolbox/${ROCKS_TOOLBOX_VERSION}/chisel-wrapper \
    && chmod 755 /usr/bin/chisel-wrapper

RUN groupadd \
        --gid=1654 \
        app \
    && useradd --no-log-init \
        --uid=1654 \
        --gid=1654 \
        --shell /bin/false \
        app \
    && install --directory --mode 0755 --owner 1654 --group 1654 "/rootfs/home/app" \
    && mkdir --parents "/rootfs/etc" \
    && rootOrAppRegex='^\(root\|app\):' \
    && cat /etc/passwd | grep $rootOrAppRegex > "/rootfs/etc/passwd" \
    && cat /etc/group | grep $rootOrAppRegex > "/rootfs/etc/group"

RUN mkdir --parents /rootfs/var/lib/dpkg/ \
    && chisel-wrapper --generate-dpkg-status /rootfs/var/lib/dpkg/status -- \
        --release ubuntu-24.04 --root /rootfs \
            base-files_base \
            base-files_release-info \
            ca-certificates_data \
            libc6_libs \
            libgcc-s1_libs \
            libssl3t64_libs \
            libstdc++6_libs \
            zlib1g_libs


FROM scratch

COPY --from=chisel /rootfs /

ENV \
    # UID of the non-root user 'app'
    APP_UID=1654 \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since ICU package isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

# Workaround for https://github.com/moby/moby/issues/38710
COPY --from=chisel --chown=$APP_UID:$APP_UID /rootfs/home/app /home/app

USER $APP_UID
