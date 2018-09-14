# escape=`

# Installer image
FROM microsoft/windowsservercore:1709 AS installer-env

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core Runtime
ENV DOTNET_VERSION 2.2.0-preview3-26913-03

RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$Env:DOTNET_VERSION/dotnet-runtime-$Env:DOTNET_VERSION-win-x64.zip; `
    $dotnet_sha512 = '2e672dea38d54df5161a632db1769eeccfb2867ea437fd9f75638a8444964fd89c1a7aa3694ba988a01b5e1de30b87f6f60d2081593e21ab423bb7d257b71447'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive dotnet.zip -DestinationPath dotnet; `
    Remove-Item -Force dotnet.zip


# Runtime image
FROM microsoft/nanoserver:1709

COPY --from=installer-env ["dotnet", "C:\\Program Files\\dotnet"]

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator 
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

# Configure Kestrel web server to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
