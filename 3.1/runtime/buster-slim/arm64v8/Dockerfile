ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.1-buster-slim-arm64v8

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
ENV DOTNET_VERSION 3.1.0-preview2.19522.1

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='8015a113f0ef7dc8d730b722ce2d2ed9190f9b55000675335c08005a845548c3ef43b7523dca14be7f14ec4ca1bc8369368c77a9177bc9d4fb5420f172a8b8ae' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
