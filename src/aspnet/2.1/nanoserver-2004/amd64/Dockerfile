# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:2004-amd64 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
ENV ASPNETCORE_VERSION=2.1.29

RUN Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNETCORE_VERSION/aspnetcore-runtime-$Env:ASPNETCORE_VERSION-win-x64.zip; `
    $aspnetcore_sha512 = '34a686fbe4d94493c3c02fc189f7dd577ca14211f85db128cd0eb72f085dc0d6c4d232b2f504bf60230e67faab2f54233ab45629fe5d0bbb39fa7d0832234982'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    mkdir dotnet; `
    tar -C dotnet -oxzf aspnetcore.zip; `
    Remove-Item -Force aspnetcore.zip


# Runtime image
FROM mcr.microsoft.com/windows/nanoserver:2004-amd64

# Note: Runtime image's SHELL is the CMD shell (different than the installer image).

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

ENV `
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # Deprecated, use `DOTNET_RUNNING_IN_CONTAINER` instead - https://github.com/dotnet/dotnet-docker/issues/677
    DOTNET_RUNNING_IN_CONTAINERS=true
