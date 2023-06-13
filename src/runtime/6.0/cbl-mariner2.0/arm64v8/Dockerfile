ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.18-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.18

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='cdb1cd7c543c4caa87a72d343d995d4a4abd1a5ae5d146f68fe47164c314b7e1304fab66217035f04b78d3ccc7ae461bce25d540a9205cd548f30ed5f7db5e55' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='626165e550ba161bfe05adc446b2643eec1d76cca827a58bd4d4db1ec1b18443a6921b0db5d33df1f10b2def763aea7b5d4fc9173d1a42599efbab23e27bac2e' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='fc3dd1b9cf4fe5b8a4def1e42c1c6f7d97a118d22a4bc82f40ed01b32d7140251719d0788a622a3709d080d97355f7bd71d4b53fef91c90c744f5c7c04ad44a8' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
