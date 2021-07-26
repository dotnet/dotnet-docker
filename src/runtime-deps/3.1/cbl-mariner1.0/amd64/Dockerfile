FROM cblmariner.azurecr.io/base/core:1.0

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
RUN dotnet_version=3.1.17 \
    && curl -SL --output dotnet-runtime-deps.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.1-x64.rpm \
    && dotnet_sha512='5d453dcc251319037a809c83115563fcbb853835f545b01c4f3d099087c89e511ff4610e5d48904b14cc569ffec1a8d3d37ca055b0b7dd02e31b7c1f8c3929fa' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && rpm --install dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
