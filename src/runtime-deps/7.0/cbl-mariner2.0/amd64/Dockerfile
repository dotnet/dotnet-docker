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
RUN dotnet_version=7.0.0-rc.2.22472.3 \
    && curl -fSL --output dotnet-runtime-deps.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.2-x64.rpm \
    && dotnet_sha512='682df20f01a341c0d9d983824c7e2a08603a8316bfb882624d98fa27cb9a749bfe31adcbd22f1ddac334d1a19ffbe0c090d0995811a5ea37a13dfff56972f15f' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
