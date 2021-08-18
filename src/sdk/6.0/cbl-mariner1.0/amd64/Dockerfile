ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.0-rc.1-cbl-mariner1.0-amd64

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=6.0.100-rc.1.21417.19 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Unset Logging__Console__FormatterName from aspnet base image
    Logging__Console__FormatterName= \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-CBL-Mariner-1.0

RUN tdnf install -y \
        git \
    && tdnf clean all

# Install .NET SDK
RUN curl -SL --output dotnet.rpm https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-x64.rpm \
    && dotnet_sha512='34d9a9b4add9d7677bf51c5de48086c191b6eaf32f9cdba9472596174ed7514f2409549d54c7cd4910c178431cf1f02a0cc036ae3b09369c9ec21e9819ac1ae1' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    \
    && curl -SL --output apphost.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-apphost-pack-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='9f73b39f7f40be2e928b9b3ef69274759f4eca7dc3e201390cebd7c3195b6d4f172732a5b812ce55ef27c5f5ca0a4e6998da1bffa36bfa28f355466b54cb83cf' \
    && echo "$dotnet_sha512  apphost.rpm" | sha512sum -c - \
    \
    && curl -SL --output targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-targeting-pack-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='1a7915eccdb81b2a6eb7fc8b332e51711c6adc409fe20e2604b6c247eef53f5949ae9108edcf4a725c3105721e19ed472be25e517dcbdcff5930500a47491f7a' \
    && echo "$dotnet_sha512  targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -SL --output aspnetcore-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-targeting-pack-$ASPNET_VERSION.rpm \
    && dotnet_sha512='72d689c3f6fe82c60879d22179803c479b6c52ba76b9861ecf710d71a094640371723b4baf8cc6c42f87b2aae568fd82a4fa56ed879c4e35205038335c5d61ce' \
    && echo "$dotnet_sha512  aspnetcore-targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -SL --output netstandard-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/3.1.0/netstandard-targeting-pack-2.1.0-x64.rpm \
    && dotnet_sha512='fab41a86b9182b276992795247868c093890c6b3d5739376374a302430229624944998e054de0ff99bddd9459fc9543636df1ebd5392db069ae953ac17ea2880' \
    && echo "$dotnet_sha512  netstandard-targeting-pack.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm netstandard-targeting-pack.rpm \
    && rm dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm netstandard-targeting-pack.rpm \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.2.0-preview.8 \
    && curl -SL --output PowerShell.Linux.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.x64.$powershell_version.nupkg \
    && powershell_sha512='cf3b6415fdad38dd582c57b2df06a28528d6243af508626fb20de36116993fd2bb7e73a8869f07b787c73eb920c2559e8a2c2611173a26d2d227d36de08cbc71' \
    && echo "$powershell_sha512  PowerShell.Linux.x64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.x64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.x64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
