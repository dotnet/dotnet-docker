# escape=`

ARG REPO=mcr.microsoft.com/dotnet/core/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:1903 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
RUN $aspnetcore_version = '5.0.0-preview.3.20207.4'; `
    Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-win-x64.zip; `
    $aspnetcore_sha512 = '340bf964fafcacd0870725d9cc129e14822eabe1610dfcfebc782ec438ef7cd64e8f229b62042da2eae5f1603cd6b43526ccbf6a6a4beb2d696d8a274d707f45'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive aspnetcore.zip -DestinationPath dotnet; `
    Remove-Item -Force aspnetcore.zip


# Runtime image
FROM $REPO:5.0-nanoserver-1903

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
