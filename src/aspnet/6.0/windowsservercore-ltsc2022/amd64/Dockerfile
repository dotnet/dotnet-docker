# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-windowsservercore-ltsc2022

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21514.23 `
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Install ASP.NET Core Runtime
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '03d87afc5f9696f362ad90ad08623fbd32aae232b5fc5350e143d53573c18414cd50e45e2d4a4a17f63b5ccd5bcc4fc95039d55ff15163cd178f790e1a9acb4e'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -C $Env:ProgramFiles\dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
