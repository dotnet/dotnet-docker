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
RUN dotnet_version=7.0.0-alpha.1.22062.8 \
    && curl -fSL --output dotnet-runtime-deps.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.1-x64.rpm \
    && dotnet_sha512='60192fd6f0d81dc68dd88068da3d90e56fba06926ba3dcb932c8e70cdcdaedbbc00f8fae2a69339bbbb3d74e78bb2fb253839b09d3f6c1905f80de88f9f1b062' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && rpm --install dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
