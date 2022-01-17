# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-alpha.1-windowsservercore-ltsc2022

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22066.1 `
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '2ab42b19c37cafc8f6a6dbc908e0fcdea85b3c07af89dda7e46cad9ee517e56842a672ae0edd78450b50aa32db6f850d0ca34f78b415d561b1313415b0106a22'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -C $Env:ProgramFiles\dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
