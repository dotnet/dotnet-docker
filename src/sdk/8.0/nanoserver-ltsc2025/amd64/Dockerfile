# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/windows/servercore/insider:10.0.26244.5000 AS installer

# Download MinGit
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile mingit.zip https://github.com/git-for-windows/git/releases/download/v2.46.1.windows.1/MinGit-2.46.1-64-bit.zip; `
        $mingit_sha256 = 'd33b5a5838125aa2e06a917ffa7065831355fc13088abfaab3d0e3403d5928d2'; `
        if ((Get-FileHash mingit.zip -Algorithm sha256).Hash -ne $mingit_sha256) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        mkdir MinGit; `
        tar -oxzf mingit.zip -C MinGit; `
        Remove-Item -Force mingit.zip"

RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Retrieve .NET SDK
        $sdk_version = '8.0.402'; `
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Sdk/$sdk_version/dotnet-sdk-$sdk_version-win-x64.zip; `
        $dotnet_sha512 = '532e09b25ffb174db2ca6f7cd990571faaff6a6ae2963f242583d05606b9798b5672c85bd65ebe94674681c33f06f342e7e4214e6e6da72b227323e0cbb06b43'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        mkdir dotnet; `
        tar -oxzf dotnet.zip -C dotnet; `
        Remove-Item -Force dotnet.zip; `
        `
        # Install PowerShell global tool
        $powershell_version = '7.4.5'; `
        Invoke-WebRequest -OutFile PowerShell.Windows.x64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Windows.x64.$powershell_version.nupkg; `
        $powershell_sha512 = 'c072de6451beb3046af331387acad9f981366bb6c0b23f26c643e5f2d8e00adafb4d0520c98b1021f5a0cf498fc1cb9ad86af6cecaa260a286f34fed3f435892'; `
        if ((Get-FileHash PowerShell.Windows.x64.$powershell_version.nupkg -Algorithm sha512).Hash -ne $powershell_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        & \dotnet\dotnet tool install --add-source . --tool-path \powershell --version $powershell_version PowerShell.Windows.x64; `
        & \dotnet\dotnet nuget locals all --clear; `
        Remove-Item -Force PowerShell.Windows.x64.$powershell_version.nupkg; `
        Remove-Item -Path \powershell\.store\powershell.windows.x64\$powershell_version\powershell.windows.x64\$powershell_version\powershell.windows.x64.$powershell_version.nupkg -Force; `
        `
        # Delete everything in the dotnet folder that's not needed in the SDK layer but will instead be derived from base layers
        Get-ChildItem -Exclude 'LICENSE.txt','ThirdPartyNotices.txt','packs','sdk','sdk-manifests','templates','shared' -Path dotnet `
            | Remove-Item -Force -Recurse; `
        Get-ChildItem -Exclude 'Microsoft.WindowsDesktop.App' -Path dotnet\shared `
            | Remove-Item -Force -Recurse"

# SDK image
FROM $REPO:8.0.8-nanoserver-ltsc2025

ENV `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=8.0.402 `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-NanoServer-ltsc2025

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\powershell;C:\Program Files\MinGit\cmd"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
COPY --from=installer ["/powershell", "/Program Files/powershell"]
COPY --from=installer ["/MinGit", "/Program Files/MinGit"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
