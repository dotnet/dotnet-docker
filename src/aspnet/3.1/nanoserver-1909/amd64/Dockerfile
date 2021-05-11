# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:1909-amd64 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
RUN $aspnetcore_version = '3.1.15'; `
    Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-win-x64.zip; `
    $aspnetcore_sha512 = 'f6ff10fbf6a51308c6066bb68beab96c4caec8b54472e3f4f5e55638104ebd9871f88dd42806945307a66fe183b5b04f34eafa093d5a849f9e4db44843ebc639'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    mkdir dotnet/shared/Microsoft.AspNetCore.App; `
    tar -C dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
    Remove-Item -Force aspnetcore.zip


# Runtime image
FROM $REPO:3.1-nanoserver-1909

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
