# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.9-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.9

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '87a187781cb0b5cc364ed52d240feb703855698cfc6c24e49d331ddaa759cfcb135813292fb9d18dc92d182531d8eccf3a50c0494415ca5776afe8cc5230b650'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
