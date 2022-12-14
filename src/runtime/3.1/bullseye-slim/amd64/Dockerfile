ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.32-bullseye-slim

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core Runtime
RUN dotnet_version=3.1.32 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='a1de9bbc3d2e3a4f5f52b7742c678b182a58a724d36232997511e390027044d60144a7e010a29d6ee016ec91f2911daef28ac5712a827fff8bdde73314b7e002' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
