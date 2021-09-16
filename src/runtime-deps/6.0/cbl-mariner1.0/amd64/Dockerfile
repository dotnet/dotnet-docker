FROM cblmariner.azurecr.io/base/core:1.0

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

# Install dotnet-runtime-deps package
RUN dotnet_version=6.0.0-rc.1.21451.13 \
    && curl -SL --output dotnet-runtime-deps.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.1-x64.rpm \
    && dotnet_sha512='a73989612d15befcf1ce606c256e05935dac764f543feb254430adf05a35f9b1a0d17aa1930040222449a5f8619d58b9c0f4604c11e8302b538c1d7fcd92daf1' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && rpm --install dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm \
    \
    # Non-root user and group
    && groupadd \
        --gid 3000 \
        dotnet \
    && adduser \
        --uid 1000 \
        --gid 3000 \
        --shell /usr/sbin/nologin \
        --no-create-home \
        --system \
        "dotnet"

ENV \
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_URLS=http://+:8080 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true

# Run as non-root
USER 1000:3000
