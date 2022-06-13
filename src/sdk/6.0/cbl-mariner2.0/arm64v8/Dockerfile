ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.6-cbl-mariner2.0-arm64v8

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=6.0.301 \
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
    && dotnet_sha512='43b9d366182ad996a7ae3cab3629548d0688eeee451e616691e97fcd0e0acbd0ffe25e83f46e9a518d0b4898904a49651a66d8a4d4c2174ae7ecc8050221c9a8' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    \
    && curl -fSL --output apphost.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-apphost-pack-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='d3288107333c81bb958c85e6d247ae52a0e42f5ff67ec083c3a53560f0b32e5aad3c70823647b2ab7bc675e16adc06f7a58d3000e6278f9c0107d63f406088d6' \
    && echo "$dotnet_sha512  apphost.rpm" | sha512sum -c - \
    \
    && curl -fSL --output targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.6/dotnet-targeting-pack-6.0.6-aarch64.rpm \
    && dotnet_sha512='dc78bc3c8b43ff7d0acac6d70ac9ea38d4ffbb720683cb22cdbd76f448c2600b6e1f2a4ed3d37c8fd7d8ce7505edb8c5fa51099064b078004e9e85744ab48ef7' \
    && echo "$dotnet_sha512  targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -fSL --output aspnetcore-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.6/aspnetcore-targeting-pack-6.0.6-aarch64.rpm \
    && dotnet_sha512='2888f361728e83428fa9a0d54bc27269ed870459962eb49624e6267f3dca882ef7804d9da5f99a6ed17ecf8e596e6b45b7587975cec078c100c6a224e38a0c43' \
    && echo "$dotnet_sha512  aspnetcore-targeting-pack.rpm" | sha512sum -c - \
    \
    && tdnf install -y dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm \
    && rm dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.2.4 \
    && curl -fSL --output PowerShell.Linux.arm64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.arm64.$powershell_version.nupkg \
    && powershell_sha512='ef6ef8f9486adb635e2e6a5be4c0bdd5df4bd753afacc60b2f1889e666f7848c2332008a72eb18d9cfa3dc30277c138551af494fd7729733d30417967f20a1a3' \
    && echo "$powershell_sha512  PowerShell.Linux.arm64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.arm64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
