ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.10-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.10

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='414985749572323d733dbf246348f8541f9f60f5b6a7fd527b49642ade1d1504227b2c0b8a1dcdf0875428323745760e2f3b7f521a86bc7d91eb4578524b8f45' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='2c4e495c3e3b5359163f6fcadd75754defc95cca3d1e65ee52e4e53e9037788b76a8e0c9e63b4f6a89a7dd52c7395c822815126fef899beb4361b5e941b55b2d' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='2e453f22ce5e880628813432c252d32d8ad011cd4d79e0e70089aa07cb48f8f4fa5d8f9b60901d1c9d034667a3e630b46e384854cc9198bab5040a5abcaedf87' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
