# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:10.0.0-preview.1-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25079.4

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://ci.dot.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '0b1503d98df15b30f5fc2114707c1a9dc95e24e90ff5f7278f0820886cf835ce46ce36f4e1069d4076ab51922e458547d6e6c5a36850a8fca9c550a2edac0d97'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
