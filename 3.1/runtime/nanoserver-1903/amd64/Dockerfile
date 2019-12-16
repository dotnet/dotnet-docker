# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:1903 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core Runtime
RUN $dotnet_version = '3.1.1-servicing.19575.1'; `
    Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-win-x64.zip; `
    $dotnet_sha512 = 'aa0eedee5e997f67f7febbbd13444082e17ceb4bc109f9be3bb4f2994009f7751d930772c783a1ee4045640ee633f89d353c7dbfd629e5f95c4a06675dc97c7a'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive dotnet.zip -DestinationPath dotnet; `
    Remove-Item -Force dotnet.zip


# Runtime image
FROM mcr.microsoft.com/windows/nanoserver:1903

ENV `
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
