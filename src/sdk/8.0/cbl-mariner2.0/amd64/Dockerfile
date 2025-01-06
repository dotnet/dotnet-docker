ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:8.0.11-cbl-mariner2.0-amd64

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=8.0.404 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-CBL-Mariner-2.0

RUN tdnf install -y \
        git \
        libatomic_ops \
        tar \
    && tdnf clean all

# Install .NET SDK
RUN curl -fSL --output dotnet.tar.gz https://builds-int.dotnet.microsoft.com/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='2f166f7f3bd508154d72d1783ffac6e0e3c92023ccc2c6de49d22b411fc8b9e6dd03e7576acc1bb5870a6951181129ba77f3bf94bb45fe9c70105b1b896b9bb9' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.4.5 \
    && curl -fSL --output PowerShell.Linux.x64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.x64.$powershell_version.nupkg \
    && powershell_sha512='3dcdb54cdd40540211668825e2fddb09f49af3f372a3cc0b6f0fa60a7cd2e4a84310c9361fc3d92ffc194e87a00ae2ce6efded5ac714ff019f0850e6b82108b4' \
    && echo "$powershell_sha512  PowerShell.Linux.x64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.x64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.x64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
