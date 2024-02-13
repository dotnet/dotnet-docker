# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.16-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.16

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '255a587e9540aa4b03d1179e257ebeee8582b3199c9ec57ba2575334059a36b4cb1e40db93437341edab3eee1172f5801679e14e15ce397325c03103c809431c'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
