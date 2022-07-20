ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.7-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.7-servicing.22322.2

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4fd756f686c4566f1abb4cc1b3da9a6367c3aa7d5ec6ca21adbae814a95cbee34155889259476e0c4a7948e385d7d5766976f2df3eba5daf3c769028ce808103' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='55419089fdbd263a6da7bdbdb8ad809ae47fed1a1588d122153f8147fd92f733d4b734cd9ea35dbfc3b6b1097f69b88f30492d556eac28f6598f75636f0016ae' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='944e7340677a7d0a7fc926abadedf7261ac2c37d0dc5684d0c212a4d85641176a5a4b25519fcd7f4db97fc0e7b993b4236c15b82238c5769bffed4e384e2577e' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
