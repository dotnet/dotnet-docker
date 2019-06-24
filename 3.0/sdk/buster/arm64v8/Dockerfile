FROM arm64v8/buildpack-deps:buster-scm

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu63 \
        libssl1.1 \
        libstdc++6 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 3.0.100-preview7-012589

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-arm64.tar.gz \
    && dotnet_sha512='23e1dcfc49fc7b91cd4ec242537e845a6bdfccbd88caf081e2683b8686a6f43b7307aced5fc634bde7ae34c2da610d1cc437be893e6ec5a999d9193eb1f8a57c' \
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
ENV POWERSHELL_VERSION 6.2.1

RUN curl -SL --output PowerShell.Linux.arm64.$POWERSHELL_VERSION.nupkg https://pwshtool.blob.core.windows.net/tool/$POWERSHELL_VERSION/PowerShell.Linux.arm64.$POWERSHELL_VERSION.nupkg \
    && powershell_sha512='339c118f49d0699113b471f12ca62ca3a1bf511ab27ad6fd4dd6c9df4d9f8403c9a38da11c79310fa257be7cfcf76e7e291668460bf70199a0fe77ef54e62b3a' \
    && echo "$powershell_sha512  PowerShell.Linux.arm64.$POWERSHELL_VERSION.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $POWERSHELL_VERSION PowerShell.Linux.arm64 \
    && rm PowerShell.Linux.arm64.$POWERSHELL_VERSION.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
