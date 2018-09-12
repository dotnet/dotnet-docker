# escape=`
FROM microsoft/nanoserver:sac2016

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install ASP.NET Core Runtime
ENV ASPNETCORE_VERSION 2.1.5-rtm-31008

RUN Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$Env:ASPNETCORE_VERSION/aspnetcore-runtime-$Env:ASPNETCORE_VERSION-win-x64.zip; `
    $aspnetcore_sha512 = 'b1c8e085f85594adfc747ad4d4d99a1e84b6a50f20764b1fb5679fd2be986a4c1d6cde3b5b8cf0ddd7cd20840c1854af4f9a34048ef4ed4b9351dee5fcac2e59'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive aspnetcore.zip -DestinationPath  $Env:ProgramFiles\dotnet; `
    Remove-Item -Force aspnetcore.zip
RUN setx /M PATH $($Env:PATH + ';' + $Env:ProgramFiles + '\dotnet')

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # Deprecated, use `DOTNET_RUNNING_IN_CONTAINER` instead - https://github.com/dotnet/dotnet-docker/issues/677
    DOTNET_RUNNING_IN_CONTAINERS=true
