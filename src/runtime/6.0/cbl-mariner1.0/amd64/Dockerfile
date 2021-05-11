ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-preview.5.21276.11

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3179f15672c56baaeed98fe587322ce1d199c16746fadea6535e2347d895d9cae69b198c9df4257536a017c1fe02510f6c84a0448435b3b907d2d970d2e913b2' \
    && echo "$dotnet_sha512 dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='5ea6a5496817191dba4804dd08fed371dd926ebce6c9a96a0d483d56cda80d042627e24931ed95a969d4ae48c43b71cbf578d1a6ee5e49f13fdaa144a9ad08eb' \
    && echo "$dotnet_sha512 dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime-deps.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-deps-$DOTNET_VERSION-cm.1-x64.rpm \
    && dotnet_sha512='2d8838446ca2f0913e456c7a3cfc382186b8b2c172170757f461b8d02baf8d60fa1f83174f78f1dc427a9880e98a6eb4841f02bcf2059c96ff4cf98175784f7e' \
    && echo "$dotnet_sha512 dotnet-runtime-deps.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ce1b083752dbdabb77e65508fcac03eb28537eba24c387adfa711867b0a6a7fee3ff33b0282b5585050319d81e4dcfa8263bd90166ec58e1d08372289e07b204' \
    && echo "$dotnet_sha512 dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime-deps.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime-deps.rpm dotnet-runtime.rpm \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
