# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2025-amd64 AS installer

# Download MinGit
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile mingit.zip https://github.com/git-for-windows/git/releases/download/v2.52.0.windows.1/MinGit-2.52.0-64-bit.zip; `
        $mingit_sha256 = 'f42a561840627747ad48e6ece05a14093292d31f3393a401a7f7c780ee7695c2'; `
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
        $dotnet_sdk_version = '8.0.416'; `
        $dotnet_version = '8.0.22'; `
        $dotnet_file = 'dotnet-sdk-' + $dotnet_sdk_version + '-win-x64.zip'; `
        $dotnet_checksums_file = $dotnet_version + '-sha.txt'; `
        `
        Invoke-WebRequest -OutFile $dotnet_file https://builds.dotnet.microsoft.com/dotnet/Sdk/$dotnet_sdk_version/$dotnet_file; `
        Invoke-WebRequest -OutFile $dotnet_checksums_file https://builds.dotnet.microsoft.com/dotnet/checksums/$dotnet_checksums_file; `
        `
        $dotnet_sha512 = ( `
            (Get-Content $dotnet_checksums_file | Where-Object { `
                    $_ -match ([regex]::Escape($dotnet_file))` + '$'; `
            }) -split '\s+' `
        )[0].ToUpper(); `
        $actual_hash = (Get-FileHash $dotnet_file -Algorithm SHA512).Hash.ToUpper(); `
        `
        if ($dotnet_sha512 -ne $actual_hash) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            Write-Host 'Expected: ' + $dotnet_sha512; `
            Write-Host 'Actual:   ' + $actual_hash; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file $dotnet_file --directory dotnet; `
        Remove-Item -Force `
            $dotnet_file, `
            $dotnet_checksums_file; `
        `
        # Install PowerShell global tool
        $powershell_version = '7.4.13'; `
        Invoke-WebRequest -OutFile PowerShell.Windows.x64.$powershell_version.nupkg https://powershellinfraartifacts-gkhedzdeaghdezhr.z01.azurefd.net/tool/$powershell_version/PowerShell.Windows.x64.$powershell_version.nupkg; `
        $powershell_sha512 = 'e819797e59c10e3da29e56184e4b6941bd731f7cfc79207a88ff3a5d42e913c2429d5cb28cdbb071b60061151c37395377d52d9e1e5f7c99e625a6f3be78efb7'; `
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
FROM $REPO:8.0.22-nanoserver-ltsc2025

ENV `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=8.0.416 `
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
