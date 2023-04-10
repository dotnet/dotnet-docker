ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.16-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.16

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='cfdcfff7515caa0eae810b200324c8b1686e916c7ffad6fc427fff5adcef1872617ba32121c6ca6226caa0888381cba25e4153dbe9fe81f1d59f4c8aea1b2898' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='655731b2cf7c6a553952c392d505b31247d49774f3aed991deb1973fd9eff44fec04cc90f730988060e4e115a413645e97df543f49442f590fd5ca390a521661' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='c167b001844dcd06c81fc7415a4a3cf9f26f235df88e1fd1b83557bc6e8db0cd96013424378035954c9591e9d195abcaf076171bac79ad5762207b49de6325b9' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
