ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.32-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.32

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='7c90cfcba46d0b3c299df9918cc64ede1fe47c44f0fd13e60315e80dc24af88b6bc0464619c07b9389b19e0315fcf008a86da59e6e8d4512365fdbb9afeb5d23' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='455935b33bf0ee1c5c1cffcd73fbc691b3d8b7d9b7d8130de1fe32857c5f36715007d816d85013c78cde1e7993e59bbddcbdd40213a9089e0fb68dc18cb1619f' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='d2218cf2cf99959075146ab0f01b6203c1ae5d3ea648bbef1d385cd985df0e00fffba5d55bc0df7d660cdddbffb8bae024c70a97c6f177fa0ff47dc49ce37fc7' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
