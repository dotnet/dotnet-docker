# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=5.0.0-rtm.20526.5

# Installer image
FROM mcr.microsoft.com/windows/servercore:1909-amd64 AS installer
ARG ASPNET_VERSION

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
RUN Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-5.0.0-win-x64.zip; `
    $aspnetcore_sha512 = 'ead6b1e12361261124bb89de1e77dba9469ce00483d963b237ce1e2a17500e420c2bd078c2732e7ce269e23ee3922b880f88ade9c1426c7170045fec1818ece4'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    mkdir dotnet/shared/Microsoft.AspNetCore.App; `
    tar -C dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
    Remove-Item -Force aspnetcore.zip


# ASP.NET Core image
FROM $REPO:5.0-nanoserver-1909
ARG ASPNET_VERSION

ENV ASPNET_VERSION $ASPNET_VERSION

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
