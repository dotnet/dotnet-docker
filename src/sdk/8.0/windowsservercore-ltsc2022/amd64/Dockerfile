# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:8.0.0-alpha.1-windowsservercore-ltsc2022

ENV `
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=8.0.100-alpha.1.23061.8 `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-WindowsServerCore-ltsc2022

# Download MinGit
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile mingit.zip https://github.com/git-for-windows/git/releases/download/v2.39.0.windows.2/MinGit-2.39.0.2-64-bit.zip; `
        $mingit_sha256 = '771e7bef1b672e3f63b18b8c4a62d626c8f47c41390a745f313758c0b6ae4d63'; `
        if ((Get-FileHash mingit.zip -Algorithm sha256).Hash -ne $mingit_sha256) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        mkdir $Env:ProgramFiles\MinGit; `
        tar -oxzf mingit.zip -C $Env:ProgramFiles\MinGit; `
        Remove-Item -Force mingit.zip"

RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Retrieve .NET SDK
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetbuilds.azureedge.net/public/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
        $dotnet_sha512 = '12b304b38bb2a90c79804d77bb85c42d963a8ab34a930e50d0458ef62555cec9f5ccfa63447dc8422b85e084a2dc57bed9303def3c619b80b113d39670facbf8'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        tar -oxzf dotnet.zip -C $Env:ProgramFiles\dotnet ./LICENSE.txt ./ThirdPartyNotices.txt ./packs ./sdk ./sdk-manifests ./templates ./shared/Microsoft.WindowsDesktop.App; `
        Remove-Item -Force dotnet.zip;"

RUN setx /M PATH "%PATH%;C:\Program Files\powershell;C:\Program Files\MinGit\cmd"

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
