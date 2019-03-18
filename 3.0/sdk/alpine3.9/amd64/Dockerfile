ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:3.0-alpine3.9

# Disable the invariant mode (set in base image)
RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 3.0.100-preview4-010965

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='23b25afb4ba64049482be6f9e9812de484e8b01bcf5b17c0c990561c4f9bfd029c0425592e610d71150567b10d1b9325889b09690f98ac2b4b2f143610a45ac1' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz

# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd
RUN dotnet help

# Install PowerShell global tool
ENV POWERSHELL_VERSION 6.2.0-rc.1

# The global tool package PowerShell.Linux only supports linux-x64, linux-musl-x64, unix, linux, linux-arm64, osx runtimes.
RUN wget -O PowerShell.Linux.$POWERSHELL_VERSION.nupkg https://pwshtool.blob.core.windows.net/$POWERSHELL_VERSION/PowerShell.Linux.$POWERSHELL_VERSION.nupkg \
    && powershell_sha512='0bb710ed0ffba267c6d82253abae5427a771861d6b50c3574dfe77d746f8d78d309397abbd1ff300d5b6b70a9d794943836822158d7de6095b61df3fba490c08' \
    && echo "$powershell_sha512  PowerShell.Linux.$POWERSHELL_VERSION.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $POWERSHELL_VERSION PowerShell.Linux \
    && rm PowerShell.Linux.$POWERSHELL_VERSION.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    # Add ncurses-terminfo-base to resolve psreadline dependency
    && apk add --no-cache ncurses-terminfo-base
