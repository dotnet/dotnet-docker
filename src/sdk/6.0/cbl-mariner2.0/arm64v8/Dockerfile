ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.7-cbl-mariner2.0-arm64v8

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=6.0.400-rtm.22364.21 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-CBL-Mariner-2.0-arm64

RUN tdnf install -y \
        git \
    && tdnf clean all

# Install .NET SDK
RUN curl -fSL --output dotnet.rpm https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-aarch64.rpm \
    && dotnet_sha512='c14c0a3fdeb5ea0ff52b3d698610d355c2fbcf42da079058e28802395cbeb24e60248a23d6eb0a84ef2a0c7e8a2b1d5383d4f1c22c6053964329ea8d79aa701b' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    \
    && curl -fSL --output apphost.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-apphost-pack-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='70e0edc816ad7378be0f1b0956b5336f3af5e30d52b98f027379833a2b8fa94704c3a04ec9d3284868b3aaecef2a492aa86a01842c5fbb2f6460c1a15f58c9ef' \
    && echo "$dotnet_sha512  apphost.rpm" | sha512sum -c - \
    \
    && curl -fSL --output targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.7-servicing.22322.2/dotnet-targeting-pack-6.0.7-servicing.22322.2-aarch64.rpm \
    && dotnet_sha512='8a83cf8327ef96a340c1b47d3d201c1ce5e4f87d9ce398c39d0b810b9485b32286c47c2bc426b75f0f0b03eba9c2724b8993ece9d418b3b206399d3179cfe5fd' \
    && echo "$dotnet_sha512  targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -fSL --output aspnetcore-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.7-servicing.22322.5/aspnetcore-targeting-pack-6.0.7-servicing.22322.5-aarch64.rpm \
    && dotnet_sha512='c1295e9babc2543f141ff9a37bb5f7efea98ebfb160de53cfdc3b46e450bda095b990895dc3fe63ffe7ac59fd9c204e1d930781ebc0c4d26cd8cb926a8a22f1b' \
    && echo "$dotnet_sha512  aspnetcore-targeting-pack.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm \
    && rm dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.2.5 \
    && curl -fSL --output PowerShell.Linux.arm64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.arm64.$powershell_version.nupkg \
    && powershell_sha512='4bf8ef286559979c47eec1613088afc907025e5b323183bbeb99f59bbb8e41739387ed12082b192104236f8e0c5064266a71ce5f56c9430031763d7efd120148' \
    && echo "$powershell_sha512  PowerShell.Linux.arm64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.arm64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
