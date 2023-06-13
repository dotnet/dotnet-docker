ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.18-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.18

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='8b2bea7337a7630b3bda136c22e6390e53cd90a58f71327a5d840ac6552ed7288c22f5d57aab811695aa9e11ac25028bcba11c1d8f726e0b83bdc2aba718ec49' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='6518cc4a08fd72cc67158fb3287ac897fc9f1c6d907eaccaebcfbd806c6c150e038062f0f94066d45f9512db3dff2fe6a7b75b958886a3e140909f009bd27da5' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='69ed055f36b1ea33e36515d98f65b8f90deee4bc340d9c79e806424153ee07eef5b74119bfe00b899f66e812a0d35820c646358f65dd323deaa7925753c42a7f' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
