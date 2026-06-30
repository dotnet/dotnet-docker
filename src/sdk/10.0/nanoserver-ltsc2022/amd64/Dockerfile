# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Download MinGit
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile mingit.zip https://github.com/git-for-windows/git/releases/download/v2.55.0.windows.1/MinGit-2.55.0-64-bit.zip; `
        $mingit_sha256 = '31497e7968196332263459ee319d2524e3ebc5786ab895e2abad34ffdd4f4ebf'; `
        if ((Get-FileHash mingit.zip -Algorithm sha256).Hash -ne $mingit_sha256) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        mkdir MinGit; `
        tar --gzip --extract --no-same-owner --file mingit.zip --directory MinGit; `
        Remove-Item -Force mingit.zip"

RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Retrieve .NET SDK
        $dotnet_sdk_version = '10.0.301'; `
        $dotnet_file = 'dotnet-sdk-' + $dotnet_sdk_version + '-win-x64.zip'; `
        $dotnet_sha512_file = $dotnet_file + '.sha512-bare'; `
        `
        Invoke-WebRequest -OutFile $dotnet_file https://builds.dotnet.microsoft.com/dotnet/Sdk/$dotnet_sdk_version/$dotnet_file; `
        Invoke-WebRequest -OutFile $dotnet_sha512_file https://builds.dotnet.microsoft.com/dotnet/Sdk/$dotnet_sdk_version/$dotnet_sha512_file; `
        `
        if ((Get-FileHash $dotnet_file -Algorithm sha512).Hash -ne (Get-Content $dotnet_sha512_file)) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file $dotnet_file --directory dotnet; `
        Remove-Item -Force `
            $dotnet_file, `
            $dotnet_sha512_file; `
        `
        # Install PowerShell global tool
        $powershell_version = '7.6.2'; `
        Invoke-WebRequest -OutFile PowerShell.Windows.x64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Windows.x64.$powershell_version.nupkg; `
        $powershell_sha512 = '2125580fe740a5e0483f6d5e3a92714c2c9487ab3d38b1f9cd4632b41444280d379fb19f757f2cd6c95d8059ff1bde71c4acc664cf0895b31666fbfc20eeee73'; `
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
        Get-ChildItem -Exclude 'dnx.cmd','dnx.ps1','LICENSE.txt','ThirdPartyNotices.txt','packs','sdk','sdk-manifests','templates','shared' -Path dotnet `
            | Remove-Item -Force -Recurse; `
        Get-ChildItem -Exclude 'Microsoft.WindowsDesktop.App' -Path dotnet\shared `
            | Remove-Item -Force -Recurse"


# SDK image
FROM $REPO:10.0.9-nanoserver-ltsc2022

ENV `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=10.0.301 `
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
