# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Download MinGit
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile mingit.zip https://github.com/git-for-windows/git/releases/download/v2.41.0.windows.1/MinGit-2.41.0-64-bit.zip; `
        $mingit_sha256 = 'c9cffc25e2ef81f51029138678b7bfc538a56095ec0538125dc790a01e20d77a'; `
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
        $sdk_version = '7.0.305'; `
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Sdk/$sdk_version/dotnet-sdk-$sdk_version-win-x64.zip; `
        $dotnet_sha512 = '9cad4fe5efbf9d37bcefad88a1623da12946958759a6ab7795602598a9e2c7e9a948c335e7fbb002b7c03af877798f24c97546857e8822f639cf92ae81006221'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        mkdir dotnet; `
        tar -oxzf dotnet.zip -C dotnet; `
        Remove-Item -Force dotnet.zip; `
        `
        # Install PowerShell global tool
        $powershell_version = '7.3.5'; `
        Invoke-WebRequest -OutFile PowerShell.Windows.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Windows.x64.$powershell_version.nupkg; `
        $powershell_sha512 = 'eedccc2e277ca0e7c5f110c12f02ad56cdc867ec1d83a37cd2c9aeac5e931b9182a2d0ac6fc81d74016a9a19092c9f57e797972a0ff6185ed21692b270482aa2'; `
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
FROM $REPO:7.0.8-nanoserver-ltsc2022

ENV `
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=7.0.305 `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-NanoServer-ltsc2022

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\powershell;C:\Program Files\MinGit\cmd"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
COPY --from=installer ["/powershell", "/Program Files/powershell"]
COPY --from=installer ["/MinGit", "/Program Files/MinGit"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
