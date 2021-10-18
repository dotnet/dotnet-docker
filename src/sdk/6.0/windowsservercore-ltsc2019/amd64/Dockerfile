# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.0-windowsservercore-ltsc2019

ENV `
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=6.0.100-rtm.21518.6 `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Unset Logging__Console__FormatterName from aspnet base image
    Logging__Console__FormatterName= `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-WindowsServerCore-ltsc2019

RUN powershell -Command "`
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Retrieve .NET SDK
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
        $dotnet_sha512 = '58c92ab9b1084d3c69175620530c3eca9d7b67e4d7c3d2f994486441b7a3530c568a7dfb3eeded28f6ce7179d92710751eb2cf0d866942d205044ac0d0b8716a'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        tar -C $Env:ProgramFiles\dotnet -oxzf dotnet.zip ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt ./shared/Microsoft.WindowsDesktop.App; `
        Remove-Item -Force dotnet.zip; `
        `
        # Install PowerShell global tool
        $powershell_version = '7.2.0-preview.10'; `
        Invoke-WebRequest -OutFile PowerShell.Windows.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Windows.x64.$powershell_version.nupkg; `
        $powershell_sha512 = 'dce4b7f3e6579a65484ec3d8ada23f31f862f49fe1aeae78b3326a8e64399f1247d40b65ad9b28eefac69466d13ea6b2985bad8869051482d558305395172987'; `
        if ((Get-FileHash PowerShell.Windows.x64.$powershell_version.nupkg -Algorithm sha512).Hash -ne $powershell_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        & $Env:ProgramFiles\dotnet\dotnet tool install --add-source . --tool-path $Env:ProgramFiles\powershell --version $powershell_version PowerShell.Windows.x64; `
        & $Env:ProgramFiles\dotnet\dotnet nuget locals all --clear; `
        Remove-Item -Force PowerShell.Windows.x64.$powershell_version.nupkg; `
        Remove-Item -Path $Env:ProgramFiles\powershell\.store\powershell.windows.x64\$powershell_version\powershell.windows.x64\$powershell_version\powershell.windows.x64.$powershell_version.nupkg -Force;"

RUN setx /M PATH "%PATH%;C:\Program Files\powershell"

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
