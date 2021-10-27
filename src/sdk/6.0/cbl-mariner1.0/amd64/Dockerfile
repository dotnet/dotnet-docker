ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.0-cbl-mariner1.0-amd64

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=6.0.100 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Unset Logging__Console__FormatterName from aspnet base image
    Logging__Console__FormatterName= \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-CBL-Mariner-1.0

RUN tdnf install -y \
        git \
    && tdnf clean all

# Install .NET SDK
RUN curl -SL --output dotnet.rpm https://dotnetcli.azureedge.net/dotnet/Sdk/6.0.100-rtm.21526.10/dotnet-sdk-6.0.100-x64.rpm \
    && dotnet_sha512='a2c91cc9e3dfad9be50c6c0b9e4e99fda045a65daf04b6f688505dd3ed7e9ec0878565ff9acbaab614dde0a5f9dae5379cbaa6b76fda8a25f8310aef821d15e4' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    \
    && curl -SL --output apphost.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21522.10/dotnet-apphost-pack-6.0.0-x64.rpm \
    && dotnet_sha512='3b34180892ebf853036074fc40f1c33dbf35588c4b16f311ef10447a0a4ae68459182bcbdce0b639180d5d7780d18fc1c683ca3f3d207b48cbc0d1bc3d915b56' \
    && echo "$dotnet_sha512  apphost.rpm" | sha512sum -c - \
    \
    && curl -SL --output targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21522.10/dotnet-targeting-pack-6.0.0-x64.rpm \
    && dotnet_sha512='d66a3f413d80503f92bdf9b74defbb35c08e06d4ce5279ee8f473aa59bc2cbab74b431d44af21dbe6af5b5b9f667d08ba19378e53e776023bc74625e59d58a46' \
    && echo "$dotnet_sha512  targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -SL --output aspnetcore-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21526.8/aspnetcore-targeting-pack-6.0.0.rpm \
    && dotnet_sha512='2ff4d759145cc82b3de6855af69b67d1d7e5cf379fe408e5072be0947b9fccfd991e42f47453c5f95c9eef1a520c0eb9aa352d58b6a08c4963764cbdd14bf6b3' \
    && echo "$dotnet_sha512  aspnetcore-targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -SL --output netstandard-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/3.1.0/netstandard-targeting-pack-2.1.0-x64.rpm \
    && dotnet_sha512='fab41a86b9182b276992795247868c093890c6b3d5739376374a302430229624944998e054de0ff99bddd9459fc9543636df1ebd5392db069ae953ac17ea2880' \
    && echo "$dotnet_sha512  netstandard-targeting-pack.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm netstandard-targeting-pack.rpm \
    && rm dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm netstandard-targeting-pack.rpm \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.2.0-rc.1 \
    && curl -SL --output PowerShell.Linux.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.x64.$powershell_version.nupkg \
    && powershell_sha512='571006ffaab8eeb88bccbbf886c180b55639ed60bf99915cd1a22868dafa22f96a123f710e7d42f9bf504b2fd0fd2b37b31c657381cb8bc57cd83eab0052ba3d' \
    && echo "$powershell_sha512  PowerShell.Linux.x64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.x64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.x64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
