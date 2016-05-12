FROM microsoft/dotnet:1.0.0-rc2-core-deps

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
ENV DOTNET_CORE_VERSION 1.0.0-rc2-3002702
RUN curl -SL https://dotnetcli.blob.core.windows.net/dotnet/beta/Binaries/$DOTNET_CORE_VERSION/dotnet-debian-x64.$DOTNET_CORE_VERSION.tar.gz --output dotnet.tar.gz \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
