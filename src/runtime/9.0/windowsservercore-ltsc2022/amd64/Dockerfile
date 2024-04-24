# escape=`

FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-preview.4.24223.11

# Install .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetbuilds.azureedge.net/public/Runtime/$Env:DOTNET_VERSION/dotnet-runtime-$Env:DOTNET_VERSION-win-x64.zip; `
        $dotnet_sha512 = '9512e2f4f80531c97bc2890680cd77a7e409017428a9f67acb7cecd7fc49f761f91bd6ece601db4e15c812fe8b1a179cac39509bb42904ddbfd50369b247e455'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir $Env:ProgramFiles\dotnet; `
        tar -oxzf dotnet.zip -C $Env:ProgramFiles\dotnet; `
        Remove-Item -Force dotnet.zip

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
