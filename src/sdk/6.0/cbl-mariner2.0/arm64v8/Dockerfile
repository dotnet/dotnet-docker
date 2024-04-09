ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.29-cbl-mariner2.0-arm64v8

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=6.0.421 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-CBL-Mariner-2.0-arm64

RUN tdnf install -y \
        git \
        tar \
    && tdnf clean all

# Install .NET SDK
RUN curl -fSL --output dotnet.rpm https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-aarch64.rpm \
    && dotnet_sha512='721fd9b2b21db1ed20dfe4b66b0c3bfd72109bb666546d086516d4347f7507fe834051b2dde75ae1ca14c3504e2e799d4e1c37bc48de50d95c2ed1245d40acb4' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    \
    && curl -fSL --output apphost.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-apphost-pack-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='233899e6abcda69251ca5774ab568e06368240ba02b2627f317d75c657214d30d395e6b3d3b5ced71e714ec0b3a4c76b846826d2f53d27f39359c8ad7135a55b' \
    && echo "$dotnet_sha512  apphost.rpm" | sha512sum -c - \
    \
    && curl -fSL --output targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.29/dotnet-targeting-pack-6.0.29-aarch64.rpm \
    && dotnet_sha512='723b6ccf8515a506eb63ee81331e76363e85e00d57b28f6be1de1b27643124f0f626c20d34ffbac4536048989fd07a0b63ee1e6a050db1fd5019ac50812e8255' \
    && echo "$dotnet_sha512  targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -fSL --output aspnetcore-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.29/aspnetcore-targeting-pack-6.0.29-aarch64.rpm \
    && dotnet_sha512='d5c510d9bcd6a8bb5ef71fc662fbaa2331b9d515b6c2aa0cafc5ab23ee93582513b2b997af9b66b60703f8bf3ad822f2479b971e27efb16fd6c2e3bc44ee6488' \
    && echo "$dotnet_sha512  aspnetcore-targeting-pack.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm \
    && rm dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.2.18 \
    && curl -fSL --output PowerShell.Linux.arm64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Linux.arm64.$powershell_version.nupkg \
    && powershell_sha512='75012b9a68188a94a491a26f6bcfc62858f5764182c310e4ae9ba1fb203c5ae0b7f87e6d1ac196928b5d144b8c2753df1f09514d2b09045c6ed1a782bd27d0fd' \
    && echo "$powershell_sha512  PowerShell.Linux.arm64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.arm64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
