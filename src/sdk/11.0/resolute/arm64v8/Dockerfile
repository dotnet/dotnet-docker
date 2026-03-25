ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM arm64v8/buildpack-deps:resolute-curl AS installer

# Install .NET SDK
RUN dotnet_sdk_version=11.0.100-preview.3.26174.112 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-arm64.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-arm64.tar.gz.sha512 \
    && echo "$(cat dotnet-sdk-$dotnet_sdk_version-linux-arm64.tar.gz.sha512| tr 'A-F' 'a-f')  dotnet-sdk-$dotnet_sdk_version-linux-arm64.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-sdk-$dotnet_sdk_version-linux-arm64.tar.gz --directory /dotnet ./dnx ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm \
        dotnet-sdk-$dotnet_sdk_version-linux-arm64.tar.gz \
        dotnet-sdk-$dotnet_sdk_version-linux-arm64.tar.gz.sha512


# .NET SDK image
FROM $REPO:11.0.0-preview.3-resolute-arm64v8

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=11.0.100-preview.3.26174.112 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Ubuntu-26.04-arm64 \
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
RUN powershell_version=7.6.0 \
    && curl --fail --show-error --location --output PowerShell.Linux.arm64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.arm64.$powershell_version.nupkg \
    && powershell_sha512='21efd12f1eb232d95fb2d08f07d3afb8ca9e5b77992b28de149394ad444357ab3f37cc571443dafb0d9ac09b2d59c1fbd682b8c3630f0b24726d6c85690f542b' \
    && echo "$powershell_sha512  PowerShell.Linux.arm64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir --parents /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.arm64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
