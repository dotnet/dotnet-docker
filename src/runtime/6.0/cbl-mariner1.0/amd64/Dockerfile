ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.1-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.1.21451.13

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='d7ae604c1dd92ba18377b04343440fa94becb6ac9edca441a7aeac6ebe8a98af10a34eb046fdde67c03f8b3708987630db2373b2cd55e02684e0cbc4a772df9e' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='6c46f76d592c376a84d23dab9164568dfbf97aa1c447ede03d461dd56eeee1750393dbd61dece1f1f3f2df6a81189704423ca041290a1a670cc0cf7e8aa217cd' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='7d3d86864eb2219ead0b4b8b6440a91ece7f5800e445fdf334f18f40834f14dbdc565db08a14a4bb43b5036882a20849d57a37acb7ba617bafdc437a92e149cc' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
