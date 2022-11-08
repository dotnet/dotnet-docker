FROM mcr.microsoft.com/cbl-mariner/base/core:1.0

RUN tdnf install -y \
        ca-certificates \
        \
        # .NET Core dependencies
        glibc \
        icu \
        krb5 \
        libgcc \
        libstdc++ \
        openssl-libs \
        zlib \
    && tdnf clean all

# Install dotnet-runtime-deps package
RUN dotnet_version=3.1.31 \
    && curl -fSL --output dotnet-runtime-deps.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.1-x64.rpm \
    && dotnet_sha512='31e7553592cdda3f180267be95a8bbb9ae014571521a9d54fdde86d0768c06831223e155aeadee110bfee3445427a21e62f2f9ca72284daa86c02f7ffff7818c' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && rpm --install dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
