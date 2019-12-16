ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.1-buster-slim

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
RUN dotnet_version=3.1.1-servicing.19575.1 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='696eb8a9b4f7cf3347a58381c156304027dedc8ee0bba418e4d3971b3fc9bf09163295b49bbefb89e56c62d99383b065ffea382e242984e3b57bf0ce92cb5ab7' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -ozxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
