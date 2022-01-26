ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.1-cbl-mariner1.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.1

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='1cab3cd9148543d511eff5b8bc71cf86201120d29a5eaa5a84d129e38e6c24d994b71b18989f9d0676526ddc79547acafd21505595392c8b6aef45f23adbd27e' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='dec0ab1b43ba557861f9380a28d7ce51edc6f69650e455ca6d5d97af9cc98212772cef55ee3d0625d0caf1533d27a74166abe28b4523db31a278b83382f5acf6' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='962edf038bb583ee2529c425153d276f353509ad9292676e5b63c081a65b7475e3b44c82ae95089a996931a213c1b080ba3e9e05b054b3d7d79a763ef92766e5' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
