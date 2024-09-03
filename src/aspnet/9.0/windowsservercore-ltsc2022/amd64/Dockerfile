# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:9.0.0-rc.1-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.1.24452.1

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '089020e21b337c74860c2c67ba367bdc4c270da822f9f1b181e092a9d96e8d6ff2609f05bc6ac5410523aa991fb5fa876477362dcf2f72c4c2ea2ea0e295b3b5'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
