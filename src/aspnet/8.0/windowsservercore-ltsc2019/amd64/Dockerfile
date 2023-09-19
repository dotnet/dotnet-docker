# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:8.0.0-rc.2-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-rc.2.23468.15

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '1a0aa2aea22889f959fbdf0716714f6a16592316f4c97ae6e453bcc6e4dc1b6c080a165c6304b9e36b325636b3a730cb43545b63854ce8cff8c3fc710723a9e6'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
