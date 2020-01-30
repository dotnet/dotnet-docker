ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:5.0-buster-slim-arm32v7

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
RUN dotnet_version=5.0.0-alpha.1.20070.4 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='6ac8d72c06bba23ccf43af6602a2cf81c18219ce9e362ce4be8304276347c3ba45399e29224a91c2639a72f01599bcf0a74d4c22f1daf18a131ddb1fdb1e41d6' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -ozxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
