FROM arm32v7/buildpack-deps:noble-curl AS chisel

RUN apt-get update && apt-get install -y file

RUN curl -fSL --output chisel.tar.gz https://github.com/canonical/chisel/releases/download/v1.0.0/chisel_v1.0.0_linux_arm.tar.gz \
    && chisel_sha384='1bbe68cf9df8a7a2a0c2718bfd28cc68fa5bee424909d01e305480b0510f58fa40e3aa917e077b83445c426317e5772a' \
    && echo "$chisel_sha384  chisel.tar.gz" | sha384sum -c - \
    && tar -xzf chisel.tar.gz -C /usr/bin/ chisel \
    && curl -fSL --output /usr/bin/chisel-wrapper https://raw.githubusercontent.com/canonical/rocks-toolbox/v1.1.2/chisel-wrapper \
    && chmod 755 /usr/bin/chisel-wrapper

RUN groupadd \
        --gid=1654 \
        app \
    && useradd -l \
        --uid=1654 \
        --gid=1654 \
        --shell /bin/false \
        app \
    && install -d -m 0755 -o 1654 -g 1654 "/rootfs/home/app" \
    && mkdir -p "/rootfs/etc" \
    && rootOrAppRegex='^\(root\|app\):' \
    && cat /etc/passwd | grep $rootOrAppRegex > "/rootfs/etc/passwd" \
    && cat /etc/group | grep $rootOrAppRegex > "/rootfs/etc/group"

RUN mkdir -p /rootfs/var/lib/dpkg/ \
    && chisel-wrapper --generate-dpkg-status /rootfs/var/lib/dpkg/status -- \
        --release ubuntu-24.04 --root /rootfs \
            base-files_base \
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
