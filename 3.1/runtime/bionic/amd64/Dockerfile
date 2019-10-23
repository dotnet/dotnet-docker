ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.1-bionic

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
ENV DOTNET_VERSION 3.1.0-preview2.19522.1

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='0f0ccc1def93c4b2a2c68efe714db3fd00db2d1c65a6a2783895ffa42da714762053bbd085427c74bdc66733072a2f6a3566e8c8abfe3d05cd82b6e0223749ff' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
