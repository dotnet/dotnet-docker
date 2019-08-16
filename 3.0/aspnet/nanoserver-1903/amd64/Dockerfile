# escape=`

ARG REPO=mcr.microsoft.com/dotnet/core-nightly/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:1903 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
ENV ASPNETCORE_VERSION 3.0.0-preview9.19415.11

RUN Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$Env:ASPNETCORE_VERSION/aspnetcore-runtime-$Env:ASPNETCORE_VERSION-win-x64.zip; `
    $aspnetcore_sha512 = '35fa8c76b801d75faa11ba42fdda88ea51dc3f9d83072be6750715120ad56e0ea64488de8a9f024af73c1429b6f3a62821dd768138e97176dd13206c5fbfb99a'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive aspnetcore.zip -DestinationPath dotnet; `
    Remove-Item -Force aspnetcore.zip


# Runtime image
FROM $REPO:3.0-nanoserver-1903

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
