# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:9.0.0-windowsservercore-ltsc2025

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rtm.24514.7

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '6debd4233e98d2a44d221834396f84ca9f22877d471f4ad6a2d6c6cea2285b590d9942b359c08a52fd5d7cb213e21d279408fd27c9339904b8f71b18f2ca387c'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
