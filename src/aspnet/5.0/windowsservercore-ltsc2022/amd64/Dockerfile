# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.11-windowsservercore-ltsc2022

ENV ASPNET_VERSION=5.0.11

RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Install ASP.NET Core Runtime
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = 'ebfdab8c09fbb17d9e49971c0e4927cacaa164187dbfc4df8fd1ed52656d97681486ac148a2f2543d88da7a766287d5570fc8686deea12fdeef2b57afa0e084e'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -C $Env:ProgramFiles\dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
