ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.24-bullseye-slim-arm32v7

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core Runtime
RUN dotnet_version=3.1.24 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='30e2453419f0bc84d08db0436ed924fe2633bafe1e7a7513d3bda939e6c201b2387351d5f8724e4964f3d760d336c771312988c94b1f145d74a706e414f166f1' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
