# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $aspnetcore_version = '10.0.0-preview.6.25274.104'; `
        Invoke-WebRequest -OutFile aspnetcore.zip https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-win-x64.zip; `
        $aspnetcore_sha512 = 'a22c29eae86b11d3d91b795188013cb1a4e0cd0e96c6ed4fe6ee32351f5a697c42ab4a903c8790a35a5c26349b6a8a6c149bedab328cc024b1a6fea2c50eb8b2'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet/shared/Microsoft.AspNetCore.App; `
        tar --gzip --extract --no-same-owner --file aspnetcore.zip --directory dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip


# ASP.NET Core image
FROM $REPO:10.0.0-preview.6-windowsservercore-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.6.25274.104

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
