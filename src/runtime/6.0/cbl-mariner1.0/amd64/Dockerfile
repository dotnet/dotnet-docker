ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.7.21368.2

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='2accf0f6bb3a1b724555dcddd71b916cdc719ecf6ac8042c43b8dd7a9eff4894d2b8cfa4963eb8d578cc52f38e83258a36fa4f07cd76edbdb29492ab6e7deab2' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='7a9ab9d064a90f288e3a51cba0784ff322ad4e76cc851beeac653210de7cb39bd1e4232e5520123c04cab2adf6a860da2f2ce21bd5884c2784cddd790cfb62fa' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3007f77d069f49c34a9ad98e1ec51fcca0345858bae6f02877452b2e65a5ff045fe89cbb59865b904f00fd08f74666a4010b4455cf2b4ad3acd9578b4d3754b6' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    \
    ## Manually configure symbol link due to https://github.com/dotnet/runtime/issues/53543
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
