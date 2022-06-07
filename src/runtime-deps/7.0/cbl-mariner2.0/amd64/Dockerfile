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
RUN dotnet_version=7.0.0-preview.5.22301.12 \
    && curl -fSL --output dotnet-runtime-deps.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.2-x64.rpm \
    && dotnet_sha512='8e524a3285c45742ed1dbd60b9db93223860e87877e9d8c5514deef8fba38f86492d55db12f4d3d1b945c419b6f682254d766b7d18538f7df12c5d39d4ac42ec' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && tdnf install -y dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
