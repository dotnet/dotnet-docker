# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:1809-amd64 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
RUN Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rc.2.21480.10/aspnetcore-runtime-6.0.0-rc.2.21480.10-win-x64.zip; `
    $aspnetcore_sha512 = '453ed645205869fe933c5071cd97b267031d0e21f961df7509840f923ad15a2cb8ae09bfb7083333f6fad5236aaa08b7964dbd9784c2f54b9607fb451efec5d4'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    mkdir dotnet/shared/Microsoft.AspNetCore.App; `
    tar -C dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
    Remove-Item -Force aspnetcore.zip


# ASP.NET Core image
FROM $REPO:6.0.0-rc.2-nanoserver-1809

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.2.21480.10 `
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
