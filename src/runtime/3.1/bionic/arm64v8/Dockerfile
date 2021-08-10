ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:3.1.18-bionic-arm64v8

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
RUN dotnet_version=3.1.18 \
    && curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='0a106e73e3f4e1ce76ef8f89eab3f951ce999b0efa7a8c4ea2995cd89fe33566f1ee7868304c8e989da96e94af79ff128284911916f3d0d0f0421a86e8d74e54' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -ozxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
