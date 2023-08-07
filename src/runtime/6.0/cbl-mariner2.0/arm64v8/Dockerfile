ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.21-cbl-mariner2.0-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=6.0.21

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='624729f50122962d449697c048c68ad233ac8bceefe18da9e1e5ba54e0f101591902d4b26b11cad4deaa76cd29d0f03c88e08644a1bc5f318a6afc3f4cd5ae2c' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='723f14da4dcf90b2234793e637bc94a533e6693ab015689f3e6373b9f22438fd4c8394ca80a73b0888b4287642629cb30d07a1dd3cb212ac5a6847ccb0eef1a4' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='ef066649d5d344cc8b9923017ad10230c12680136e903a8aa8f125d219a8ae5b1d6566f2bb02e6f6f246f7b36fda8941ea704811bf25069e3618b0c6191fdf36' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
