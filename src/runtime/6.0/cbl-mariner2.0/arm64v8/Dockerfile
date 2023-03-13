ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.15-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.15

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='d296af853dd637520164ad745a3aea7a048b2f972e14d5947db986729099de219c08c560bf10cc89a0e39b21a4cc03445ecaeefe04b9872e9ccf5b7310592c79' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='31feb1a3d809a0a4c645e1ebacebe5f90848bc1e7ca122c7dcf7ea63c3757dd0c739cc7811fb08359b13bbda9d2c18acd282b6740216b1c243f6130d9910f284' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='20e437efdcddfd826c0e6c573809a017c3ca22af8ef6826272f13b7afcb6a221ef002514700106ce559028e9d2d66d37aa74b7404ce9570e38d9131c615147bc' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
