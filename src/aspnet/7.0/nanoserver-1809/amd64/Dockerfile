# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:1809-amd64 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
RUN Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/7.0.0-alpha.1.21567.15/aspnetcore-runtime-7.0.0-alpha.1.21567.15-win-x64.zip; `
    $aspnetcore_sha512 = '146776ab9634938e351585f1656bd5b9c056e06e6feb21a23251a4e0bbdba8496fb1c58fbe0dbc371fe8c63b33c5a4463a53bbc531dcdfc7d86afd3bfd7f269d'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    mkdir dotnet/shared/Microsoft.AspNetCore.App; `
    tar -C dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
    Remove-Item -Force aspnetcore.zip


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-nanoserver-1809

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.21567.15 `
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
