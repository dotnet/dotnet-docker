ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.4-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.4.22229.4

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='190c6c9bfb013b4afb68e317bc21fb4a90b2ed53c20db8e32159c1429025d8f33d0aa3503ddc6c8e9b48de6dff2fe4f17eb677d5fa0bebcbb3ec79fe0ebb17de' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='21989e90160e59ed78206fd85b11486134c089743cd922b56a4871714fc0c92673ccec4feb997dd0ed2c972e1fb2d3f5ef5e9c1f07fa0a25384332a8a5cabc6f' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='fcce15f81c8c945fa0a5f8c5808d27c216a4d2c9c4a3be1ac7394c0d5a08b030cf6163be5b0a43fb86f91ba7ae28177e8e08d82f04aab3ba6e57e8cd0481d536' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
