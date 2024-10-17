# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore/insider:10.0.26304.5000 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $aspnetcore_version = '8.0.10'; `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-win-x64.zip; `
        $aspnetcore_sha512 = 'b9d08366e056def8760882d08f09e8495a6cf6ebfd1b881c92f3c385637c77ccf432ed51f930e4994e66156fb5311daeb0b2aa06074da17cbbb068e0e5dfad3a'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet/shared/Microsoft.AspNetCore.App; `
        tar -oxzf aspnetcore.zip -C dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip


# ASP.NET Core image
FROM $REPO:8.0.10-nanoserver-ltsc2025

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.10

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
