ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.21-focal

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
RUN dotnet_version=3.1.21 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='cc4b2fef46e94df88bf0fc11cb15439e79bd48da524561dffde80d3cd6db218133468ad2f6785803cf0c13f000d95ff71eb258cec76dd8eb809676ec1cb38fac' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -ozxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
