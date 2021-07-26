# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:20H2-amd64 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
RUN Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-preview.7.21374.2/aspnetcore-runtime-6.0.0-preview.7.21374.2-win-x64.zip; `
    $aspnetcore_sha512 = 'e3b5440cfe813a681dbc9a53611ad23a26ff6b4c04d0a28c63e4cc3123123f38ea76ab743ed026a4c551f42f7064464a377b2f12c88b8db70256b44a1dce1956'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    mkdir dotnet/shared/Microsoft.AspNetCore.App; `
    tar -C dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
    Remove-Item -Force aspnetcore.zip


# ASP.NET Core image
FROM $REPO:6.0.0-preview.7-nanoserver-20H2

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.7.21374.2 `
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
