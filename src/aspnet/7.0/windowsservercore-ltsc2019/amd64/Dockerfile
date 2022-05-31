# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.5-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.5.22273.7

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '646843a61eba6d6153d147b31a95c6fb046a797141b00aef887ce7b653a08a25bacf0f2960762099ba368e985091fee2d50f3fd8aab630de5f13c0ad0501276f'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
