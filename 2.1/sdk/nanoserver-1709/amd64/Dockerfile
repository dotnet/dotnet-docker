# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:1709 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.503

RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
    $dotnet_sha512 = '29e44a4d6bd81ace5f7f5b5be946e7fc81325f4563d375d6809150bfc0552c70e07467770c8c6b44127b5b1c01d93ca14e1c98ba527313dc093db8942358760c'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive dotnet.zip -DestinationPath dotnet; `
    Remove-Item -Force dotnet.zip

# Install artifacts credential provider
ENV NUGET_CREDENTIAL_PROVIDER_VERSION 0.1.8

RUN [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; `
    Invoke-WebRequest -OutFile microsoft.nuget.credentialprovider.zip https://github.com/Microsoft/artifacts-credprovider/releases/download/$Env:NUGET_CREDENTIAL_PROVIDER_VERSION/Microsoft.NuGet.CredentialProvider.zip; `
    $credprovider_sha512 = '853e2b0b348742520e87ea557e6c1f38306fed3544d87dc10ec2823942caaea4d41b2c3912b8c4802e7211a38707706d5e6fdd3806ebf4b42e939960c0ca5d35'; `
    if ((Get-FileHash microsoft.nuget.credentialprovider.zip -Algorithm sha512).Hash -ne $credprovider_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED FOR CREDENTIAL PROVIDER!'; `
        exit 1; `
    }; `
    `
    Expand-Archive microsoft.nuget.credentialprovider.zip -DestinationPath credentialprovider; `
    Remove-Item -Force microsoft.nuget.credentialprovider.zip

# SDK image
FROM mcr.microsoft.com/windows/nanoserver:1709

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]

COPY --from=installer-env ["credentialprovider\\plugins\\netcore", "C:\\Users\\Public\\credentialprovider\\plugins\\netcore"]

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
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # Setting environment variable for credential provider
    NUGET_PLUGIN_PATHS="C:\Users\Public\credentialprovider\\plugins\\netcore\CredentialProvider.Microsoft\CredentialProvider.Microsoft.dll"

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
