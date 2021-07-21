ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.0-preview.7-cbl-mariner1.0-amd64

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=6.0.100-preview.7.21371.2 \
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
    && dotnet_sha512='3b7aeb49694b83d2d4244966ca5d8ee499b51dec14cbc833f0b673987cb8fe4638fc65424a93d6818c06f5a2253487b47b9cc92e66b2b155bae49058811e7902' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    \
    && curl -SL --output apphost.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-apphost-pack-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='1a9b8bf20f97017a8c3b60b5b0141945037cfc16dd3b1fcc84e3f302e7bac187439e98c4e1b4c94ed5bd86ff41001deaa539bcfa56ad06d5d3fc63c06df6e94c' \
    && echo "$dotnet_sha512  apphost.rpm" | sha512sum -c - \
    \
    && curl -SL --output targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-targeting-pack-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='bce49d13593bebe182b17dfbebebb9872fc96b9f80e8f38214c1d6b0b66d218af8658b863e7efa3a829a3ca5679a88ccd95973eb06badb2e47180333fd4354fd' \
    && echo "$dotnet_sha512  targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -SL --output aspnetcore-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-targeting-pack-$ASPNET_VERSION.rpm \
    && dotnet_sha512='56c735fabf235941878a834014aabb6aeb4e288fe020bf0790eb3fc56137f991ac32e889acbf0cc742f5e932532018a83bc04084b1a88a62d1676e9ad7fa81c6' \
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
RUN powershell_version=7.2.0-preview.7 \
    && curl -SL --output PowerShell.Linux.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.x64.$powershell_version.nupkg \
    && powershell_sha512='c436f2c646c8aa36d8552465d0388ce08919362bb0d074c5c7761dc7750e3749165fc01a5d37aa1f6fad751fb98560b2e96b7b0137e6b5dee9a5bf44823c9877' \
    && echo "$powershell_sha512  PowerShell.Linux.x64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.x64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.x64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
