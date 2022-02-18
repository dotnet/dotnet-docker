ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.2-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.2.22116.9

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='22d478979482623c65a606c1638e72575fb359acf67b902d93f912cc6cbe93d0e3b016c9aff027ec751ec6ac418159bff2021bd157aa5429f0693fcbb4ce1c0c' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='20a18d3ad68474ef7a748dc42a704d10408f34dc64cae84b272cc123db3eb7c47e2d4dd37fba4034eafd88b5115c4b7a07f4042171c67955972364bfc888daa4' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='20778c2c94662fb50284f9ec1f4d137e240de22f02bdb973775e4caf2a872a29652171c4cb725fb302582b6902e6691fb93e92514c2cc290ab52438f417cfc66' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
