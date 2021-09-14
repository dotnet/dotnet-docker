ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.0-rc.1-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=6.0.0-rc.1.21451.13

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='d1b3782911a6ba7a4c331e0b191851c7cc71c70fdfc393600fb27aaff4f23ff67630ee5d2cc3a4f4128a52a7b36354e84d373b5132946f324c5d0b77517982c3' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='f2053745fc35560968b7f3a1cdf7c8b24d92e4d574b72a63551e674bef901609437275c8495304bfb1756363ba434b7b046aee4a01bfdcbd7af80fd15eaa038d' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='7bc354baf66b8e0443365275ac756acd089276b531750d55f14917c7f71665c8e3ae71cbdb88f8fc081bfe708bf482aaa31bb5b3d24b78f231bcaaaf7e29c23a' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
