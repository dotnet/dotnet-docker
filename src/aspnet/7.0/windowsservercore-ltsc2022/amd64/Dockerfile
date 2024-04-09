# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.18-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.18

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '27cdb80a0a28f4d0fb375a5b0fc5bd80e9e4425f6a92dc5c7fc68578c8b6eb1f9a86293e840b056d38914b1ad2a9b05b17f284e2cc459cd5ec81819b89aa0786'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
