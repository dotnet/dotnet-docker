ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps

# Installer image
FROM ubuntu:focal as installer

# Installer pre-reqs
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Retrieve .NET Core
RUN dotnet_version=5.0.0-alpha.1.20072.3 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='9aff5d596ae8774bbf77110d89cf4cb062d3b3a8084b3dbbedb834b9b97b780265677f9324508c3b916a80c5dfa35c634a9c4b4491b19630a93ce7db0e67103a' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET Core runtime image
FROM $REPO:5.0-focal

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
