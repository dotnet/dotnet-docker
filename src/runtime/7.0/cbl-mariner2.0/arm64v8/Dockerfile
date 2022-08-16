ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-rc.1-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-rc.1.22411.12

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='15d2251a9aa7f92be6aee2e99641ed325d0d8aaaec4ff9bb5cf421afd7ecc2101d8e3b855b246d404e9a2bab65375781e10595412de6e735e085ef04cdbc6f96' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='c58a6d7534b78240cebe32e08155f144526762d4e44bad07acbd2172b4330d214d3827ef07255dfcbed9f0aa3370f3f0c0d8acb69d29af68ed3ee6c24a672b67' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='4ceb6496deb2e38b5dcb7a628cde6d426d8482fbf3b18ccf0e4b880843d896d4b87bd6d7442866e9297ca5f31453c0b2499494d2058feea6f21fc37ba8820cf9' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
