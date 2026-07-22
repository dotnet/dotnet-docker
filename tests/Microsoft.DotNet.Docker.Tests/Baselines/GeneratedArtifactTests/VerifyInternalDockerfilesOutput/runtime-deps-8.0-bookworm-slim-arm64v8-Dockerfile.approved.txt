FROM arm64v8/debian:bookworm-slim

ENV \
    # UID of the non-root user 'app'
    APP_UID=1654 \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true

RUN __netiso_apt_srcs="$(find /etc/apt -type f \( -name '*.list' -o -name '*.sources' \))" \
    && __netiso_backup_dir="$(mktemp -d)" \
    && for f in $__netiso_apt_srcs; do cp -a --parents "$f" "$__netiso_backup_dir" || exit 1; done \
    && for f in $__netiso_apt_srcs; do sed -E -i 's#([[:space:]/])deb\.debian\.org/debian([[:space:]/-]|$)#\1debian-archive.trafficmanager.net/debian\2#g' "$f" || exit 1; done \
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        \
        # .NET dependencies
        libc6 \
        libgcc-s1 \
        libicu72 \
        libssl3 \
        libstdc++6 \
        tzdata \
        zlib1g \
    && for f in $__netiso_apt_srcs; do cp -a "$__netiso_backup_dir$f" "$f" || exit 1; done \
    && rm -rf "$__netiso_backup_dir" \
    && rm -rf /var/lib/apt/lists/*

# Create a non-root user and group
RUN groupadd \
        --gid=$APP_UID \
        app \
    && useradd --no-log-init \
        --uid=$APP_UID \
        --gid=$APP_UID \
        --create-home \
        app
