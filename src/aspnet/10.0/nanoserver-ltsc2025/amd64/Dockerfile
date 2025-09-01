# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2025-amd64 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $aspnetcore_version = '10.0.0-rc.1.25429.105'; `
        $aspnetcore_file = 'aspnetcore-runtime-' + $aspnetcore_version + '-win-x64.zip'; `
        $dotnet_sha512_file = $aspnetcore_file + '.sha512'; `
        `
        Invoke-WebRequest -OutFile $aspnetcore_file https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/$aspnetcore_file; `
        Invoke-WebRequest -OutFile $dotnet_sha512_file https://ci.dot.net/public-checksums/aspnetcore/Runtime/$aspnetcore_version/$dotnet_sha512_file; `
        `
        if ((Get-FileHash $aspnetcore_file -Algorithm sha512).Hash -ne (Get-Content $dotnet_sha512_file)) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file $aspnetcore_file --directory dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force `
            $aspnetcore_file, `
            $dotnet_sha512_file


# ASP.NET Core image
FROM $REPO:10.0.0-rc.1-nanoserver-ltsc2025

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-rc.1.25429.105

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
