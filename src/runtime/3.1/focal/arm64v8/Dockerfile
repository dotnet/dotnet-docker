ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.24-focal-arm64v8

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core Runtime
RUN dotnet_version=3.1.24 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='8d6a47104097e0872f2685046d3a26a5d63cb27a4b65b5510d8b41861eace611f75c585d9a818f985cb7d929d7f174acd5b9ed1147f97a82c6579eadd645ccca' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
