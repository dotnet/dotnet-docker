ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.15-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.15

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='3714d921eabc8833461ed4bbb2a40cb85cdd1e91e6a0eb4a62c6ceeedb984962159542beb87b36fe957b1aae3f6e865d1fcdffc67e44259eea3515591a09eabf' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='de061946dc159a1a4d6e35e06ee0e6f7b55e639a186df99235d2639a7198826887b75579c6e9f220e3e08d2f94aa3da3f4f4696c144a5ba1a253eea3c4d72826' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='ca562f433fa802d7754705323aa5a58fa8b20c47685724f5aa5bfb051867b9a303b606703932b00d516bb726e8156876855111cc269ddbc13a2a89d1846b8fc9' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
