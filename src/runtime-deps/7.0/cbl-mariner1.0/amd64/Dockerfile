FROM cblmariner.azurecr.io/base/core:1.0

RUN tdnf install -y \
        prebuilt-ca-certificates \
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
RUN dotnet_version=7.0.0-preview.1.22076.8 \
    && curl -fSL --output dotnet-runtime-deps.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.1-x64.rpm \
    && dotnet_sha512='31bf16e63e48160e2747c5fd315dccd01a97c01e1c7fbeaa647685b2473b7684de609b7ac30a4bffca5e188200e7ca0dc77e06327b0d547e64ea03759bfbb74e' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && rpm --install dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
