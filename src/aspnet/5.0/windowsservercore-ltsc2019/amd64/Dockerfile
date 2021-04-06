# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-windowsservercore-ltsc2019

ENV ASPNET_VERSION=5.0.5

RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Install ASP.NET Core Runtime
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '1dc0fba71253374090a9e5b745e438c4f2d87b0558fc186ef2cbbfdf9c7869381d9b1cab1421759d12805f36870f40e88e5f0da372e1bf35081a2361f731bf55'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -C $Env:ProgramFiles\dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
