# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:6.0.2-windowsservercore-ltsc2022

ENV `
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=6.0.200 `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Unset Logging__Console__FormatterName from aspnet base image
    Logging__Console__FormatterName= `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-WindowsServerCore-ltsc2022

RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Retrieve .NET SDK
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
        $dotnet_sha512 = '442ea9bb696005a9646aa02e10b0b09809c5b70acd91b08fd489aa8915f8b2a925d73843a742acb8f1068363d3db96137017ca478a6a69ea28b2f17a411eb5fe'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        tar -oxzf dotnet.zip -C $Env:ProgramFiles\dotnet ./LICENSE.txt ./ThirdPartyNotices.txt ./packs ./sdk ./sdk-manifests ./templates ./shared/Microsoft.WindowsDesktop.App; `
        Remove-Item -Force dotnet.zip; `
        `
        # Install PowerShell global tool
        $powershell_version = '7.2.1'; `
        Invoke-WebRequest -OutFile PowerShell.Windows.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Windows.x64.$powershell_version.nupkg; `
        $powershell_sha512 = '7170e2e331c070f4679a9d582dfb6cb320de1f2671d029b6f255fd81c73d969723991b4a876f4be03cfb034670cbc60ffbedc45a87862576d2cd862b66c1500a'; `
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
