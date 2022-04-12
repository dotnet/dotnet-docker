ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.4-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.4

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='2fffc4142a17f290b7ee6d5db0a4afe82e091a0a7605e427afc5e78ed41f27446fc6d71a5d2b7a9bfa4ac491309070dbb2c352f0be68a082ad7b8fc6927ac66a' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='425f4a1d738a3066e0f1f6ce5f9849065bfca0bddc26064db566ce6049e6911b8324d38317720a75baf12b29d3a20994b39012643ec3c7d88eed1208830e8aee' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='30ec48a161b8c77ee8c288228cd0819445fd53f093656d2f18dbb31a1a912a8dc096aaad615e3729c5d3434067799a00d9ca731e9688a9371997ef8bda3de4ea' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
