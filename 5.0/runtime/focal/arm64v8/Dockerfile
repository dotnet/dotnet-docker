ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps

# Installer image
FROM arm64v8/ubuntu:focal as installer

# Installer pre-reqs
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Retrieve .NET Core
RUN dotnet_version=5.0.0-alpha.1.20072.3 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='d7c75c4104d2f04a31a2c23146fbc4d46a7630aadaf9c9fba41052672607a910d9d5f358f3e977694fcc8aa602d3acd843c5c32d7e10bf49534d3dbf0960a2d0' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET Core runtime image
FROM $REPO:5.0-focal

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
