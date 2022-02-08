ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.2-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.2

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='a41d032118dc8db13a5a7e6c40f479efe472cdd77e0a1acbe0c0b5f1f74e17cadcef489332be0a15bad7b3541e6e79d588231c1c244603047f6c37109d18703c' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='0c1f9a7db8c0caa07ce3cb5bf376da55e8ede9f83af1f2723ee0f66afc1fe111e3a0f1e65deca199760b5650d25cde46855c52ab558e1520ea752da301883f7b' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='c9af474bedb97874fab9b29cabdd858379fd947bf9d0f9b3d1773a635cfb255169a36afed3cd7ab79a8ef5077cf4806a9e75263ace85ddbf81f509e834ccfc3b' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
