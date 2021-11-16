ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:3.1.21-cbl-mariner1.0

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetCoreSDK-CBL-Mariner-1.0

# Install .NET Core SDK
RUN dotnet_sdk_version=3.1.415 \
    dotnet_version=3.1.21 \
    aspnet_version=3.1.21 \
    && curl -fSL --output dotnet.rpm https://dotnetcli.azureedge.net/dotnet/Sdk/$dotnet_sdk_version/dotnet-sdk-$dotnet_sdk_version-x64.rpm \
    && dotnet_sha512='1ed0d86d822b15ebd588e0db198585efa9cb9adcecf77f661ea492cd2d3e78b3e6701838466ac96abb06da60de3cb6b7b3b18a44103f9e083accd5029191e3d8' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    \
    && curl -fSL --output apphost.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-apphost-pack-$dotnet_version-x64.rpm \
    && dotnet_sha512='dd974eadceb5e1faa44bbe95cc452847974bd128e85b2bdc1c0d0b4f581c048601017d50df663fb7ae2491aef4bb7cc493e3e48dc778e59bc15381918de6fd9d' \
    && echo "$dotnet_sha512  apphost.rpm" | sha512sum -c - \
    \
    && curl -fSL --output targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/3.1.0/dotnet-targeting-pack-3.1.0-x64.rpm \
    && dotnet_sha512='9111ef4a0083c1fbd969f5adfca8f7b0a2e90eada94771c68ca4d00d34756fb280de153ecce4ad3da4b16698b7add4848f5b6ec24b89a144c2a119a42fb27c85' \
    && echo "$dotnet_sha512  targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -fSL --output aspnetcore-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/3.1.0/aspnetcore-targeting-pack-3.1.0.rpm \
    && dotnet_sha512='fdab7122896ff3602d8a25c0b283996973ef0cf4f4d0f9eba429d3a93b2441ebe0e0d0b499b97bb28c43cfb386501cc617ca7ed7c949dd3e45c61db0d5879fd4' \
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
RUN powershell_version=7.0.8 \
    && curl -fSL --output PowerShell.Linux.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.x64.$powershell_version.nupkg \
    && powershell_sha512='2ad4d9d26af9bc5a0cd25b0dff2d7360a43a9f3bb29a7b9f4b27474f3897258dcc9b3bf6839b7f401cda2feefbc8f4ded2b15edb5ebf613b4ab26147dd9a330b' \
    && echo "$powershell_sha512  PowerShell.Linux.x64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.x64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.x64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
