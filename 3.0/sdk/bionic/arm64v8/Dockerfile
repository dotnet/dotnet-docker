FROM arm64v8/buildpack-deps:bionic-scm

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu60 \
        libssl1.1 \
        libstdc++6 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 3.0.100-preview4-010965

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='6d3e1f2abefeab5bfe1e8cd12161b4e1e4005783c33b79274fe078735ecb191e6a974b2a401a48daabde8fa4569a1837b85135c733364994314d18e9604edcf7' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd
RUN dotnet help

# Install PowerShell global tool
ENV POWERSHELL_VERSION 6.2.0-rc.1

# The global tool package PowerShell.Linux only supports linux-x64, linux-musl-x64, unix, linux, linux-arm64, osx runtimes.
RUN curl -SL --output PowerShell.Linux.$POWERSHELL_VERSION.nupkg https://pwshtool.blob.core.windows.net/$POWERSHELL_VERSION/PowerShell.Linux.$POWERSHELL_VERSION.nupkg \
    && powershell_sha512='0bb710ed0ffba267c6d82253abae5427a771861d6b50c3574dfe77d746f8d78d309397abbd1ff300d5b6b70a9d794943836822158d7de6095b61df3fba490c08' \
    && echo "$powershell_sha512  PowerShell.Linux.$POWERSHELL_VERSION.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $POWERSHELL_VERSION PowerShell.Linux \
    && rm PowerShell.Linux.$POWERSHELL_VERSION.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh
