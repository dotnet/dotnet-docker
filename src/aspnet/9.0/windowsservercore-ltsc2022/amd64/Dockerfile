# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:9.0.0-preview.4-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.4.24223.1

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = 'c11defd769c681b22d18216808c33ef4557f15981e1300b37142c91c1ebeee8f2abfd4e5aa283c07d48c5dbd597fe75cfcc50b7a5fa6bde10ead9daab7daba06'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
