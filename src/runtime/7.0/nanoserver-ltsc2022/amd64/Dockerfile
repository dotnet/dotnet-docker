# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Retrieve .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnet_version = '7.0.0-preview.5.22272.3'; `
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-win-x64.zip; `
        $dotnet_sha512 = 'f8c1ad3176238f91deebfd59c86a75699669a842028b825c3e1658d4ed26c6f1be9f371c3e46fa1b7bbeda50bb44128829476a3616ab04c47a478b20e8934e3e'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar -oxzf dotnet.zip -C dotnet; `
        Remove-Item -Force dotnet.zip


# Runtime image
FROM mcr.microsoft.com/windows/nanoserver:ltsc2022-amd64

ENV `
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=7.0.0-preview.5.22272.3

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
