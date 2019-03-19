FROM arm32v7/buildpack-deps:bionic-scm

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

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='815f6a77e4341941314f680a11030cccfaa0d8da67bd3f35e6e386c1b6a4b7b86c42ac27a0f4a49b9d00df8d9d7a966bfc207ad5bf523e5581c2f8b6376826b7' \
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
RUN curl -SL --output PowerShell.Linux.$POWERSHELL_VERSION.nupkg https://pwshtool.blob.core.windows.net/tool/$POWERSHELL_VERSION/PowerShell.Linux.$POWERSHELL_VERSION.nupkg \
    && powershell_sha512='0bb710ed0ffba267c6d82253abae5427a771861d6b50c3574dfe77d746f8d78d309397abbd1ff300d5b6b70a9d794943836822158d7de6095b61df3fba490c08' \
    && echo "$powershell_sha512  PowerShell.Linux.$POWERSHELL_VERSION.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $POWERSHELL_VERSION PowerShell.Linux \
    && rm PowerShell.Linux.$POWERSHELL_VERSION.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm \
    # Remove unnecessary runtimes
    && rm -fr /usr/share/powershell/.store/powershell.linux/$POWERSHELL_VERSION/powershell.linux/$POWERSHELL_VERSION/tools/netcoreapp2.1/any/runtimes/linux-arm64 \
    && rm -fr /usr/share/powershell/.store/powershell.linux/$POWERSHELL_VERSION/powershell.linux/$POWERSHELL_VERSION/tools/netcoreapp2.1/any/runtimes/linux-musl-x64 \
    && rm -fr /usr/share/powershell/.store/powershell.linux/$POWERSHELL_VERSION/powershell.linux/$POWERSHELL_VERSION/tools/netcoreapp2.1/any/runtimes/linux-x64 \
    && rm -fr /usr/share/powershell/.store/powershell.linux/$POWERSHELL_VERSION/powershell.linux/$POWERSHELL_VERSION/tools/netcoreapp2.1/any/runtimes/osx
