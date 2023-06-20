# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:8.0.0-preview.6-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.6.23316.5

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '910d93144acbab6cd32c381dbd2654b34e2edbc03807181d0f0bf60af4aa8500d6c9cfdd0b98ea31d793cd094582a0eb93a2ac59ed3aa0896bea6313d443e52a'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
