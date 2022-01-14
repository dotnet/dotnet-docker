ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22062.8

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='efc5ea27fcc9d17dfd1b27ff11ebf995f4b424c67a59c49c9cc29aa5a6fc8a6ebe364b92f36a9ab9cb5c0fa60a1438790b48b9981346a2d5ec09d1e0e17497fd' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='19171f8d8aa57d8e38d3c2004a02b3c8dd44cf7ddbb7dd3ba0b79a387629680a6ac1a166038f8f5679f9c50858a571c8d97f6def90f57166d5052c748c0b230c' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='0162f9751edbfc37e354fc80c8f10bdddb91ba994b11f1691867828ddfa2cace7bc658c4b06b66fbbc1217468c207d8e0eb8f6bc2d7c3b4826c1f16d14b8da3a' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
