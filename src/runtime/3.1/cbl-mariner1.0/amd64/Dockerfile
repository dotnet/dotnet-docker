ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.19-cbl-mariner1.0

# Install .NET Core
RUN dotnet_version=3.1.19 \
    && curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='4ce5fed1d44e54e93da53d2c87c3f8010083736d86f9ca4b7afc4d0200f08b0fa1673a141f493e2b814ab50fde1b03a598f15b5cfdbe1bc3084086671e4a5d06' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='d0d0b06e59d3d906decab13881f4c467c739dbd41cb221b8ecff62006f10460ed57fb707e5e230b246b4e5344852031f00e7107ee7cc16dbf8d5a9e38ede60d9' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='7ca51abfe045859bb8cda5f29baa3c1db44ee02131961de3709203b4a31e50a45c5e2cc58220e5986b06e328803d106cd1a2d85f1b71f5fd28dff95cd93211c7' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
