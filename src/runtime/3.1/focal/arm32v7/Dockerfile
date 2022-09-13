ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.29-focal-arm32v7

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core Runtime
RUN dotnet_version=3.1.29 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='d378ae9be1c899aaf7e0c06926eeeced0c1857b9ebd1f0b8fd0ad6a9550fc3c430c3a016e7a009cb111750cbbb0ac7cb36bb4fe48efbd6400e9aaf1493984c17' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
