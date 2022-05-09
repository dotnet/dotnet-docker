# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.17-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.17

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = 'ffa1ac15c489acde822981b4c9fdf69eb0e55dcb3e1bd3d9c23cceafba88388a4420f6b9ac01e5758c6162993ffa3a76350f0b50afffe9ad10eae74adffcbe0a'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
