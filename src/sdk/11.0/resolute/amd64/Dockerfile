ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM amd64/buildpack-deps:resolute-curl AS installer

# Install .NET SDK
RUN dotnet_sdk_version=11.0.100-preview.4.26230.115 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-x64.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-x64.tar.gz.sha512 \
    && echo "$(cat dotnet-sdk-$dotnet_sdk_version-linux-x64.tar.gz.sha512| tr 'A-F' 'a-f')  dotnet-sdk-$dotnet_sdk_version-linux-x64.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-sdk-$dotnet_sdk_version-linux-x64.tar.gz --directory /dotnet ./dnx ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm \
        dotnet-sdk-$dotnet_sdk_version-linux-x64.tar.gz \
        dotnet-sdk-$dotnet_sdk_version-linux-x64.tar.gz.sha512


# .NET SDK image
FROM $REPO:11.0.0-preview.4-resolute-amd64

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=11.0.100-preview.4.26230.115 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Ubuntu-26.04 \
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
RUN powershell_version=7.7.0-preview.1 \
    && curl --fail --show-error --location --output PowerShell.Linux.x64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.x64.$powershell_version.nupkg \
    && powershell_sha512='7622aeae7e62a4bbeaa7b80f09c3dec63e35f209d920a956b08265351a8af63ae42770a4461c985dd1dd7f4f977dcb1ebf35ac0b9768fdb7d86889dbb1ff1eeb' \
    && echo "$powershell_sha512  PowerShell.Linux.x64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir --parents /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.x64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.x64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
