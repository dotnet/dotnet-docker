ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.31-cbl-mariner1.0

# Install .NET Core Runtime
RUN dotnet_version=3.1.31 \
    && curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='79bca60d3c4237df2adbe3915f9b52464d189e77cf01f6109f1d719140e7e2d69f127559df59aa23ea930ad9099b5317263a83707187636eb480bff132c70404' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='c3a76ba884e49344718e57a859e5a3f53644a5a835014a42294b6c930f146a6d8b78954861bb00c5adfe6cde3791e41bce1fcbcc80eb18c7425fcab6f513f304' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='c12f23704e7901564215c09ec30e837fda79d072d889acf8baed9d73aad521727c6c140175269339646b69939ec2d9c066a464e405f4cd52fd35c00d348f799a' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
