ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.17-alpine3.13

# Install .NET Core
RUN dotnet_version=3.1.17 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='aca9ff3a2d3105eed366816fedee8fa0d3c06f0c1081a9df5ad6e7638e540299d4452faefd42518de2023fccb4169127883c0f30f202b3b1a6ac9a68aa9e9efd' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
