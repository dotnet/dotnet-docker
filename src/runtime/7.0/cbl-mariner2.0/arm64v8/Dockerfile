ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.5.22301.12

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='5987037ff0ec44bf517e832e2e015ddb2fa649191fc444553fd38806a5215e76d327e9ada38dc46a68b41de92bd260034327d251baedeaebde5595231d304751' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='ad968c8d1d2b08605b5dba1b030c647cfe7d4a52a9f5f1057403c3998593f100d4814e5eeee912613560190a342b7bf1c6ae0f76153df6866beea05206ed1bd4' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='8b4b37c78b999cc26f30a9169d39c7a87e915b6ab38ef546296b3949a42864d8bfda081587a4114b3bdb7d892c3d37869c168ef9cebaeb1aafeee203212ec605' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
