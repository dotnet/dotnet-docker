ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:7.0.0-rc.1-cbl-mariner2.0-arm64v8

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Do not show first run text
    DOTNET_NOLOGO=true \
    # SDK version
    DOTNET_SDK_VERSION=7.0.100-rc.2.22423.10 \
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
RUN curl -fSL --output dotnet.rpm https://dotnetbuilds.azureedge.net/public/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-aarch64.rpm \
    && dotnet_sha512='7dc59f09bfd318f904dac5bd858510397c74c9cbcb6b84c47e4378860eee9543c82e9fdb80ce6cef926176784e7a0e29b99ac2e61799df65978babdab7b863a1' \
    && echo "$dotnet_sha512  dotnet.rpm" | sha512sum -c - \
    \
    && curl -fSL --output apphost.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-apphost-pack-$DOTNET_VERSION-aarch64.rpm \
    && dotnet_sha512='f008bf2a36a0f59781c4e75d177403d7eb9f3c4f9b0fc4a1f72d06eae79b9885b626684ded5d6b3adc3b1f8f55c4cbdb5295261ec2824ff19f8f382f7e8b556d' \
    && echo "$dotnet_sha512  apphost.rpm" | sha512sum -c - \
    \
    && curl -fSL --output targeting-pack.rpm https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-rc.1.22422.12/dotnet-targeting-pack-7.0.0-rc.1.22422.12-aarch64.rpm \
    && dotnet_sha512='74b6fb17ac89ace5e19f5539972bc3ab9e8ab2bcf82267337e459ae48cb0b7b53d9a852dc9908eb634874c7024bbb3c482bc595c85c93eebfee53afc3698c4aa' \
    && echo "$dotnet_sha512  targeting-pack.rpm" | sha512sum -c - \
    \
    && curl -fSL --output aspnetcore-targeting-pack.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-rc.2.22423.1/aspnetcore-targeting-pack-7.0.0-rc.2.22423.1-aarch64.rpm \
    && dotnet_sha512='2d7848cafa2281b54cc7eebab5318c5f85c82e2b07abdeb6920822b585a4fde577e60b36695938f098fde9133dfeda7089853e5c862be7e62c2ca3722919d6a3' \
    && echo "$dotnet_sha512  aspnetcore-targeting-pack.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm \
    && rm dotnet.rpm apphost.rpm targeting-pack.rpm aspnetcore-targeting-pack.rpm \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help

# Install PowerShell global tool
RUN powershell_version=7.3.0-preview.7 \
    && curl -fSL --output PowerShell.Linux.arm64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Linux.arm64.$powershell_version.nupkg \
    && powershell_sha512='6723654f051204187f10175409ee4cc6fb804d793f7c813c6d38f0b33c1d0a335631c32ad56d853b69e509e225a349edb297a047bda1e1d0ec765be91bfba06b' \
    && echo "$powershell_sha512  PowerShell.Linux.arm64.$powershell_version.nupkg" | sha512sum -c - \
    && mkdir -p /usr/share/powershell \
    && dotnet tool install --add-source / --tool-path /usr/share/powershell --version $powershell_version PowerShell.Linux.arm64 \
    && dotnet nuget locals all --clear \
    && rm PowerShell.Linux.arm64.$powershell_version.nupkg \
    && ln -s /usr/share/powershell/pwsh /usr/bin/pwsh \
    && chmod 755 /usr/share/powershell/pwsh \
    # To reduce image size, remove the copy nupkg that nuget keeps.
    && find /usr/share/powershell -print | grep -i '.*[.]nupkg$' | xargs rm
