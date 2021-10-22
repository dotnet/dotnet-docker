# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:2004-amd64 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
RUN Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21521.10/aspnetcore-runtime-6.0.0-rtm.21521.10-win-x64.zip; `
    $aspnetcore_sha512 = 'ca86b568ee27395e8d5ef978ffe1869515cd94d21a297d15e935822f7a2f91357bac1f6c7c844162c4dfd15111e04a230238083560e9a371564bda56b80bd267'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    mkdir dotnet/shared/Microsoft.AspNetCore.App; `
    tar -C dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
    Remove-Item -Force aspnetcore.zip


# ASP.NET Core image
FROM $REPO:6.0.0-nanoserver-2004

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21521.10 `
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
