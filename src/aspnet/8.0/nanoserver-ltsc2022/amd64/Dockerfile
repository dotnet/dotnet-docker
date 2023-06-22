# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $aspnetcore_version = '8.0.0-preview.6.23320.14'; `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-win-x64.zip; `
        $aspnetcore_sha512 = '8f221b9f182c5bfb1c357f4455d7ba7ae7109904ff545d27f23abe7abf775ca6130d2e814f003ee9260de3f6837fa4d0d5c865a29ee6e05f02d07e76ee8ab801'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet/shared/Microsoft.AspNetCore.App; `
        tar -oxzf aspnetcore.zip -C dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip


# ASP.NET Core image
FROM $REPO:8.0.0-preview.6-nanoserver-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.6.23320.14

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
