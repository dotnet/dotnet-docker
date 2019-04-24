# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:1803 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.603

RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
    $dotnet_sha512 = '54a579f74a88c4132852fc52b40a3fb43c016a0fa90f7021bed64d228a85d9b4ed5aff2fae91e8aa6af18c9e3b552ec9e34d96069756b37a4d32308324dc35ff'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive dotnet.zip -DestinationPath dotnet; `
    Remove-Item -Force dotnet.zip

# Install artifacts credential provider
ENV NUGET_CREDENTIAL_PROVIDER_VERSION 0.1.15

RUN Invoke-WebRequest -OutFile microsoft.nuget.credentialprovider.zip https://azureartifactassets.blob.core.windows.net/credentialprovider/releases/download/$Env:NUGET_CREDENTIAL_PROVIDER_VERSION/Microsoft.NetCore2.NuGet.CredentialProvider.zip; `
    $credprovider_sha512 = 'dc5cc9cb83bddc2d2d2b535f477a5a91df806e9e5c4db547c395ccc76c669709441c200b83512928792d3ba2711cfb7c754490795b05070f45c08b6eb0ed8586'; `
    if ((Get-FileHash microsoft.nuget.credentialprovider.zip -Algorithm sha512).Hash -ne $credprovider_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED FOR CREDENTIAL PROVIDER!'; `
        exit 1; `
    }; `
    `
    Expand-Archive microsoft.nuget.credentialprovider.zip -DestinationPath credentialprovider; `
    Remove-Item -Force microsoft.nuget.credentialprovider.zip


# SDK image
FROM mcr.microsoft.com/windows/nanoserver:1803

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]

COPY --from=installer ["/credentialprovider", "/Users/Public/credentialprovider"]

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # NuGet Credential provider path
    NUGET_PLUGIN_PATHS="C:\Users\Public\credentialprovider\plugins\netcore\CredentialProvider.Microsoft\CredentialProvider.Microsoft.dll" `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
