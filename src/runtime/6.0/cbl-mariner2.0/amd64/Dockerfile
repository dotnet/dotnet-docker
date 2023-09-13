ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.22-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.22

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ba5a8eb42de8ee2ccaea15bf4aac9bc8927f8d5cd68f7d6ef4e1900e1dfe9ffecd63ae9e2194269a31744fb42d2d9c0ae82f6f374bb2c2688e8c7edd72f401bf' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='7c840358bb2e2b4062bd46acb61fea0335c732db97abb586ba92067a00aa89527454a7d31bda5c758a9e8654cf96ec0a714fe5e15aabff7abcd444fc20a9404b' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='c7ba8b6b27a86aa9ec711df68bb8726733cee3a832b1d45a631ca533b8533ca2b248b0b8e7cf47dd2c0f79718fe3eedcf54c82eaad725fd5a5839bc3c1e0d0a8' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
