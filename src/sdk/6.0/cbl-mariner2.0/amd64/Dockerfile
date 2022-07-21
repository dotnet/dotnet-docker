ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.7-cbl-mariner2.0-amd64

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # Enable validation for signed NuGet packages
    DOTNET_NUGET_SIGNATURE_VERIFICATION=true \
    # SDK version
    DOTNET_SDK_VERSION=6.0.302 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-CBL-Mariner-2.0

RUN tdnf install -y \
        git \
    && tdnf clean all

# Install .NET SDK
RUN curl -fSL --output dotnet.rpm https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-x64.rpm \
    && dotnet_sha512='0d725fa33fb2c025a3b0e1db760946fae3d1dea8c27ba122e45f99e9e7e1732aec85a8bdec9e124f115915249864b7779969440c1f1ec70a691b56b26dbbcbd4' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    \
    && curl -fSL --output apphost.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-apphost-pack-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='45aa393ffc0e3dc04aa9d03ad1ed33d65d26a2667c48807423272d6da262a2f6d2ed94cf234671ce88bd5669d15cf1d2eab9cd6ffeb6aabf3fbe53a0aa430ed7' \
    && echo "$dotnet_sha512  apphost.rpm" | sha512sum -c - \
    \
    && curl -fSL --output targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.7/dotnet-targeting-pack-6.0.7-x64.rpm \
    && dotnet_sha512='b8c585fc3bc80f06387ea0eebac85a356b6763a9189b8d61dd5f4a40361932a4691d4ce77926f5e365ea879dbfe66af22a07f9e967d8b47687b0e8752a81f9f3' \
    && echo "$dotnet_sha512  targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -fSL --output aspnetcore-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.7/aspnetcore-targeting-pack-6.0.7-x64.rpm \
    && dotnet_sha512='2546a6b51bafde4cedab5998e637b5b5dc4cdaae5b50fc5af58ee2c4db3f80a27517136a835b78f8c0e9cf1b5f53dbf7aa51ae653131c1b5ee2fae4f492303a2' \
    && echo "$dotnet_sha512  aspnetcore-targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -fSL --output netstandard-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/3.1.0/netstandard-targeting-pack-2.1.0-x64.rpm \
    && dotnet_sha512='fab41a86b9182b276992795247868c093890c6b3d5739376374a302430229624944998e054de0ff99bddd9459fc9543636df1ebd5392db069ae953ac17ea2880' \
    && echo "$dotnet_sha512  netstandard-targeting-pack.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm netstandard-targeting-pack.rpm \
    && rm dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm netstandard-targeting-pack.rpm \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.2.5 \
    && curl -fSL --output PowerShell.Linux.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.x64.$powershell_version.nupkg \
    && powershell_sha512='459fdc6efff0a5b38d1485b28ae97b39352011a237cbd0addd48390aa4caef87db25e8caab7c55b8e050b95f772df0712a3b29973504fcbae10df17aa6773af3' \
    && echo "$powershell_sha512  PowerShell.Linux.x64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.x64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.x64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
