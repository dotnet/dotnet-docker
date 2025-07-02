# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Download MinGit
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile mingit.zip \"https://github.com/git-for-windows/git/releases/download/v2.50.0.windows.2/MinGit-2.50.0.2-64-bit.zip\"; `
        $mingit_sha256 = '6d28c7e9f9c219a16c078c94a80492dd10fa309fbd17a67b2230736fdfb263b9'; `
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
        $dotnet_sdk_version = \"8.0.411\"; `
        $dotnet_version = \"8.0.17\"; `
        $dotnet_file = \"dotnet-sdk-$dotnet_sdk_version-win-x64.tar.gz\"; `
        $dotnet_checksums_file = \"$dotnet_version-sha.txt\"; `
        `
        Invoke-WebRequest -OutFile $dotnet_file \"https://builds.dotnet.microsoft.com/dotnet/Sdk/$dotnet_sdk_version/$dotnet_file\"; `
        Invoke-WebRequest -OutFile $dotnet_checksums_file \"https://builds.dotnet.microsoft.com/dotnet/checksums/$dotnet_checksums_file\"; `
        `
        $dotnetSha512 = ( `
            (Get-Content $dotnet_checksums_file | Where-Object { `
                    $_ -match \"$([regex]::Escape($dotnet_file))$\"; `
            }) -split '\s+' `
        )[0].ToUpper(); `
        $actualHash = (Get-FileHash $dotnet_file -Algorithm SHA512).Hash.ToUpper(); `
        `
        if ($dotnetSha512 -ne $actualHash) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            Write-Host \"Expected: $expectedHash\"; `
            Write-Host \"Actual:   $actualHash\"; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file $dotnet_file --directory dotnet; `
        Remove-Item -Force `
            $dotnet_file `
            $dotnet_checksums_file; `
        `
        # Install PowerShell global tool
        $powershell_version = '7.4.11'; `
        Invoke-WebRequest -OutFile  \"\"; `
        Invoke-WebRequest -OutFile  \"\"; `
        ; `
        $powershell_sha512 = '3e8848ef2759e2941217438635330848b1a340c7cbea16e9a53d76612b695ba27a689f9a1895b1a1cd458b7e21ed18bf726f0a8b95421018356a07def36cdbd2'; `
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
FROM $REPO:8.0.17-windowsservercore-ltsc2022

ENV `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=8.0.411 `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-WindowsServerCore-ltsc2022

RUN setx /M PATH "%PATH%;C:\Program Files\powershell;C:\Program Files\MinGit\cmd"

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
COPY --from=installer ["/powershell", "/Program Files/powershell"]
COPY --from=installer ["/MinGit", "/Program Files/MinGit"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
