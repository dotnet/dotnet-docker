# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:10.0.0-alpha.1-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-alpha.2.25070.3

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://ci.dot.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '9249b53b636f3551758c3030ad6fe3f2833d9b0d71a4c8fb96c38bea5fe329446e68bff3f2751ff401753edfe70e8baec6464462dcf85973806c706964d40391'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
