# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:1809-amd64 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $aspnetcore_version = '9.0.0-preview.4.24173.3'; `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-win-x64.zip; `
        $aspnetcore_sha512 = 'c8bd86d26928c39cef0246ae56899a0bcf5d013d3107202bc93ad4ec71703d8e899bcbc4f639248d8f3d7505244e7cd7e38f25d551ea71b6c8219a065f6bd466'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet/shared/Microsoft.AspNetCore.App; `
        tar -oxzf aspnetcore.zip -C dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip


# ASP.NET Core image
FROM $REPO:9.0.0-preview.4-nanoserver-1809

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.4.24173.3

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
