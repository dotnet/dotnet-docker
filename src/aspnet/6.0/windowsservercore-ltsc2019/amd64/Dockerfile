# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.23-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.23

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '1dc5781bc3bdf633a4141b12fee25673c3448bae89d119d8f59c792f39eef84a505035f2526dd624ed6cfac1f7da7e02d3a23f19f3d5e1961876102472212bf6'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
