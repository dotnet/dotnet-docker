# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:2004-amd64 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Runtime
RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21414.4/dotnet-runtime-6.0.0-rc.1.21414.4-win-x64.zip; `
    $dotnet_sha512 = '96fe937c6c71b78c9f339e1816390015a28b65ab7a7bb1fb14068e1e2dfb82294ea7cd0ff0c4ceb523cc6eda1df167566f3ba395d37ce0621aed9fa367605b4f'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    mkdir dotnet; `
    tar -C dotnet -oxzf dotnet.zip; `
    Remove-Item -Force dotnet.zip


# Runtime image
FROM mcr.microsoft.com/windows/nanoserver:2004-amd64

ENV `
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    DOTNET_VERSION=6.0.0-rc.1.21414.4

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
