ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.2-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.2.21470.23

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='c0aa7869013d54a07954e800e3198e9b1c5705ba7b72a68edfa9c755e77bcd99ddfbe7068176a62277bd166b7866a3a1e5445fce91e7f08bab286b7a06a7cc5c' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='54f936488d53dc5b533344c13a458c8ff433a6a4bd3734a4720af368868506a440c862ffea5f7c2f53931497e6656c925f0377d3c4a1a92528a76edab6523653' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='1b2fac18f93db0f7b13ea8fbf82f99b9b4950f0fa51f8d81610cc5e601c2ea2c5c24ec742f2c2ac5f02c1b72c75f2521fb1deeeef1cce879df63f9f485beb3d0' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
