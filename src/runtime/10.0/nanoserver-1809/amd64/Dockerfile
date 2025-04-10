# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:1809-amd64 AS installer

# Retrieve .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnet_version = '10.0.0-preview.3.25171.5'; `
        Invoke-WebRequest -OutFile dotnet.zip https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-win-x64.zip; `
        $dotnet_sha512 = '586e24e04ada25753dad5bea6f9302771ffb08d6a0d8be998b68ab66a28d665b6fa82c2935c4a50e9030e6baff6ba052ac43144e93c720e64cec852de125c89b'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar -oxzf dotnet.zip -C dotnet; `
        Remove-Item -Force dotnet.zip


# Runtime image
FROM mcr.microsoft.com/windows/nanoserver:1809-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.3.25171.5

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
