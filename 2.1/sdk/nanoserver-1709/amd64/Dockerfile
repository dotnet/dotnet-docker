# escape=`

# Installer image
FROM microsoft/windowsservercore:1709 AS installer-env

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.300-preview2-008354

RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
    $dotnet_sha512 = 'bb388d0fdaad1f1bde653bd1611518f3f7a215bbfc133d03ba7980c1625dddd83770a7bcfb8b31cd2586a5b3970960f140681cfb4d537c23941e70850a6e44e8'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive dotnet.zip -DestinationPath dotnet; `
    Remove-Item -Force dotnet.zip


# SDK image
FROM microsoft/nanoserver:1709

COPY --from=installer-env ["dotnet", "C:\\Program Files\\dotnet"]

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator 
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

# Configure Kestrel web server to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80

# Enable detection of running in a container
ENV DOTNET_RUNNING_IN_CONTAINER true

# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER true

ENV NUGET_XMLDOC_MODE skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
