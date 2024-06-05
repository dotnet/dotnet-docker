FROM azurelinuxpreview.azurecr.io/public/azurelinux/base/core:3.0

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

# Create a non-root user and group
RUN tdnf install -y \
        shadow-utils \
    && groupadd \
        --gid=$APP_UID \
        app \
    && useradd -l \
        --uid=$APP_UID \
        --gid=$APP_UID \
        --create-home \
        app \
    && tdnf autoremove -y \
        shadow-utils \
    && tdnf clean all
