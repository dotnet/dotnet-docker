ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.2-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.2.22152.2

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='5a9fa3714449309c056ff3dbc957b568069d2f2bf071f03d047907384670b5de7762a8e48e233820aa27d01de6b8aa11eceb76584fa53ecebb2f8039856ee4d8' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3312f2eadf03d330e58ed0e1cfeea16c99f02319225e52ad346c3b161f4e2e059a141947971e80f4d0ba5125d6749e0776741627d1870f262fb6d8fc70bd6a1b' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3e3616449024270df957c104ab51045ae8296fe146fad01aef34a7338afa164d25b87d8f444d39acfeddac4ee39d4994d21ece351ef2894d94e8bd3cd2cb7bf5' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
