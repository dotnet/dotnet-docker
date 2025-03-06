ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM $REPO:10.0.0-preview.2-azurelinux3.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Install .NET SDK
RUN curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/Sdk/10.0.100-preview.2.25155.11/dotnet-sdk-10.0.100-preview.2.25155.11-linux-arm64.tar.gz \
    && dotnet_sha512='a526401a22f52fccb97ac59cc62cfea6dfb013f354dba3869c5938cf87cdb7491035e1a4da5beac2f1001370b8d214f07d8be8c44125e160278cde81fd4d1448' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz


# .NET SDK image
FROM $REPO:10.0.0-preview.2-azurelinux3.0-arm64v8

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=10.0.100-preview.2.25155.11 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Azure-Linux-3.0-arm64 \
    # Workaround for https://github.com/PowerShell/PowerShell/issues/20685
    DOTNET_ROLL_FORWARD=Major

RUN tdnf install -y \
        git \
        libgcc-atomic \
        tar \
    && tdnf clean all

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help

# Install PowerShell global tool
RUN powershell_version=7.6.0-preview.3 \
    && curl -fSL --output PowerShell.Linux.arm64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.arm64.$powershell_version.nupkg \
    && powershell_sha512='3818a8dff822db30072e61c9cac6f3bdaf7e6ca0efc8473c89f7547e47ce9c422841541b3ec08b5a9b1b346e465c8b81667b564c0ca5144e4f928609a9ec9ecb' \
    && echo "$powershell_sha512  PowerShell.Linux.arm64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.arm64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
