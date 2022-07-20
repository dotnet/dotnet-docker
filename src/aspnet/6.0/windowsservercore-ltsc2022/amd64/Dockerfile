# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.7-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.7-servicing.22322.5

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = 'e2c52af3361fdb9d5897a238e8275f7f9df8b37bd2b7d6f24c7b889111d1f1fd80f30b1a68022dee3e7378c6973a37f1ad0c96cea4c22d73d3cb5068d1cb3efd'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
