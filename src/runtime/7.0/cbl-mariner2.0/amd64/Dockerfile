ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.5.22301.12

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='7f242a194d58a149e5b0a45577d5fa1a42134fe46e99e4e96227c19a848c2399a896f951c292e75c186dabc75818741ef7fcb651e084efcbb0c4fa134c012184' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3d2743aaf0aef04c6968a7193b1a3475692707338adfcee01b82d61e9bf370730c2a20c4ef8ad4696824c2d910b85e9c90ee6f320c6e1a6f82e3c6ca541e699e' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='7151e5f3afd41c01d91af7e4fd946ecd848bbd96f4eb24cde3eaf912d7b0a75c2fb831e90fbb5839006d384f9e1afe9517bec4ecf7023862275775f0b890a71a' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
