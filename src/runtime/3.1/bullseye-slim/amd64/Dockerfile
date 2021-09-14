ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.19-bullseye-slim

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
RUN dotnet_version=3.1.19 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='2cf268cc13bb4739203a18a9160aac58f2088e8ec275b1f7ef2fe2b74bc16edfdfbeb886a74fc9421edbf2899fa9e3ee7ea2734a83b1800b4a9848fb7e10bbbe' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -ozxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
