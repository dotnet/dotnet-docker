# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:10.0.0-alpha.1-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-alpha.2.25073.4

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://ci.dot.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '7d009a0b511ae475cf03cd8ab9a605502e0e1d70a6e9b64f332eecff2023c96b3ae89fa594c328b6745e97717ccd0efb1e390dada9b38c4fb30de50a95b3b232'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
