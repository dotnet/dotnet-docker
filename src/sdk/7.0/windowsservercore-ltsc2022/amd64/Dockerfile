# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:7.0.0-preview.1-windowsservercore-ltsc2022

ENV `
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=7.0.100-preview.1.22076.6 `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Unset Logging__Console__FormatterName from aspnet base image
    Logging__Console__FormatterName= `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-WindowsServerCore-ltsc2022

RUN powershell -Command "`
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Retrieve .NET SDK
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetbuilds.azureedge.net/public/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
        $dotnet_sha512 = '7a39538f6319d2bce678a6771304c4ebda858aac49b7a1e0e1bc30e60da4b2313de707e2482f56157ca0673782b56d89808eb0b2273316dad1759a70f9de2a22'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        tar -oxzf dotnet.zip -C $Env:ProgramFiles\dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt ./shared/Microsoft.WindowsDesktop.App; `
        Remove-Item -Force dotnet.zip; `
        `
        # Install PowerShell global tool
        $powershell_version = '7.3.0-preview.1'; `
        Invoke-WebRequest -OutFile PowerShell.Windows.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Windows.x64.$powershell_version.nupkg; `
        $powershell_sha512 = '9dcaace7382b8946bde8f8fcf177794962f881882e3cfd7e71fb4b64778872a471cefc3af5bf3cfc0de5af40602be85f67ec78ceaa57e049f5ad341ac4ec837d'; `
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
