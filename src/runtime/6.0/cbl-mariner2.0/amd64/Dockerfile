ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.6-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.6

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='715367ead38f50519db1ba6c1c820a74cc9e54ad31f9b125076db92a8d2d6aeb13eb9e4dcef57f90cb1aa2c523f2d377de8a2667a7e910db9a0c871f379badb0' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4285e8391d7fcd029bad9d2bd3a882f6282113ea5d69ea70f1180a9bc5c55287a1fcb6f95016c0a9f59556408de50af06d845b06fcd785a7152992f490034067' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3b0005328297ccf66a5209043f1a2e9684772bd61230aa2a4d15948bc30c5a29b2b8ab9222091cf31c9d360db2f0d387c25d66defb78d12a806dcda251780649' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
