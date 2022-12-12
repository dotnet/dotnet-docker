ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.32-cbl-mariner1.0

# Install .NET Core Runtime
RUN dotnet_version=3.1.32 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='bb226ffd7f3483e6cca8c7fe1909a5b47e8eb4ded5aefa00d4578bbe1ca3f08a1b963d1addcc6beff92acdba658691804536db35b51d3ab146dbab4c020f8821' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='75c2bdf665a814977f48f1926f7fc72ff0cc32d459fc55d7a328726d919d0d8c4669582fb46fac359dbdb308819c7f8b6a9646c16abe568a5b001593ccda9171' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='7da987a078374bdbd704acce731698797e80c6ca8affdf7ddb027f075cc09da96dd5095156ca32179d4739db608c4b692b1991ed2e51a24d9aae3c643ccaf043' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
