ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.17-cbl-mariner1.0

# Install .NET Core
RUN dotnet_version=3.1.17 \
    && curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-host-$dotnet_version-x64.rpm \
    && dotnet_sha512='cf672eace64fa3832cd01523167885fedb573c93eb1b7b763bc54683e935b26476ff0df412ac70c892846c6f7b7b55e68adf202c3d46299bebb5a98270d5f51f' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-hostfxr-$dotnet_version-x64.rpm \
    && dotnet_sha512='789cf75d037df3a9a4779913d8bb8c3008fec1a062e245fccd8737c7fd2a97a21ac370696782a1da28292eefa79eefb24090ba14d66f42f10d530dcb9175d008' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-x64.rpm \
    && dotnet_sha512='19fb5492545ed96584944d5ef7d6f5865214b5ed51991610bf488de2f09800b7e2fe167f1f86935e14d236cd2d3a28f5beba93ad1d5861cb8aba133a3dcfd01a' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
