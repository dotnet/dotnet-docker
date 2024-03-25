# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:9.0.0-preview.4-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.4.24173.3

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = 'c8bd86d26928c39cef0246ae56899a0bcf5d013d3107202bc93ad4ec71703d8e899bcbc4f639248d8f3d7505244e7cd7e38f25d551ea71b6c8219a065f6bd466'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
