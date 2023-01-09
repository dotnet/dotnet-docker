# escape=`

FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64

ENV `
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=6.0.13

# Install .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Runtime/$Env:DOTNET_VERSION/dotnet-runtime-$Env:DOTNET_VERSION-win-x64.zip; `
        $dotnet_sha512 = '948ae03aec80f4cc1caf96dafce4a443c6f82a03c265ee61204ec1201ba61a4d9c9a2114ec5ba4ec4c56131bc3104d11ebfb5ae6f03429e365d30fa57a1163b4'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir $Env:ProgramFiles\dotnet; `
        tar -oxzf dotnet.zip -C $Env:ProgramFiles\dotnet; `
        Remove-Item -Force dotnet.zip

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
