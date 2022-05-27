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
RUN dotnet_version=7.0.0-preview.5.22272.3 \
    && curl -fSL --output dotnet-runtime-deps.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.1-x64.rpm \
    && dotnet_sha512='b03c30cd0a06859d66544b64788b0c4d65fb96df2682e2d18e11bfec5fb9e3c11e024f95e625b779386d39ed4d8a7e500e01ccfe264f03be71b0eb58715ea79b' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && tdnf install -y dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
