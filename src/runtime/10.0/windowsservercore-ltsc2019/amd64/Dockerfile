# escape=`

FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=9.0.1

# Install .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile dotnet.zip https://builds.dotnet.microsoft.com/dotnet/Runtime/$Env:DOTNET_VERSION/dotnet-runtime-$Env:DOTNET_VERSION-win-x64.zip; `
        $dotnet_sha512 = '30b880c3cd6c39355e92b5422e8c044a26fba1da15b4f1f8a89dc4622962c8a3537b075064c33c8493d8bbc909ae8c135a5533110080e95ef31e3407eade291f'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir $Env:ProgramFiles\dotnet; `
        tar -oxzf dotnet.zip -C $Env:ProgramFiles\dotnet; `
        Remove-Item -Force dotnet.zip

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
