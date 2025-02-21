# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:10.0.0-preview.1-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25120.3

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://ci.dot.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = 'a5e5c941ad8b831c31fe623d0d3675bb9e1b4c1a442a5890061b83ff874e0c106e87fa18394873798c193de8ce0146c90d5903e98c0926929614b15ffdde2f12'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
