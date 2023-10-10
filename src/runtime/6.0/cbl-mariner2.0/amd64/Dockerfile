ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.23-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.23

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='9ae288ef02377a82a8d4ceea0483d92850fd6eab9bb2e9ffb7f65f51dbb9a6f44597338dcadf298f0fd0eccfbc76b14d014ea0ee5c2329658ce03bf63ce82447' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='1cb5f0eefe6045913f20fb02f2bca9df4ad78bc5d2589485ab3d068ba7b5ed783d326cadeb8290a041331915c570babddce4584905ec952667cb68419d263375' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='fe872047c7e02ae86d3969555b69faebdfba49806e80492a8c97a353ee8551ef9c218892fb08de4c71377d80889925a4e0213a07c58cb916c4f8d3ccab45dda3' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
