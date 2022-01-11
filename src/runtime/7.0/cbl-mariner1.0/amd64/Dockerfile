ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=7.0.0-alpha.1.22059.2

RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='e90c984a707953fcbb1453223f51dbc002eb8ff962c848daf243cef6a68b096bc14f00627ea686375654849a41615f3ffe43064d96425f06e91b1b88b20f7bd8' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='94d1007d8c07d19483798d1dead8e4642432b59f39f467a4a5989a58af30c1e7bb4e1d15b72d1c20860692ab7789e98ccce0283c08506497776c93fc697abf1c' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='a122b8772d1c2341dc2d316c512a41d8e61b691e32c4bbe4e0ba322fc496f2b98f3cd451d0646508a8de9375113d8539a1ac8fd39d60d6a1112a43b114ff4bbb' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
