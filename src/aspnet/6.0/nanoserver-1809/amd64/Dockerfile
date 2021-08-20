# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:1809-amd64 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
RUN Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rc.1.21419.28/aspnetcore-runtime-6.0.0-rc.1.21419.28-win-x64.zip; `
    $aspnetcore_sha512 = '121aad832e0110eedc185cf4c0fc9a5523ef6433d489f43dce3fac2cb997bd1d7b86064cb30b9f4ae3339ebc7a067787a61db415d3e3fd7ef5f860ecab7a4637'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    mkdir dotnet/shared/Microsoft.AspNetCore.App; `
    tar -C dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
    Remove-Item -Force aspnetcore.zip


# ASP.NET Core image
FROM $REPO:6.0.0-rc.1-nanoserver-1809

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21419.28 `
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
