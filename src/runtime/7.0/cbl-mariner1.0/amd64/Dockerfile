ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.2-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.2.22074.8

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='b860a04aeefefa6aa84cfea16d63ccd19057dc41b42cd554858048c66ea1c54c57eccca875a155fd5413277f59bb56a3740b5261a7e3e46bd560d30a6f271521' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4c2827d03c99b25abdded03fe1c51bc5007bf57d87f3f5c7e925a26056377e530628eca1f177f344ff2917162cc14b355b985c447541a6dbe97c6f1880a43825' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='c429ce9edb57b497e2d449fed055763065f66900dc70dd6e8b9ab848701e58109f15d48fc880e97cdeeee754b5b664a91c73630d5acaa5b68f266d1e33ee5d67' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
