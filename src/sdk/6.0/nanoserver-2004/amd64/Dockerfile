# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet
ARG DOTNET_SDK_VERSION=6.0.100-preview.3.21202.5

# Installer image
FROM mcr.microsoft.com/windows/servercore:2004-amd64 AS installer
ARG DOTNET_SDK_VERSION

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

RUN `
    # Retrieve .NET SDK
    Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
    $dotnet_sha512 = '38fc6d76ef0d2331fcc32319be17edf4887443ad7504131041a12026340f7418227b85aa597d7ecb6c4a57cfd928004c6511455cedc6dcee42284793f66146cf'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    mkdir dotnet; `
    tar -C dotnet -oxzf dotnet.zip; `
    Remove-Item -Force dotnet.zip; `
    `
    # Install PowerShell global tool
    $powershell_version = '7.2.0-preview.5'; `
    Invoke-WebRequest -OutFile PowerShell.Windows.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Windows.x64.$powershell_version.nupkg; `
    $powershell_sha512 = '76fec0e3160bb9d19531cb95f7bd1fce07074f990bc2e2e35fe67b00460e034a24079d52aef0353070cecb8261721f1bb314bdd8919f52f788e6c48cadedbecd'; `
    if ((Get-FileHash PowerShell.Windows.x64.$powershell_version.nupkg -Algorithm sha512).Hash -ne $powershell_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    \dotnet\dotnet tool install --add-source . --tool-path \powershell --version $powershell_version PowerShell.Windows.x64; `
    \dotnet\dotnet nuget locals all --clear; `
    Remove-Item -Force PowerShell.Windows.x64.$powershell_version.nupkg; `
    Remove-Item -Path \powershell\.store\powershell.windows.x64\$powershell_version\powershell.windows.x64\$powershell_version\powershell.windows.x64.$powershell_version.nupkg -Force; `
    `
    # Delete everything in the dotnet folder that's not needed in the SDK layer but will instead be derived from base layers
    Get-ChildItem -Exclude 'LICENSE.txt','ThirdPartyNotices.txt','packs','sdk','sdk-manifests','templates','shared' -Path dotnet `
        | Remove-Item -Force -Recurse; `
    Get-ChildItem -Exclude Microsoft.WindowsDesktop.App -Path dotnet\shared | Remove-Item -Force -Recurse

# SDK image
FROM $REPO:6.0-nanoserver-2004
ARG DOTNET_SDK_VERSION

ENV `
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=$DOTNET_SDK_VERSION `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-NanoServer-2004

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\powershell"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]

COPY --from=installer ["/powershell", "/Program Files/powershell"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
