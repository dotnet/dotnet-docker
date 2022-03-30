# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.3-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.3.22175.3

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = 'b673d12d0a3f7427b4404e39fd64379e08a38761ba4a719920ce87331f831bb9c677186c01ad2dd3c5d3eb89bfc5bb2b4958d3cbc074c7a855a4feaa83d21ac5'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
