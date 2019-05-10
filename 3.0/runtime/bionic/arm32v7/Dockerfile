ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.0-bionic-arm32v7

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
ENV DOTNET_VERSION 3.0.0-preview5-27709-08

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='6eb70d1ef8e3b32fcb0841f7db89d0c607b4a2c666fc37ef897a97bfa5cb24ea2c49099b03518618deee01eb8866092aef144d87ad4ce1daeb47135a010a725e' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
