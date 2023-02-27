# escape=`

FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=8.0.0-preview.2.23126.3

# Install .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetbuilds.azureedge.net/public/Runtime/$Env:DOTNET_VERSION/dotnet-runtime-$Env:DOTNET_VERSION-win-x64.zip; `
        $dotnet_sha512 = 'b02f334054cfd5b515bd06b57351b59a8306c9d01f0a4c2e38a8af9ebf81b493168da775ea2a2c04633bac61a4e4f4bca90390e86c24a6f9e0b7bab1a98e8332'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir $Env:ProgramFiles\dotnet; `
        tar -oxzf dotnet.zip -C $Env:ProgramFiles\dotnet; `
        Remove-Item -Force dotnet.zip

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
