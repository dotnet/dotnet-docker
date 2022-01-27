ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.22-buster-slim-arm32v7

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core Runtime
RUN dotnet_version=3.1.22 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='2b189ab7fa7c2a42c155a270c59ec149b78194aa4c84c2e35b236e757ae73b7d0055bcade6d45e208f00e391fecb04fa5f71ef51a6a0851f29949517d33086fa' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
