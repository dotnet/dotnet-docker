# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:5.0-windowsservercore-ltsc2019

ENV `
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= `
    DOTNET_SDK_VERSION=5.0.100-rc.2.20479.15 `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
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
        $dotnet_sha512 = '49ea4f9e0fdc51bfd46b1269b84dd09f26be957c3a81c80203a0b0d521c8ec190d0d2d631a4e1899c83b604bf9a529698f19eccdf4aed3d0fd2f89102f6556e1'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        tar -C $Env:ProgramFiles\dotnet -oxzf dotnet.zip ./packs ./sdk ./templates ./LICENSE.txt ./ThirdPartyNotices.txt ./shared/Microsoft.WindowsDesktop.App; `
        Remove-Item -Force dotnet.zip; `
        `
        # Install PowerShell global tool
        $powershell_version = '7.1.0-rc.1'; `
        Invoke-WebRequest -OutFile PowerShell.Windows.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Windows.x64.$powershell_version.nupkg; `
        $powershell_sha512 = '11548549314d2e940210ef1d79b6948ca14a6ad1296591ad115a292b4003ff5090ea376b2d7957c00b88b153c9e043b95cf91e3ff20bd74c6e8ba44a763ec957'; `
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
