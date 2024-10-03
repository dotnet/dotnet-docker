ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Install .NET SDK
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/9.0.100-rc.1.24452.12/dotnet-sdk-9.0.100-rc.1.24452.12-linux-arm.tar.gz \
    && dotnet_sha512='f31a4a2c3080a921cfdd71933d1f57c2f57ff4c43f5a0ad6f52640bc791e54f8c0526d8e1206ad21f8682357a53cf6d488a8b01107e7c34beafe2c8c3425dd8c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz


# .NET SDK image
FROM $REPO:9.0.0-rc.1-noble-arm32v7

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=9.0.100-rc.1.24452.12 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Ubuntu-24.04-arm32

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
        git \
        libatomic1 \
        wget \
    && rm -rf /var/lib/apt/lists/*

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help

# Install PowerShell global tool
RUN powershell_version=7.5.0-preview.5 \
    && curl -fSL --output PowerShell.Linux.arm32.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.arm32.$powershell_version.nupkg \
    && powershell_sha512='667203585a5244d4f4197aa737c30f854d727001cbea5330afad6a414b8a8623fa6a65c573e810117f71faf33de04c973007abc2ddade5f5e4df29d062841aba' \
    && echo "$powershell_sha512  PowerShell.Linux.arm32.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm32 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.arm32.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
