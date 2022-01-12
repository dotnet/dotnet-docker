# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:7.0.0-alpha.1-windowsservercore-ltsc2019

ENV `
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=7.0.100-alpha.1.22060.9 `
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
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetbuilds.azureedge.net/public/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
        $dotnet_sha512 = '7351eb177309da568507f5f29570afd8ea3f882b5b44b40aff3c2d3c1b50018faa9503d24dd3e4151af33ed403217f94a27c8cc52c7170a7f39109014e4e7669'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        tar -C $Env:ProgramFiles\dotnet -oxzf dotnet.zip ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt ./shared/Microsoft.WindowsDesktop.App; `
        Remove-Item -Force dotnet.zip;"

RUN setx /M PATH "%PATH%;C:\Program Files\powershell"

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
