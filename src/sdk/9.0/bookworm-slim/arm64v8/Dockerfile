ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM arm64v8/buildpack-deps:bookworm-curl AS installer

# Install .NET SDK
RUN dotnet_sdk_version=9.0.316 \
    && dotnet_version=9.0.18 \
    && curl --fail --show-error --location \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-arm64.tar.gz \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/checksums/$dotnet_version-sha.txt \
    && sed -i 's/\r$//' $dotnet_version-sha.txt \
    && sha512sum -c $dotnet_version-sha.txt --ignore-missing \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-sdk-$dotnet_sdk_version-linux-arm64.tar.gz --directory /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm \
        dotnet-sdk-$dotnet_sdk_version-linux-arm64.tar.gz \
        $dotnet_version-sha.txt


# .NET SDK image
FROM $REPO:9.0.18-bookworm-slim-arm64v8

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=9.0.316 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Debian-12-arm64

RUN __netiso_apt_srcs="$(find /etc/apt -type f \( -name '*.list' -o -name '*.sources' \))" \
    && __netiso_backup_dir="$(mktemp -d)" \
    && for f in $__netiso_apt_srcs; do cp -a --parents "$f" "$__netiso_backup_dir" || exit 1; done \
    && for f in $__netiso_apt_srcs; do sed -E -i 's#([[:space:]/])deb\.debian\.org/debian([[:space:]/-]|$)#\1debian-archive.trafficmanager.net/debian\2#g' "$f" || exit 1; done \
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
        git \
        libatomic1 \
        wget \
    && for f in $__netiso_apt_srcs; do cp -a "$__netiso_backup_dir$f" "$f" || exit 1; done \
    && rm -rf "$__netiso_backup_dir" \
    && rm -rf /var/lib/apt/lists/*

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help

# Install PowerShell global tool
RUN powershell_version=7.5.8 \
    && curl --fail --show-error --location --output PowerShell.Linux.arm64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.arm64.$powershell_version.nupkg \
    && powershell_sha512='5b972bda947d4762974ecdde770e4f8bb55deab6b8ae2e04d9d25e17662670432bbecceab7fcc584fa01e34dab355bbb58bc156a2cecfab9931030baa172f6bc' \
    && echo "$powershell_sha512  PowerShell.Linux.arm64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir --parents /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.arm64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    && chmod 755 /usr/share/powershell/.store/powershell.linux.arm64/$powershell_version/powershell.linux.arm64/$powershell_version/tools/*/any/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
