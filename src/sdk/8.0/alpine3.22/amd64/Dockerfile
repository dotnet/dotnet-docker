ARG REPO=mcr.microsoft.com/dotnet/aspnet
# Installer image
FROM $REPO:8.0.17-alpine3.22-amd64 AS installer

# Install .NET SDK
RUN wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Sdk/8.0.411/dotnet-sdk-8.0.411-linux-musl-x64.tar.gz \
    && dotnet_sha512='d4621c198b6b9fab8a7f3e9422d7377c2671880d8f104f6cd5bab0165f082d1f2f0a341d3305bbce7df0fcdff696c061b4eaaad1a7a84f08c4b8c1fb85d650be' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz


# .NET SDK image
FROM $REPO:8.0.17-alpine3.22-amd64

ENV \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=8.0.411 \
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
        tzdata

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help

# Install PowerShell global tool
RUN powershell_version=7.4.10 \
    && wget -O PowerShell.Linux.Alpine.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.Alpine.$powershell_version.nupkg \
    && powershell_sha512='06bc91beeb0d2ade83100432ebd54ae121a4fdca77080fbbfd9355a8f3e33c196c90385557bc82b5f0fcb1941cf369b56f08643448dcdd2271c61849ed9499b8' \
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
