FROM arm32v7/golang:1.18 as chisel

RUN git clone --depth 1 -b main https://github.com/canonical/chisel /opt/chisel \
    && git clone --depth 1 -b main https://github.com/canonical/rocks-toolbox /opt/rocks-toolbox

WORKDIR /opt/chisel

RUN go generate internal/deb/version.go \
    && go build ./cmd/chisel


FROM arm32v7/ubuntu:jammy as builder

RUN apt-get update \
    && apt-get install -y \
        ca-certificates \
        file

RUN groupadd \
        --gid=64198 \
        app \
    && useradd -l \
        --uid=64198 \
        --gid=64198 \
        --shell /bin/false \
        app \
    && install -d -m 0755 -o 64198 -g 64198 "/rootfs/home/app" \
    && mkdir -p "/rootfs/etc" \
    && rootOrAppRegex='^\(root\|app\):' \
    && cat /etc/passwd | grep $rootOrAppRegex > "/rootfs/etc/passwd" \
    && cat /etc/group | grep $rootOrAppRegex > "/rootfs/etc/group"

COPY --from=chisel /opt/chisel/chisel /opt/rocks-toolbox/chisel-wrapper /usr/bin/

RUN mkdir -p /rootfs/var/lib/dpkg/ \
    && chisel-wrapper --generate-dpkg-status /rootfs/var/lib/dpkg/status -- \
        --release "ubuntu-22.04" --root /rootfs \
            base-files_base \
            base-files_release-info \
            ca-certificates_data \
            libc6_libs \
            libgcc-s1_libs \
            libssl3_libs \
            libstdc++6_libs \
            zlib1g_libs


FROM scratch

COPY --from=builder /rootfs /

ENV \
    # UID of the non-root user 'app'
    APP_UID=64198 \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since ICU package isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

# Workaround for https://github.com/moby/moby/issues/38710
COPY --from=builder --chown=$APP_UID:$APP_UID /rootfs/home/app /home/app

USER $APP_UID
