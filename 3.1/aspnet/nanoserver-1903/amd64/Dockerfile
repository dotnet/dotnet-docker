# escape=`

ARG REPO=mcr.microsoft.com/dotnet/core-nightly/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:1903 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
ENV ASPNETCORE_VERSION 3.1.0-preview2.19522.3

RUN Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$Env:ASPNETCORE_VERSION/aspnetcore-runtime-$Env:ASPNETCORE_VERSION-win-x64.zip; `
    $aspnetcore_sha512 = '8f546fffedd95d0db4c7215aa59336d6881ea4eecc75c589e99ed894c139b87dd854b9c5d8ce93b4da2e8cc786b63c1784d963fc4929677527007eee4683be31'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive aspnetcore.zip -DestinationPath dotnet; `
    Remove-Item -Force aspnetcore.zip


# Runtime image
FROM $REPO:3.1-nanoserver-1903

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
