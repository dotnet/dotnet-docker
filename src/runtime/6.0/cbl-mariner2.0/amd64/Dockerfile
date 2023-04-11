ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.16-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.16

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='49e612c66611207c46849a0a734a32cbd1837afca29fe2542fa7625c920fc393aaf12d6708744430f49378fb91c327f0f73383b007d5eed6d693047ab689a76e' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='100eaacd4512e83f73f613e9689a14dbe73c08b585f08465cf1a07022d968c8514eb950fd952277ba35c6e57c02aabb3657d8c985cc86b119641c3b9053c60d4' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='95ad1fb69138d9ef36c7db7c8d2a81bf7dd5dbce8e9489b8333d252a583b8ca1cb2afdabe0fbe24420d512ef11dfef1c16f869c97bc68f8c357cbb39a14366d1' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
