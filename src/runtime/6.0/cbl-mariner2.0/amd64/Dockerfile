ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.27-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.27

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='f7d9cd7f40d0d6f335b18fbd09da987455dd7e44c191b089da0ed6babe25a401dc9508b35bc6d5ee55349e158540f8f134bad48fb8ec21614fdcfeffaedad622' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='593ff92c77232a8965bb9051aa2e73e7e59ea232d019cc613de99da532c39afaebcb512468af21b3ff62eeb05f707a0cde85427ed81cb4ec85f502e65a28535f' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='58693863963293873ca0d91771f995db82a4b6e22e22de438c9f520815f81f68bab9e6ba23f144190ff9b2cfcb746c7fc80b62fdd2f78412193c074db22d4051' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
