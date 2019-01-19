# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:1809 AS installer

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core SDK
ENV DOTNET_SDK_VERSION 2.2.103

RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
    $dotnet_sha512 = 'a74d230ab184c6571e445c8e05361f1db32ffb9bd76baafe4d307ef42b7bf9cebd70e1aed37136caa485ebace185fea24b476907ff7bf522f3b485c8cf0c285b'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive dotnet.zip -DestinationPath dotnet; `
    Remove-Item -Force dotnet.zip

# Install artifacts credential provider
ENV NUGET_CREDENTIAL_PROVIDER_VERSION 0.1.11

RUN Invoke-WebRequest -OutFile microsoft.nuget.credentialprovider.zip https://azureartifactassets.blob.core.windows.net/credentialprovider/releases/download/$Env:NUGET_CREDENTIAL_PROVIDER_VERSION/Microsoft.Netcore2.NuGet.CredentialProvider.zip; `
    $credprovider_sha512 = '9733d659253415e6ed88e5b728591a1f7596baa6a500ffc390c844be224916c57a91f57d2fbf2e6395cc793b6390d72bc9d73b28810f9c8d2868c0cabec932c3'; `
    if ((Get-FileHash microsoft.nuget.credentialprovider.zip -Algorithm sha512).Hash -ne $credprovider_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED FOR CREDENTIAL PROVIDER!'; `
        exit 1; `
    }; `
    `
    Expand-Archive microsoft.nuget.credentialprovider.zip -DestinationPath credentialprovider; `
    Remove-Item -Force microsoft.nuget.credentialprovider.zip


# SDK image
FROM mcr.microsoft.com/windows/nanoserver:1809

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]

COPY --from=installer-env ["credentialprovider", "C:\\Users\\Public\\credentialprovider"]

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
