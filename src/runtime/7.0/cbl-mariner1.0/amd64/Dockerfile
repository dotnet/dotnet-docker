ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.2-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.2.22128.3

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='61c16126f18039f0397da102f3227a7ac14750377effb4c0ba83d5b231c9851c5ed38155e161f28da6d88978bfb3b24a862788d01ad95b6ac3cad7eaa9a94705' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='8826ad74c53bfc19449db36a7b2bf792cd49cd064d1c04750c83d3b627a5dbd2059d814082895a8f640b0130108b1028d6a1014324b0b37b852a8e1def9e0f4c' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='d7585c4ed8240bf3c0bdd7fbbcb9c98ee3777d8fa72f52dba08415bf249d0519be46883c5a7b0c67b98d0f20d40b233f4ca1c7594b57b415cee730c3fd69d34e' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
