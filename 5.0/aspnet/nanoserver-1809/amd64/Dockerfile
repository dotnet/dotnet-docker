# escape=`

ARG REPO=mcr.microsoft.com/dotnet/core/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:1809 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
RUN $aspnetcore_version = '5.0.0-preview.2.20155.1'; `
    Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-win-x64.zip; `
    $aspnetcore_sha512 = '5340237e3f5ed17833738e65c68c2ad90c04db6a70181c89fdd8292699bf2885798d1dfaf2eb8edbd538c9a93eefddf7f573c4f4dfbcdf75c7e54019139be136'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive aspnetcore.zip -DestinationPath dotnet; `
    Remove-Item -Force aspnetcore.zip


# Runtime image
FROM $REPO:5.0-nanoserver-1809

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
