ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.26-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.26

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='f3f6a174fc5ee55093c208caaa20b458546d4b9c66fa137d4610b72a55abe52de8098c8f56c31bf4961db7bc7e6b89c13a7ea8bf7588cb9cef12b475be3d66c9' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='bc67236caf7911ae79bd2292c752c533b3161578aa0f489f7c033b96745766194a46271cd5b057595174d178d7f90b5a901971c123917b4432800008bc4b5a6e' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='0ffe1150c22c474060c418a93d3b5c0d9ff1c126029ce31b82736ce4ca169169f04de2884b442888a15a248cad56b3c4ec778bf9aa85125591c1b16542d4ea49' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
