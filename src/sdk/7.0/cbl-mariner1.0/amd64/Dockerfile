ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:7.0.0-preview.1-cbl-mariner1.0-amd64

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=7.0.100-preview.1.22110.4 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-CBL-Mariner-1.0

RUN tdnf install -y \
        git \
    && tdnf clean all

# Install .NET SDK
RUN curl -fSL --output dotnet.rpm https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-x64.rpm \
    && dotnet_sha512='dfcabae935cc3dbd663bde9a6234665fa4d2f4142c0989e042f6a259a97cafdd0a6757c3422be5c92cb91a4e177ef7f8d282c92ccc014d9d58944af853252720' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    \
    && curl -fSL --output apphost.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-apphost-pack-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='07208f773a734f7428c7c128314cd8c7f6b6b5f1e078b7e202ba9d2343f50a4428207de595dd267d35d968edff91b666efa3b75f1676f2b7a6dbce7c2b9beba5' \
    && echo "$dotnet_sha512  apphost.rpm" | sha512sum -c - \
    \
    && curl -fSL --output targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/7.0.0-preview.1.22076.8/dotnet-targeting-pack-7.0.0-preview.1.22076.8-x64.rpm \
    && dotnet_sha512='0e0d8bdf92494e6c7d74faaa6a78017b4658fba5d15f78316393ced4df188df67dbec62fd465b53314294145d15b9f2fea4f6a6f8b734971a12ef9eaac6db42f' \
    && echo "$dotnet_sha512  targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -fSL --output aspnetcore-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/7.0.0-preview.1.22109.13/aspnetcore-targeting-pack-7.0.0-preview.1.22109.13.rpm \
    && dotnet_sha512='c4fcb30b8be0c81e5be651289af7d1b34e4b5837c5d1b188d67459e1f893b8da7d6cbdaf5850a85efc9ae6049d397214415048cc58a8450778cde3f25dc2ec34' \
    && echo "$dotnet_sha512  aspnetcore-targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -fSL --output netstandard-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/3.1.0/netstandard-targeting-pack-2.1.0-x64.rpm \
    && dotnet_sha512='fab41a86b9182b276992795247868c093890c6b3d5739376374a302430229624944998e054de0ff99bddd9459fc9543636df1ebd5392db069ae953ac17ea2880' \
    && echo "$dotnet_sha512  netstandard-targeting-pack.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm netstandard-targeting-pack.rpm \
    && rm dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm netstandard-targeting-pack.rpm \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.3.0-preview.1 \
    && curl -fSL --output PowerShell.Linux.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.x64.$powershell_version.nupkg \
    && powershell_sha512='3119d35e2c436f67820c5c184393c3a8f56f6c1928e8b287cf038098f9b6caae851ff9fdf8c6cbff72e8b4e14622498e104247a7d04fa3369a3911b67f4fefe7' \
    && echo "$powershell_sha512  PowerShell.Linux.x64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.x64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.x64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
