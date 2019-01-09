# escape=`

FROM mcr.microsoft.com/windows/nanoserver:sac2016

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install .NET Core
ENV DOTNET_VERSION 2.1.7

RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$Env:DOTNET_VERSION/dotnet-runtime-$Env:DOTNET_VERSION-win-x64.zip; `
    $dotnet_sha512 = '5d5b0dad9817b1b81650f86b7df0b5ff5ef14712dc78000dce1dac4fbc4ce45d1bc6cdb569fa883ce000aaea643dcf5f66a6179a298c0a16e0a69f4632a0bdb8'; `
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
    DOTNET_RUNNING_IN_CONTAINER=true
