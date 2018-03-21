FROM buildpack-deps:bionic-scm

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libcurl4 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu60 \
        liblttng-ust0 \
        libssl1.0.0 \
        libstdc++6 \
        libunwind8 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.300-preview2-008380

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='7f226afb1b8144b3f688491adebf3b27a43cc0155ba5ed9ea530efbba0626ad12fd7b5f62f2e62dde215d257473780e840aa0ba0bf57a08e28d51fa9a1e8bd5b' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Configure Kestrel web server to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80

# Enable detection of running in a container
ENV DOTNET_RUNNING_IN_CONTAINER true

# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER true

ENV NUGET_XMLDOC_MODE skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
