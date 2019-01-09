# escape=`

FROM mcr.microsoft.com/windows/nanoserver:sac2016

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.2.103

RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
    $dotnet_sha512 = 'a74d230ab184c6571e445c8e05361f1db32ffb9bd76baafe4d307ef42b7bf9cebd70e1aed37136caa485ebace185fea24b476907ff7bf522f3b485c8cf0c285b'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive dotnet.zip -DestinationPath $Env:ProgramFiles\dotnet; `
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
    Expand-Archive microsoft.nuget.credentialprovider.zip -DestinationPath $Env:UserProfile\.nuget; `
    Remove-Item -Force microsoft.nuget.credentialprovider.zip

RUN setx /M PATH $($Env:PATH + ';' + $Env:ProgramFiles + '\dotnet')

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help
