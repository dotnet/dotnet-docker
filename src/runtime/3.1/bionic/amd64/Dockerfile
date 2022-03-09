ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.23-bionic

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core Runtime
RUN dotnet_version=3.1.23 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='5f9b800a4ee13c4dbab98348f97010211f8bfd412cafeebb89972beebb2fd69c4b7c03971683e025577696a6d943f2cbcf0b7dba766eed846beb1281cbe14182' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
