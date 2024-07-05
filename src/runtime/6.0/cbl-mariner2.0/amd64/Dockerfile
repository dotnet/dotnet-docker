ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.32-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.32

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='1d50617d6baaccfc330655f6fb0513d80519e175923b7f077072a1e0abeec1c0174d5435eec8de5c67befa2a8b52d2dacdc247a2c8d2234fc05e26f8fa1df58c' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='7e0e5036aaf9bc00ee8eb24ddc130e5b2fe0c26c11febf5068633897d5000f19c2b54c5a3136ad3c70f1a382efebdac5d054d5677179e036c46907ecb02850f2' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='66adc9fd1beca298e4100e8871f368ee71af59d7ea9c71930c60bd017a70be0732e9ea798769d8750951501e336ba72d4b172c7533ef1cd77b514f4be8d831f0' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
