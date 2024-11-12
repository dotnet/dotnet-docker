# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:8.0.11-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.11

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = 'a3fafcdbd72e3a34c60c8a607a2c8e4122bef88d8f2fea56d7a87552546adfc646865b99a0cafd8ef945b0db918ed189ad1080e9e5f845f10a25fa9df5961c85'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
