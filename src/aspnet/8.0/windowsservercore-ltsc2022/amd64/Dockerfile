# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:8.0.0-preview.2-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.3.23126.2

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '279363d50a56192f4f46129b4cd2be0c957a9fc35e3cb0ce603c0b7282ea68bd656739897dce75295acf06a0cabb6b957fd9e5f933c2fe7ea285f392f442e7e4'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
