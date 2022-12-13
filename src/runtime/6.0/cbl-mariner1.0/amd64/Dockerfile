ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.12-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.12

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='322c0dd963b5f75a6d872fe513de908ae89fa8eeb811098db4e963f858c1e6a814aa24346b00736cf8a75b34cb7b5463d1ad583806d637bc06ce2048555b727a' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4af99196abf605948963a97ee55074bce4d4b0885a7d2b182e4d081a0f662415039ae6c77df0668095fc33a201d09b225eadd2d8dcc6ab7447e671fe056f445b' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='1333667a481a68b7455ff1be88a5700fa98fb2ba1d06fdd1fa8230790bca3813753f6095069a5dab6bc35c6dc1747770d7ea689a26610de5cabfc1b59524e495' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
