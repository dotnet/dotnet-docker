FROM mcr.microsoft.com/cbl-mariner/base/core:2.0

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
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

# Install dotnet-runtime-deps package
RUN dotnet_version=6.0.36 \
    && curl -fSL --output dotnet-runtime-deps.rpm https://builds-int.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.2-x64.rpm \
    && dotnet_sha512='c480ab7722f34eef1e0e55bf60b393cf6f4dc51ea78878b5a3b0e9c8c45d054d3c81b29a8783732c01ca62d3c5b6298a166a2de7c0e07f70067f22af375b6f1c' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm
