ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps

# Installer image
FROM buildpack-deps:buster-curl as installer

# Retrieve .NET Core
RUN dotnet_version=5.0.0-preview.3.20206.14 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='8bfa41cf27e1a38f32600e7276e834c967be77d3a4bfb8b52b7dbadd4cb4db2341e829ae0bcc1a4934c0ae9fa160f535933d4b9c1216eca90ed03705a2fd273b' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET Core runtime image
FROM $REPO:5.0-buster-slim

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
