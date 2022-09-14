ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-rc.2-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-rc.2.22451.11

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='e5ad57fb2b043909fdda115c6ffa95abf6f6c6fbd0c35906464de59cc3ec9f880364561595cc29d6067d320b438164fcb6dc608bb898530793245dfd8879384e' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='23c948ebeb2b48f8b86bf359b774a537bee964aeb9471b06b280d8143e96deada4db56f4e11496a9c19c68a1f11ed09ca0aa93f5a96cca9828620ceb5d5258a6' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='8387e935689ac2b101ac3a71999736f6c7fc0d3356182687025704ef415eeb149a727256f5aeee563cb475927eec46438f944cbaf6354689ad503253a54f4237' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
