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
RUN dotnet_version=6.0.6 \
    && curl -fSL --output dotnet-runtime-deps.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.2-aarch64.rpm \
    && dotnet_sha512='02de1bab70f66e26d3fdf2b97b9b751a62bb42b632a645e0326a7e8b42d45c843dcf9c05ad2887dd0f594cf1e4838a727a69ceeed14f7e18461bd5ac39ec9c5d' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && tdnf install -y dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
