FROM mcr.microsoft.com/cbl-mariner/base/core:2.0

RUN tdnf install -y \
        ca-certificates-microsoft \
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
RUN dotnet_version=7.0.0-rc.1.22403.8 \
    && curl -fSL --output dotnet-runtime-deps.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.2-aarch64.rpm \
    && dotnet_sha512='4096c7c252e36ddc76a2b08c003c09cded801d7061bfeee2a1a2e0091509a8b9fd8a09b45fa5b54459cc13f5230f3e79df0e539204a78f5ba49bc433df919ce6' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
