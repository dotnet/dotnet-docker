ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-amd64 AS installer

# Install .NET SDK
RUN wget -O dotnet.tar.gz https://ci.dot.net/public/Sdk/10.0.100-preview.3.25181.22/dotnet-sdk-10.0.100-preview.3.25181.22-linux-musl-x64.tar.gz \
    && dotnet_sha512='2e335eb3a37d8e487a12e9bcf96caa6c3fd183f51b75fa6f48f81b6a512b9fd7024da84a712761fa40af99d6b346b2ca002de4e3ec6787a8371ff8838a8133b2' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz


# .NET SDK image
FROM $REPO:10.0.0-preview.3-alpine3.21-amd64

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=10.0.100-preview.3.25181.22 \
    # Disable the invariant mode (set in base image)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Alpine-3.21 \
    # Workaround for https://github.com/PowerShell/PowerShell/issues/20685
    DOTNET_ROLL_FORWARD=Major

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
RUN powershell_version=7.6.0-preview.3 \
    && wget -O PowerShell.Linux.Alpine.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.Alpine.$powershell_version.nupkg \
    && powershell_sha512='31694321fc30490d8f29e58c2b36df1a38442bcd9ca8e105c27765004a963c3331d185d8bba6631e6cebde4c3a120e354dab9fd7c9446064ae5b297fc7d6e950' \
    && echo "$powershell_sha512  PowerShell.Linux.Alpine.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.Alpine \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.Alpine.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm \
    # Add ncurses-terminfo-base to resolve psreadline dependency
    && apk add --no-cache ncurses-terminfo-base
