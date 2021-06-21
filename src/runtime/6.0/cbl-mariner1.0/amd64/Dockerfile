ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.6.21317.12

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='c33bd55fe97596bc66c0c67355f5482c4ca9d9ce800f79638364392d0a9c3898f26ab49633c015847ef0dc257c77e2e857049ecb3ae0f942fa5ee1d3a4faa58d' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4d58a97f360ad0c6922907f4c7e8a00f6cb38741265edf5310be573e6c09b29751c801073ae93c8eb1f79b01aa6791cab17c0f96903cc958b66ea75b98722846' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='399b93deaeacef939a45bcb3875de73b1a6bc7b52c3d4455b58e9b28cb97e6c57123b36706063fb743f423343b2d548f0b44e50aed4ad676f3370e713db24015' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    \
    ## Manually configure symbol link due to https://github.com/dotnet/runtime/issues/53543
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
