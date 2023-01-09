ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.13-cbl-mariner2.0-arm64v8

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=6.0.405 \
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
    && dotnet_sha512='0f69bea6143ad4be69e0acb632362329bb097cac4f32794330daed2d18c39586dd1a20425e80090d27dbea783ce07bbee0c58707ec491637107db60709ec7653' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    \
    && curl -fSL --output apphost.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-apphost-pack-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='87c114efbcbb319ecce473d0789e4b076cd40c78f1d035c48dc741d13523d625e52c7b87498c58f84ca4468b1ba5edc8e3677da144fea6a02956859f83e4bc1e' \
    && echo "$dotnet_sha512  apphost.rpm" | sha512sum -c - \
    \
    && curl -fSL --output targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.13/dotnet-targeting-pack-6.0.13-aarch64.rpm \
    && dotnet_sha512='bd6195bf08d274654fed735678a7beae5b4940da5600ca5ace19cb64777b72229788490b1899485d2349732bf01538e07bce4f3588dd92c69017830ee17b467b' \
    && echo "$dotnet_sha512  targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -fSL --output aspnetcore-targeting-pack.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.13/aspnetcore-targeting-pack-6.0.13-aarch64.rpm \
    && dotnet_sha512='6bcb05e5a37c44781c24b3b1079164c3ea21c2ebf3d635ace81ffb6a2582f61b36cbfbb06aebb0d26b515ea9ec9e16cbb7a020719df6b234034ca9786a03167d' \
    && echo "$dotnet_sha512  aspnetcore-targeting-pack.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm \
    && rm dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.2.8 \
    && curl -fSL --output PowerShell.Linux.arm64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.arm64.$powershell_version.nupkg \
    && powershell_sha512='927f70c059295597892369869b548b000aa581bac2e34a2de801db6f5f3c6f9ba5af719ac5b01bb10ec6b54f4a4d81e755f12d0185bfdb1b82611b5f15774205' \
    && echo "$powershell_sha512  PowerShell.Linux.arm64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.arm64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
