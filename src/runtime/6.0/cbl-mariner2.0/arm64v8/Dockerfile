ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.14-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.14

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='d7765c546632dfb8827f6b5f04db3d8ec0d249d66a3e34491194bead8a871026ad743ce9e4e7a97b1bea0117b8caecaf6a269605f63b89e69c02ac62d47f9e1a' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='0d21b4b9122ac7fee6ad05708e0d5b7b0389a5ec4ed072df2dec6a07475364bb4021751ce58792740d2132f306e3c206d887fb19453da969e5d7160d3ce0e697' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='5e1539ed893a3788eb702d30a4db72fd16b6cc12af2ef75ba160a481c70b9af3a976adea4de25bc66d4f58d3883154cf2641213bc6bd7b4efd1e5d22659300be' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
