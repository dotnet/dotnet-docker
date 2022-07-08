ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.6-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.6

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='b06b269352f784e9bac778279ceef79c794481829183b03b40ed5745824819a0223ce75322406a14ad2e5ecfde25f2af9d2d1268320c591afb257dc5eab83d5c' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='7533ec385254a2a545b6bcbd9f0ded7d239eedf98d09e5a4d3dca77982139eeffda40130257107c84982025d86290431ec357825da1f2ce580dcaa8c8b57da55' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='96556c12057b8ca927aa3c2976cbcc505e31c5c46f0b7abc01d71d471d6080d20aef0e6479515142f45e93ddc099b1de414b4c044da57af7aebacb4ec81d6b16' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
