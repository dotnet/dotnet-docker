# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:8.0.0-preview.6-windowsservercore-ltsc2019

ENV `
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false `
    # Do not show first run text
    DOTNET_NOLOGO=true `
    # SDK version
    DOTNET_SDK_VERSION=8.0.100-preview.6.23322.27 `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-WindowsServerCore-ltsc2019

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
        mkdir $Env:ProgramFiles\MinGit; `
        tar -oxzf mingit.zip -C $Env:ProgramFiles\MinGit; `
        Remove-Item -Force mingit.zip"

RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Retrieve .NET SDK
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetbuilds.azureedge.net/public/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
        $dotnet_sha512 = '11521201663198b562aaf22a22dddd72d1447378cabae740d26aa439bee9635194f31067372466f565526a5ded9225e436cc7a8308fc00bb36b630e0314fbcab'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        tar -oxzf dotnet.zip -C $Env:ProgramFiles\dotnet ./LICENSE.txt ./ThirdPartyNotices.txt ./packs ./sdk ./sdk-manifests ./templates ./shared/Microsoft.WindowsDesktop.App; `
        Remove-Item -Force dotnet.zip; `
        `
        # Install PowerShell global tool
        $powershell_version = '7.4.0-preview.3'; `
        Invoke-WebRequest -OutFile PowerShell.Windows.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Windows.x64.$powershell_version.nupkg; `
        $powershell_sha512 = 'e1c97f48e5de1b38dfaf60be1085c3f6c996c61a5ce1a20dec0a9e50bb747cccf816c6778f77951cff7f3b24095d7e7f849bc9b222e139ef07c7b90381a4b21b'; `
        if ((Get-FileHash PowerShell.Windows.x64.$powershell_version.nupkg -Algorithm sha512).Hash -ne $powershell_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        & $Env:ProgramFiles\dotnet\dotnet tool install --add-source . --tool-path $Env:ProgramFiles\powershell --version $powershell_version PowerShell.Windows.x64; `
        & $Env:ProgramFiles\dotnet\dotnet nuget locals all --clear; `
        Remove-Item -Force PowerShell.Windows.x64.$powershell_version.nupkg; `
        Remove-Item -Path $Env:ProgramFiles\powershell\.store\powershell.windows.x64\$powershell_version\powershell.windows.x64\$powershell_version\powershell.windows.x64.$powershell_version.nupkg -Force; `
        `
        # Workaround for https://github.com/PowerShell/PowerShell/issues/19679
        $runtimeConfig = Get-Content $Env:ProgramFiles\powershell\.store\powershell.windows.x64\$powershell_version\powershell.windows.x64\$powershell_version\tools\net8.0\any\pwsh.runtimeconfig.json -Raw | ConvertFrom-Json; `
        $runtimeConfig.runtimeOptions.configProperties | Add-Member -MemberType NoteProperty -Name "System.Runtime.Loader.UseRidGraph" -Value $true; `
        $runtimeConfig | ConvertTo-Json -Depth 10 | Set-Content $Env:ProgramFiles\powershell\.store\powershell.windows.x64\$powershell_version\powershell.windows.x64\$powershell_version\tools\net8.0\any\pwsh.runtimeconfig.json;"

RUN setx /M PATH "%PATH%;C:\Program Files\powershell;C:\Program Files\MinGit\cmd"

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
