ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.12-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.12

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='fb196ff9767d191a65661f910c06337b98e9cf2d2452f22047334bb60d6f532f4b4f5c4105244685622f96d4ca98a42d1d5da2663370bdebc1371a6e70dbaac1' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='62163656aae67b50efb02a67d6959619ebe867b97ddca99ebef80a865b620c6bf2cd4415ffb8774acb6d960fb8d8a5ad25db12e1ec1a34864c236867e5fc2361' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='e32e2a5b456127e8ae93b1f2561d0acc59094c47635f52aeaea0fa3712559065463a3563d312650abb9024b16134e1b2de16e8f3b2ecaf650bdd22ba6b39c9c0' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
