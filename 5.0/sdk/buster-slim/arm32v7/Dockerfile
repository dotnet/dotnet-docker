ARG REPO=mcr.microsoft.com/dotnet/core/aspnet
FROM $REPO:5.0-buster-slim-arm32v7

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SDK_VERSION=5.0.100-preview.4.20229.10 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetCoreSDK-Debian-10-arm32

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        curl \
        git \
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu63 \
        libssl1.1 \
        libstdc++6 \
        wget \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='187af255332989d1d80a4c381118cdf3533bb6734ef40399e2039435a624ea7815cc38a5fc7fbf690faeb0a07454b7562d262d28edf925e3e05456c44bf4213c' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz ./packs ./sdk ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.1.0-preview.2 \
    && curl -SL --output PowerShell.Linux.arm32.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.arm32.$powershell_version.nupkg \
    && powershell_sha512='3bad721b2f0c72fd61772c0efee16f288174ae1396763ac88a970e5a002c533576375caa6ecbcf4049d5a3f9f8eb0ed5b43440501c1a03fa8283c834022b2d20' \
    && echo "$powershell_sha512  PowerShell.Linux.arm32.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm32 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.arm32.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # Remove the copy nupkg that nuget keeps to reduce size.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
