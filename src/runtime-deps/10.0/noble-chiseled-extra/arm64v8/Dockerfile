FROM mcr.microsoft.com/oss/go/microsoft/golang:1.25.8-bookworm AS chisel

RUN apt-get update \
    && apt-get install -y --no-install-recommends file \
    && rm -rf /var/lib/apt/lists/*

RUN git init /src/chisel \
    && git -C /src/chisel remote add origin https://github.com/canonical/chisel.git \
    && git -C /src/chisel fetch --depth 1 origin 1262471cb663f57f3e201f100019f9bf5b4a7d4f \
    && git -C /src/chisel checkout --detach FETCH_HEAD \
    && sed --in-place 's|http://archive.ubuntu.com/ubuntu/|http://azure.archive.ubuntu.com/ubuntu/|' /src/chisel/internal/archive/archive.go

WORKDIR /src/chisel

ENV CHISEL_BIN="go run ./cmd/chisel"

RUN curl --fail --show-error --location --output /usr/bin/chisel-wrapper https://raw.githubusercontent.com/canonical/rocks-toolbox/v1.2.0/chisel-wrapper \
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
            base-files_chisel \
            base-files_release-info \
            ca-certificates_data \
            libc6_libs \
            libgcc-s1_libs \
            libicu74_libs \
            libssl3t64_libs \
            libstdc++6_libs \
            tzdata-legacy_zoneinfo \
            tzdata_zoneinfo


FROM scratch

COPY --from=chisel /rootfs /

ENV \
    # UID of the non-root user 'app'
    APP_UID=1654 \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true

# Workaround for https://github.com/moby/moby/issues/38710
COPY --from=chisel --chown=$APP_UID:$APP_UID /rootfs/home/app /home/app

USER $APP_UID
