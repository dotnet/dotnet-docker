# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:9.0.0-preview.7-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.7.24366.8

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '5b76e045123b813a060c14856e98ea9f6d28d34ee24e2239eec3aa90ac9a9c2eb33c3002ba92a45584fe44ef1cb47a49a8424d6bda1caf67bda500426518f655'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
