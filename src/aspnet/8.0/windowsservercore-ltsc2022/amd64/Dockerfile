# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:8.0.12-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.12

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '28a3d2cde0d55da3c6943d707b2744cbf9b225bf3e66f8dbb18aba5196725bd7048f4fba5af219ae0a7aef31a4ff7e309396518ee7dc0eb2a5c2a61955d5b10c'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
