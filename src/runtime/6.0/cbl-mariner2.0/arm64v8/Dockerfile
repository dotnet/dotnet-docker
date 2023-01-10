ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.13-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.13

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='030406e134272a5f5c44971c52c17c6e6072d51f4f288da1af4722985e7e19c748d51855fc5dc0b86b93b649b9f4305a4acf47c683a19029d441d6830037f102' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='be0b34d570a8bc22cc08e9e90e079b192ac1dd8685a95ff3615b97f5891004ccb8ee0400fccebe9e80c4be20c2743e20dd81000c710a613fd6f70dd1cd72bc98' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='9d4253b96c33f53b3313857323f1bac185585241213565a1f83fc0b50376779cf66fff3d3a66d38441ab0b9df4ad2ba78234ec200e8b79c48832a378acc68f56' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
