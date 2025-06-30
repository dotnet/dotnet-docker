# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2025-amd64 AS installer

# Retrieve .NET Runtime
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnetFile = \"dotnet-runtime-9.0.6-win-x64.zip\"; `
        `
        Invoke-WebRequest -OutFile $dotnetFile https://builds.dotnet.microsoft.com/dotnet/Runtime/9.0.6/dotnet-runtime-9.0.6-win-x64.zip; `
        Invoke-WebRequest -OutFile 9.0.6-sha.txt https://builds.dotnet.microsoft.com/dotnet/checksums/9.0.6-sha.txt; `
        `
        $expectedHash = ( `
            (Get-Content 9.0.6-sha.txt | Where-Object { `
                    $_ -match \"$([regex]::Escape($dotnetFile))$\"; `
            }) -split '\s+' `
        )[0].ToUpper(); `
        $actualHash = (Get-FileHash $dotnetFile -Algorithm SHA512).Hash.ToUpper(); `
        `
        if ($expectedHash -ne $actualHash) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            Write-Host \"Expected: $expectedHash\"; `
            Write-Host \"Actual:   $actualHash\"; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file $dotnetFile --directory dotnet; `
        Remove-Item -Force $dotnetFile, 9.0.6-sha.txt"


# Runtime image
FROM mcr.microsoft.com/windows/nanoserver:ltsc2025-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=9.0.6

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
