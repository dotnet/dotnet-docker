ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Install .NET SDK
RUN dotnet_sdk_version=10.0.101 \
    && curl --fail --show-error --location \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-arm.tar.gz \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-arm.tar.gz.sha512 \
    && sha512sum -c dotnet-sdk-$dotnet_sdk_version-linux-arm.tar.gz.sha512 \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-sdk-$dotnet_sdk_version-linux-arm.tar.gz --directory /dotnet ./dnx ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm \
        dotnet-sdk-$dotnet_sdk_version-linux-arm.tar.gz \
        dotnet-sdk-$dotnet_sdk_version-linux-arm.tar.gz.sha512


# .NET SDK image
FROM $REPO:10.0.1-noble-arm32v7

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=10.0.101 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Ubuntu-24.04-arm32 \
    # Workaround for https://github.com/PowerShell/PowerShell/issues/20685
    DOTNET_ROLL_FORWARD=Major

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
        git \
        libatomic1 \
        wget \
    && rm -rf /var/lib/apt/lists/*

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dnx /usr/bin/dnx \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.6.0-preview.4 \
    && curl --fail --show-error --location --output PowerShell.Linux.arm32.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.arm32.$powershell_version.nupkg \
    && powershell_sha512='32e410abb44dcd6ba943fa8c7367a88a6c0c4e3ac0a67756fb5919139c980f1a3d66a3b055e6b2292a154c9eede22003b0b85605c101e3e6fa89fd71b7d44a7f' \
    && echo "$powershell_sha512  PowerShell.Linux.arm32.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir --parents /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm32 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.arm32.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
