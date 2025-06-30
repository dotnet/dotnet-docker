# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnetFile = \"aspnetcore-runtime-10.0.0-preview.6.25326.107-win-x64.zip\"; `
        $dotnetSha512File = \"aspnetcore-runtime-10.0.0-preview.6.25326.107-win-x64.zip.sha512\"; `
        `
        Invoke-WebRequest -OutFile $dotnetFile https://ci.dot.net/public/aspnetcore/Runtime/10.0.0-preview.6.25326.107/aspnetcore-runtime-10.0.0-preview.6.25326.107-win-x64.zip; `
        Invoke-WebRequest -OutFile $dotnetSha512File https://ci.dot.net/public-checksums/aspnetcore/Runtime/10.0.0-preview.6.25326.107/aspnetcore-runtime-10.0.0-preview.6.25326.107-win-x64.zip.sha512; `
        `
        if ((Get-FileHash $dotnetFile -Algorithm sha512).Hash -ne (Get-Content $dotnetSha512File)) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet/shared/Microsoft.AspNetCore.App; `
        tar --gzip --extract --no-same-owner --file $dotnetFile --directory dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force $dotnetFile, $dotnetSha512File"


# ASP.NET Core image
FROM $REPO:10.0.0-preview.6-nanoserver-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.6.25326.107

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
