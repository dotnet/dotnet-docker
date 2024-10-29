ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:9.0.0-azurelinux3.0-amd64

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=9.0.100-rtm.24516.6 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Azure-Linux-3.0

RUN tdnf install -y \
        git \
        libgcc-atomic \
        tar \
    && tdnf clean all

# Install .NET SDK
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='57e6d2e8391183939ab4e7c87ce4e9ad8ba37b52b7af09f1f138768fc22fb2970f6175ad4614252bb72ca9adfbdf7d5d90d8018cd376ec772b339b0177345fc6' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.5.0-preview.5 \
    && curl -fSL --output PowerShell.Linux.x64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.x64.$powershell_version.nupkg \
    && powershell_sha512='149934997170960f00f9df9720d000fc901fe40bc83399751dfeefbe514b5e2b4d1f76c8dfdbfaafbb7b5a2bdfc14e235055a9529d80d57312856ee5c8d54ea9' \
    && echo "$powershell_sha512  PowerShell.Linux.x64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.x64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.x64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
