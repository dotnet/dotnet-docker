ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.33-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.33

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='d7810f5a4b4c24ffd4f8f37d8c27c3d19dac4dd24f0b19513c2bca60ca31b3d03ae22d7e9a4ad497f0ef8d8d800db7ad44fdcd474cf641fe1abf2272f01fd7a4' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='149f011966db5a33776892410030d3a955425a5c417d9b76b7acdc97230a33698410aa3dc532c2cc7531a0ee9fe7a5db4981e704cedd1821c3dd50af639fb401' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='b013c9d0dd4fbac2ec3d6e69a04119078a6430750593034dad14ed183818449401f1e5d5137b213935049563a9c58f7b2dfe7bd065a2156df42b0659ee4cd8ac' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
