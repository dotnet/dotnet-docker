FROM amd64/golang:1.18 as chisel
RUN git clone --depth 1 -b main https://github.com/canonical/chisel /opt/chisel
WORKDIR /opt/chisel
RUN go generate internal/deb/version.go \
    && go build ./cmd/chisel


FROM amd64/ubuntu:jammy as builder

RUN apt-get update && \
    apt-get install -y ca-certificates

RUN groupadd \
        --system \
        --gid=101 \
        app \
    && adduser \
        --uid 101 \
        --gid 101 \
        --shell /bin/false \
        --system \
        app \
    && install -d -m 0755 -o 101 -g 101 "/rootfs/home/app" \
    && mkdir -p "/rootfs/etc" \
    && rootOrAppRegex='^\(root\|app\):' \
    && cat /etc/passwd | grep $rootOrAppRegex > "/rootfs/etc/passwd" \
    && cat /etc/group | grep $rootOrAppRegex > "/rootfs/etc/group"

COPY --from=chisel /opt/chisel/chisel /usr/bin/
RUN chisel cut --release "ubuntu-22.04" --root /rootfs \
        base-files_base \
        ca-certificates_data \
        libc6_libs \
        libgcc-s1_libs \
        libssl3_libs \
        libstdc++6_libs \
        zlib1g_libs


FROM scratch

COPY --from=builder /rootfs /

# Workaround for https://github.com/moby/moby/issues/38710
COPY --from=builder --chown=101:101 /rootfs/home/app /home/app

ENV \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_URLS=http://+:8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since ICU package isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

USER app
