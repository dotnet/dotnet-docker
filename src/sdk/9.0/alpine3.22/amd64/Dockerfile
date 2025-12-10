ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM $REPO:9.0.11-alpine3.22-amd64 AS installer

# Install .NET SDK
RUN dotnet_sdk_version=9.0.308 \
    && dotnet_version=9.0.11 \
    && wget \
        https://builds.dotnet.microsoft.com/dotnet/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-linux-musl-x64.tar.gz \
        https://builds.dotnet.microsoft.com/dotnet/checksums/$dotnet_version-sha.txt \
    && awk -v file="dotnet-sdk-$dotnet_sdk_version-linux-musl-x64.tar.gz" '{gsub(/\r/, "")} $2 == file' $dotnet_version-sha.txt | sha512sum -c \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet-sdk-$dotnet_sdk_version-linux-musl-x64.tar.gz --directory /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm \
        dotnet-sdk-$dotnet_sdk_version-linux-musl-x64.tar.gz \
        $dotnet_version-sha.txt


# .NET SDK image
FROM $REPO:9.0.11-alpine3.22-amd64

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=9.0.308 \
    # Disable the invariant mode (set in base image)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Alpine-3.22

RUN apk add --upgrade --no-cache \
        curl \
        git \
        icu-data-full \
        icu-libs \
        libatomic \
        tzdata

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help

# Install PowerShell global tool
RUN powershell_version=7.5.4 \
    && wget --output-document PowerShell.Linux.Alpine.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.Alpine.$powershell_version.nupkg \
    && powershell_sha512='bc8d9677d49d625663e885461e74fdce1218d7e39da9bcd3cee7ea6bda488f34f92d0734263dd1bdbecdb47425130ae97e96be89658f70eebdab88dd75852252' \
    && echo "$powershell_sha512  PowerShell.Linux.Alpine.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir --parents /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.Alpine \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.Alpine.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm \
    # Add ncurses-terminfo-base to resolve psreadline dependency
    && apk add --no-cache ncurses-terminfo-base
