# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:1809-amd64 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $aspnetcore_version = '7.0.0-preview.5.22303.8'; `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-win-x64.zip; `
        $aspnetcore_sha512 = '3aaa75387c78f0bffee37689d4f6220b32f9264d9f5c7eece04aa7e5db3df81a465f1e2da94a145797dca6f66d44a865b6d925bb8550ea1183ddcd975e514f88'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet/shared/Microsoft.AspNetCore.App; `
        tar -oxzf aspnetcore.zip -C dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip


# ASP.NET Core image
FROM $REPO:7.0.0-preview.5-nanoserver-1809

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.5.22303.8

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
